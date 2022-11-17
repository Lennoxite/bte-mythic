using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using RimWorld;
namespace BTE_MY
{
	// Token: 0x020008ED RID: 2285
	public class JobGiver_GetAurum : ThinkNode_JobGiver
	{
		// Token: 0x17000B4F RID: 2895
		// (get) Token: 0x06003F00 RID: 16128 RVA: 0x00169768 File Offset: 0x00167968
		public static float AurumPackAurumGain
		{
			get
			{
				if (JobGiver_GetAurum.cachedAurumPackAurumGain == null)
				{
					if (!ModsConfig.BiotechActive)
					{
						JobGiver_GetAurum.cachedAurumPackAurumGain = new float?(0f);
					}
					else
					{
						IngestibleProperties ingestible = BTEMY_ThingDefOf.BTEMy_AurumFuel.ingestible;
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
								obj = outcomeDoers.FirstOrDefault((IngestionOutcomeDoer x) => x is IngestionOutcomeDoer_OffsetAurum);
							}
						}
						IngestionOutcomeDoer_OffsetAurum ingestionOutcomeDoer_OffsetAurum = obj as IngestionOutcomeDoer_OffsetAurum;
						if (ingestionOutcomeDoer_OffsetAurum == null)
						{
							JobGiver_GetAurum.cachedAurumPackAurumGain = new float?(0f);
						}
						else
						{
							JobGiver_GetAurum.cachedAurumPackAurumGain = new float?(ingestionOutcomeDoer_OffsetAurum.offset);
						}
					}
				}
				return JobGiver_GetAurum.cachedAurumPackAurumGain.Value;
			}
		}

		public static void ResetStaticData()
		{
			JobGiver_GetAurum.cachedAurumPackAurumGain = null;
		}

		public override float GetPriority(Pawn pawn)
		{
			if (!ModsConfig.BiotechActive)
			{
				return 0f;
			}
			Pawn_GeneTracker genes = pawn.genes;
			if (((genes != null) ? genes.GetFirstGeneOfType<Gene_Aurum>() : null) == null)
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
			Gene_Aurum gene_Aurum = (genes != null) ? genes.GetFirstGeneOfType<Gene_Aurum>() : null;
			if (gene_Aurum == null)
			{
				return null;
			}
			if (!gene_Aurum.ShouldConsumeAurumNow())
			{
				return null;
			}
			if (gene_Aurum.aurumFuelAllowed)
			{
				Thing AurumPack = this.GetAurumPack(pawn);
				if (AurumPack != null)
				{
					Job job = JobMaker.MakeJob(JobDefOf.Ingest, AurumPack);
					job.count = Mathf.Min(AurumPack.stackCount, Mathf.CeilToInt((gene_Aurum.Max - gene_Aurum.Value) / JobGiver_GetAurum.AurumPackAurumGain));
					return job;
				}
			}
			return null;
		}

		private Thing GetAurumPack(Pawn pawn)
		{
			Thing carriedThing = pawn.carryTracker.CarriedThing;
			if (carriedThing != null && carriedThing.def == BTEMY_ThingDefOf.BTEMy_AurumFuel)
			{
				return carriedThing;
			}
			for (int i = 0; i < pawn.inventory.innerContainer.Count; i++)
			{
				if (pawn.inventory.innerContainer[i].def == BTEMY_ThingDefOf.BTEMy_AurumFuel)
				{
					return pawn.inventory.innerContainer[i];
				}
			}
			return GenClosest.ClosestThing_Global_Reachable(pawn.Position, pawn.Map, pawn.Map.listerThings.ThingsOfDef(BTEMY_ThingDefOf.BTEMy_AurumFuel), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false), 9999f, (Thing t) => pawn.CanReserve(t, 1, -1, null, false) && !t.IsForbidden(pawn), null);
		}



		private static float? cachedAurumPackAurumGain;
	}
}