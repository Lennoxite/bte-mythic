using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using RimWorld;
namespace BTE_MY
{
	public class JobGiver_GetReverence : ThinkNode_JobGiver
	{
		public static float ReverencePackReverenceGain
		{
			get
			{
				if (JobGiver_GetReverence.cachedReverencePackReverenceGain == null)
				{
					if (!ModsConfig.BiotechActive)
					{
						JobGiver_GetReverence.cachedReverencePackReverenceGain = new float?(0f);
					}
					else
					{
						IngestibleProperties ingestible = BTEMY_ThingDefOf.BTEMy_ReverenceFuel.ingestible;
						object obj;
						if (ingestible == null)
						{
							obj = null;
						}
						else
						{
							List<IngestionOutcomeDoer> outcomeDoers = ingestible.outcomeDoers;
							if (outcomeDoers == null)
							{
								obj = null;
							}
							else
							{
								obj = outcomeDoers.FirstOrDefault((IngestionOutcomeDoer x) => x is IngestionOutcomeDoer_OffsetReverence);
							}
						}
						IngestionOutcomeDoer_OffsetReverence ingestionOutcomeDoer_OffsetReverence = obj as IngestionOutcomeDoer_OffsetReverence;
						if (ingestionOutcomeDoer_OffsetReverence == null)
						{
							JobGiver_GetReverence.cachedReverencePackReverenceGain = new float?(0f);
						}
						else
						{
							JobGiver_GetReverence.cachedReverencePackReverenceGain = new float?(ingestionOutcomeDoer_OffsetReverence.offset);
						}
					}
				}
				return JobGiver_GetReverence.cachedReverencePackReverenceGain.Value;
			}
		}

		public static void ResetStaticData()
		{
			JobGiver_GetReverence.cachedReverencePackReverenceGain = null;
		}

		public override float GetPriority(Pawn pawn)
		{
			if (!ModsConfig.BiotechActive)
			{
				return 0f;
			}
			Pawn_GeneTracker genes = pawn.genes;
			if (((genes != null) ? genes.GetFirstGeneOfType<Gene_Reverence>() : null) == null)
			{
				return 0f;
			}
			return 9.1f;
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!ModsConfig.BiotechActive)
			{
				return null;
			}
			Pawn_GeneTracker genes = pawn.genes;
			Gene_Reverence gene_Reverence = (genes != null) ? genes.GetFirstGeneOfType<Gene_Reverence>() : null;
			if (gene_Reverence == null)
			{
				return null;
			}
			if (!gene_Reverence.ShouldConsumeReverenceNow())
			{
				return null;
			}
			if (gene_Reverence.ReverenceFuelAllowed)
			{
				Thing ReverencePack = this.GetReverencePack(pawn);
				if (ReverencePack != null)
				{
					Job job = JobMaker.MakeJob(JobDefOf.Ingest, ReverencePack);
					job.count = Mathf.Min(ReverencePack.stackCount, Mathf.CeilToInt((gene_Reverence.Max - gene_Reverence.Value) / JobGiver_GetReverence.ReverencePackReverenceGain));
					return job;
				}
			}
			return null;
		}

		private Thing GetReverencePack(Pawn pawn)
		{
			Thing carriedThing = pawn.carryTracker.CarriedThing;
			if (carriedThing != null && carriedThing.def == BTEMY_ThingDefOf.BTEMy_ReverenceFuel)
			{
				return carriedThing;
			}
			for (int i = 0; i < pawn.inventory.innerContainer.Count; i++)
			{
				if (pawn.inventory.innerContainer[i].def == BTEMY_ThingDefOf.BTEMy_ReverenceFuel)
				{
					return pawn.inventory.innerContainer[i];
				}
			}
			return GenClosest.ClosestThing_Global_Reachable(pawn.Position, pawn.Map, pawn.Map.listerThings.ThingsOfDef(BTEMY_ThingDefOf.BTEMy_ReverenceFuel), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false), 9999f, (Thing t) => pawn.CanReserve(t, 1, -1, null, false) && !t.IsForbidden(pawn), null);
		}



		private static float? cachedReverencePackReverenceGain;
	}
}