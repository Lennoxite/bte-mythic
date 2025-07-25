﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;
using RimWorld;

namespace BTE_MY
{
	[StaticConstructorOnStartup]
	public class GeneGizmo_ResourceAurum : GeneGizmo_Resource
	{
		public GeneGizmo_ResourceAurum(Gene_Resource gene, List<IGeneResourceDrain> drainGenes, Color barColor, Color barhighlightColor) : base(gene, drainGenes, barColor, barhighlightColor)
		{
			
		}

        private static bool draggingBar;
        protected override bool DraggingBar
        {
            get
            {
                return draggingBar;
            }
            set
            {
                draggingBar = value;
            }
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
            _ = (MainTabWindow_Inspect)MainButtonDefOf.Inspect.TabWindow;
            if (MapGizmoUtility.LastMouseOverGizmo is Command_Ability command_Ability && gene.Max != 0f)
            {
                foreach (CompAbilityEffect effectComp in command_Ability.Ability.EffectComps)
                {
                    if (effectComp is CompAbilityEffect_AurumCost compAbilityEffect_AurumCost && compAbilityEffect_AurumCost.Props.aurumCost > float.Epsilon)
                    {
                        Rect rect = barRect.ContractedBy(3f);
                        float width = rect.width;
                        float num3 = gene.Value / gene.Max;
                        rect.xMax = rect.xMin + width * num3;
                        float num4 = Mathf.Min(compAbilityEffect_AurumCost.Props.aurumCost / gene.Max, 1f);
                        rect.xMin = Mathf.Max(rect.xMin, rect.xMax - width * num4);
                        GUI.color = new Color(1f, 1f, 1f, num2 * 0.7f);
                        GenUI.DrawTextureWithMaterial(rect, GeneGizmo_ResourceAurum.AurumCostTex, null);
                        GUI.color = Color.white;
                        break;
                    }
                }
            }
            return result;
		}

		protected override void DrawHeader(Rect labelRect, ref bool mouseOverAnyHighlightableElement)
		{
			Gene_Aurum AurumGene;
			if ((this.gene.pawn.IsColonistPlayerControlled || this.gene.pawn.IsPrisonerOfColony) && (AurumGene = (this.gene as Gene_Aurum)) != null)
			{
				labelRect.xMax -= 24f;
				Rect rect = new Rect(labelRect.xMax, labelRect.y, 24f, 24f);
				Widgets.DefIcon(rect, BTEMY_ThingDefOf.BTEMy_AurumFuel, null, 1f, null, false, null, null, null);
				GUI.DrawTexture(new Rect(rect.center.x, rect.y, rect.width / 2f, rect.height / 2f), AurumGene.aurumFuelAllowed ? Widgets.CheckboxOnTex : Widgets.CheckboxOffTex);
				if (Widgets.ButtonInvisible(rect, true))
				{
					AurumGene.aurumFuelAllowed = !AurumGene.aurumFuelAllowed;
					if (AurumGene.aurumFuelAllowed)
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
					string onOff = (AurumGene.aurumFuelAllowed ? "On" : "Off");
					TooltipHandler.TipRegion(rect, () => "Pawn will consume aurum material: " + onOff, 828267373);
					mouseOverAnyHighlightableElement = true;
				}
			}
			base.DrawHeader(labelRect, ref mouseOverAnyHighlightableElement);
		}

		protected override string GetTooltip()
		{
			this.tmpDrainGenes.Clear();
			string text = string.Format("{0}: {1} / {2}\n", this.gene.ResourceLabel.CapitalizeFirst().Colorize(ColoredText.TipSectionTitleColor), this.gene.ValueForDisplay, this.gene.MaxForDisplay);
			if (this.gene.pawn.IsColonistPlayerControlled || this.gene.pawn.IsPrisonerOfColony)
			{
				if (this.gene.targetValue <= 0f)
				{
					text += "Never Consume Aurum".ToString();
				}
				else
				{
					text = text + ("Consume Aurum Below" + ": ") + this.gene.PostProcessValue(this.gene.targetValue);
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

		private static readonly Texture2D AurumCostTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.2f, 0.9f, 0.9f));

		private const float TotalPulsateTime = 0.85f;

		private List<Pair<IGeneResourceDrain, float>> tmpDrainGenes = new List<Pair<IGeneResourceDrain, float>>();
	}
}
