using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;
using BTE_MMLA;
namespace BTE_MY
{

    [StaticConstructorOnStartup]
    public static class AurumUtility
    {
		public static void OffsetAurum(Pawn pawn, float offset)
		{
			if (!ModsConfig.BiotechActive)
			{
				return;
			}
			Pawn_GeneTracker genes = pawn.genes;
			Gene_AurumDrainMod gene_AurumDrain = (genes != null) ? genes.GetFirstGeneOfType<Gene_AurumDrainMod>() : null;
			if (gene_AurumDrain != null)
			{
				AurumUtility.OffsetResource(gene_AurumDrain, offset);
				return;
			}
			Gene_Aurum gene_Aurum = (genes != null) ? genes.GetFirstGeneOfType<Gene_Aurum>() : null;
			if (gene_Aurum != null)
			{
				gene_Aurum.Value += offset;
			}
		}

		public static void OffsetAurumWithModifier(Pawn pawn, float offset)
		{

			offset *= pawn.GetStatValue(BTEMY_StatDefOf.BTEMy_AurumGainFactor);
			OffsetAurum(pawn, offset);
		}

		public static float AurumCost(Ability ab)
		{
			if (ab.comps != null)
			{
				using (List<AbilityComp>.Enumerator enumerator = ab.comps.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						CompAbilityEffect_AurumCost compAbilityEffect_AurumCost;
						if ((compAbilityEffect_AurumCost = (enumerator.Current as CompAbilityEffect_AurumCost)) != null)
						{
							return compAbilityEffect_AurumCost.Props.aurumCost;
						}
					}
				}
			}
			return 0f;
		}

		//Have to make our own, since the base GeneResourceDrainUtility only gives Hemogen craving.

		public static void TickResourceDrain(IGeneResourceDrain drain)
		{
			if (drain.CanOffset)
			{
				if (drain.Resource == null)
				{
					return;
				}
				AurumUtility.OffsetResource(drain, -drain.ResourceLossPerDay / 60000f);
			}
		}

		public static void PostResourceOffset(IGeneResourceDrain drain, float oldValue)
		{
			if (oldValue > 0f && drain.Resource.Value <= 0f)
			{
				Pawn pawn = drain.Pawn;
				if (!pawn.health.hediffSet.HasHediff(BTEMY_HediffDefOf.BTEMy_AurumThirst, false))
				{
					pawn.health.AddHediff(BTEMY_HediffDefOf.BTEMy_AurumThirst, null, null, null);
				}
			}
		}

		public static void OffsetResource(IGeneResourceDrain drain, float amnt)
		{
			float value = drain.Resource.Value;
			drain.Resource.Value += amnt;
			AurumUtility.PostResourceOffset(drain, value);
		}

		public static IEnumerable<Gizmo> GetResourceDrainGizmos(IGeneResourceDrain drain)
		{
			if (DebugSettings.ShowDevGizmos && drain.Resource != null)
			{
				Gene_Resource resource = drain.Resource;
				yield return new Command_Action
				{
					defaultLabel = "DEV: " + resource.ResourceLabel + " -10",
					action = delegate ()
					{
						AurumUtility.OffsetResource(drain, -0.1f);
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "DEV: " + resource.ResourceLabel + " +10",
					action = delegate ()
					{
						AurumUtility.OffsetResource(drain, 0.1f);
					}
				};
				resource = null;
			}
			yield break;
		}
	}


}


