using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;
using RimWorld;

namespace BTE_MY
{
	[StaticConstructorOnStartup]
	public class GeneGizmo_ResourceReverence : GeneGizmo_Resource
	{
		public GeneGizmo_ResourceReverence(Gene_Resource gene, List<IGeneResourceDrain> drainGenes, Color barColor, Color barhighlightColor) : base(gene, drainGenes, barColor, barhighlightColor)
		{
			this.draggableBar = true;
		}

		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			GizmoResult result = base.GizmoOnGUI(topLeft, maxWidth, parms);
			float num = Mathf.Repeat(Time.time, 0.85f);
			float num2 = 1f;
			if (num < 0.1f)
			{
				num2 = num / 0.1f;
			}
			else if (num >= 0.25f)
			{
				num2 = 1f - (num - 0.25f) / 0.6f;
			}
			MainTabWindow_Inspect mainTabWindow_Inspect = (MainTabWindow_Inspect)MainButtonDefOf.Inspect.TabWindow;
			Command_Ability command_Ability = ((mainTabWindow_Inspect != null) ? mainTabWindow_Inspect.LastMouseoverGizmo : null) as Command_Ability;
			if (command_Ability != null && this.gene.Max != 0f)
			{
				using (List<CompAbilityEffect>.Enumerator enumerator = command_Ability.Ability.EffectComps.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						CompAbilityEffect_ReverenceCost compAbilityEffect_ReverenceCost;
						if ((compAbilityEffect_ReverenceCost = (enumerator.Current as CompAbilityEffect_ReverenceCost)) != null && compAbilityEffect_ReverenceCost.Props.reverenceCost > 1E-45f)
						{
							Rect rect = this.barRect.ContractedBy(3f);
							float width = rect.width;
							float num3 = this.gene.Value / this.gene.Max;
							rect.xMax = rect.xMin + width * num3;
							float num4 = Mathf.Min(compAbilityEffect_ReverenceCost.Props.reverenceCost / this.gene.Max, 1f);
							rect.xMin = Mathf.Max(rect.xMin, rect.xMax - width * num4);
							GUI.color = new Color(1f, 1f, 1f, num2 * 0.7f);
							GenUI.DrawTextureWithMaterial(rect, GeneGizmo_ResourceReverence.ReverenceCostTex, null, default(Rect));
							GUI.color = Color.white;
							break;
						}
					}
				}
			}
			return result;
		}

		protected override void DrawLabel(Rect labelRect, ref bool mouseOverAnyHighlightableElement)
		{
			Gene_Reverence ReverenceGene;
			if ((this.gene.pawn.IsColonistPlayerControlled || this.gene.pawn.IsPrisonerOfColony) && (ReverenceGene = (this.gene as Gene_Reverence)) != null)
			{
				labelRect.xMax -= 24f;
				Rect rect = new Rect(labelRect.xMax, labelRect.y, 24f, 24f);
				Widgets.DefIcon(rect, BTEMY_ThingDefOf.BTEMy_ReverenceFuel, null, 1f, null, false, null, null, null);
				GUI.DrawTexture(new Rect(rect.center.x, rect.y, rect.width / 2f, rect.height / 2f), ReverenceGene.ReverenceFuelAllowed ? Widgets.CheckboxOnTex : Widgets.CheckboxOffTex);
				if (Widgets.ButtonInvisible(rect, true))
				{
					ReverenceGene.ReverenceFuelAllowed = !ReverenceGene.ReverenceFuelAllowed;
					if (ReverenceGene.ReverenceFuelAllowed)
					{
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					}
					else
					{
						SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
					}
				}
				if (Mouse.IsOver(rect))
				{
					Widgets.DrawHighlight(rect);
					string onOff = (ReverenceGene.ReverenceFuelAllowed ? "On" : "Off");
					TooltipHandler.TipRegion(rect, () => "Pawn will consume Reverence material: " + onOff, 828267373);
					mouseOverAnyHighlightableElement = true;
				}
			}
			base.DrawLabel(labelRect, ref mouseOverAnyHighlightableElement);
		}

		protected override string GetTooltip()
		{
			this.tmpDrainGenes.Clear();
			string text = string.Format("{0}: {1} / {2}\n", this.gene.ResourceLabel.CapitalizeFirst().Colorize(ColoredText.TipSectionTitleColor), this.gene.ValueForDisplay, this.gene.MaxForDisplay);
			if (this.gene.pawn.IsColonistPlayerControlled || this.gene.pawn.IsPrisonerOfColony)
			{
				if (this.gene.targetValue <= 0f)
				{
					text += "Never Consume Reverence".ToString();
				}
				else
				{
					text = text + ("Consume Reverence Below" + ": ") + this.gene.PostProcessValue(this.gene.targetValue);
				}
			}
			if (!this.drainGenes.NullOrEmpty<IGeneResourceDrain>())
			{
				float num = 0f;
				foreach (IGeneResourceDrain geneResourceDrain in this.drainGenes)
				{
					if (geneResourceDrain.CanOffset)
					{
						this.tmpDrainGenes.Add(new Pair<IGeneResourceDrain, float>(geneResourceDrain, geneResourceDrain.ResourceLossPerDay));
						num += geneResourceDrain.ResourceLossPerDay;
					}
				}
				if (num != 0f)
				{
					string text2 = (num < 0f) ? "RegenerationRate".Translate() : "DrainRate".Translate();
					text = string.Concat(new string[]
					{
						text,
						"\n\n",
						text2,
						": ",
						"PerDay".Translate(Mathf.Abs(this.gene.PostProcessValue(num))).Resolve()
					});
					foreach (Pair<IGeneResourceDrain, float> pair in this.tmpDrainGenes)
					{
						text = string.Concat(new string[]
						{
							text,
							"\n  - ",
							pair.First.DisplayLabel.CapitalizeFirst(),
							": ",
							"PerDay".Translate(this.gene.PostProcessValue(-pair.Second).ToStringWithSign()).Resolve()
						});
					}
				}
			}
			if (!this.gene.def.resourceDescription.NullOrEmpty())
			{
				text = text + "\n\n" + this.gene.def.resourceDescription.Formatted(this.gene.pawn.Named("PAWN")).Resolve();
			}
			return text;
		}

		private static readonly Texture2D ReverenceCostTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.2f, 0.9f, 0.9f));

		private const float TotalPulsateTime = 0.85f;

		private List<Pair<IGeneResourceDrain, float>> tmpDrainGenes = new List<Pair<IGeneResourceDrain, float>>();
	}
}
