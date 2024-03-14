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
    public static class ReverenceUtility
    {
		public static void OffsetReverence(Pawn pawn, float offset)
		{
			if (!ModsConfig.BiotechActive)
			{
				return;
			}
			Pawn_GeneTracker genes = pawn.genes;
			Gene_ReverenceDrainMod gene_ReverenceDrain = (genes != null) ? genes.GetFirstGeneOfType<Gene_ReverenceDrainMod>() : null;
			if (gene_ReverenceDrain != null)
			{
				ReverenceUtility.OffsetResource(gene_ReverenceDrain, offset);
				return;
			}
			Gene_Reverence gene_Reverence = (genes != null) ? genes.GetFirstGeneOfType<Gene_Reverence>() : null;
			if (gene_Reverence != null)
			{
				gene_Reverence.Value += offset;
			}
		}

		public static void OffsetReverenceWithModifier(Pawn pawn, float offset)
		{

			//offset *= pawn.GetStatValue(BTEMY_StatDefOf.BTEMy_ReverenceGainFactor);
			OffsetReverence(pawn, offset);
		}

		public static float ReverenceCost(Ability ab)
		{
			if (ab.comps != null)
			{
				using (List<AbilityComp>.Enumerator enumerator = ab.comps.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						CompAbilityEffect_ReverenceCost compAbilityEffect_ReverenceCost;
						if ((compAbilityEffect_ReverenceCost = (enumerator.Current as CompAbilityEffect_ReverenceCost)) != null)
						{
							return compAbilityEffect_ReverenceCost.Props.reverenceCost;
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
				ReverenceUtility.OffsetResource(drain, -drain.ResourceLossPerDay / 60000f);
			}
		}

		public static void PostResourceOffset(IGeneResourceDrain drain, float oldValue)
		{
			if (oldValue > 0f && drain.Resource.Value <= 0f)
			{
				Pawn pawn = drain.Pawn;
				if (!pawn.health.hediffSet.HasHediff(BTEMY_HediffDefOf.BTEMy_ReveredDesire, false))
				{
					pawn.health.AddHediff(BTEMY_HediffDefOf.BTEMy_ReveredDesire, null, null, null);
				}
			}
		}

		public static void OffsetResource(IGeneResourceDrain drain, float amnt)
		{
			float value = drain.Resource.Value;
			drain.Resource.Value += amnt;
			ReverenceUtility.PostResourceOffset(drain, value);
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
						ReverenceUtility.OffsetResource(drain, -0.1f);
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "DEV: " + resource.ResourceLabel + " +10",
					action = delegate ()
					{
						ReverenceUtility.OffsetResource(drain, 0.1f);
					}
				};
				resource = null;
			}
			yield break;
		}
	}


}


