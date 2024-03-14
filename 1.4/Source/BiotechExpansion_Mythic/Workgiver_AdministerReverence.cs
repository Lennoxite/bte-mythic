using System;
using Verse;
using Verse.AI;
using RimWorld;
namespace BTE_MY
{
	public class Workgiver_AdministerReverence : WorkGiver_Scanner
	{
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
			}
		}

		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.ClosestTouch;
			}
		}

		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			if (pawn2 == null || pawn2 == pawn)
			{
				return false;
			}
			Pawn_GeneTracker genes = pawn2.genes;
			Gene_Reverence gene_Reverence = (genes != null) ? genes.GetFirstGeneOfType<Gene_Reverence>() : null;
			if (gene_Reverence == null)
			{
				return false;
			}
			if (gene_Reverence.ValuePercent >= 0.95f)
			{
				return false;
			}
			if (!forced && gene_Reverence.Value >= 0.25f)
			{
				return false;
			}
			if (!FeedPatientUtility.ShouldBeFed(pawn2))
			{
				return false;
			}
			if (!gene_Reverence.ReverenceFuelAllowed)
			{
				return false;
			}
			if (!gene_Reverence.ShouldConsumeReverenceNow())
			{
				JobFailReason.Is("Not allowed to consume reverence.", null);
				return false;
			}
			if (!pawn.CanReserve(t, 1, -1, null, forced))
			{
				return false;
			}
			if (GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(BTEMY_ThingDefOf.BTEMy_ReverenceFuel), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false), 9999f, (Thing pack) => !pack.IsForbidden(pawn) && pawn.CanReserve(pack, 1, -1, null, false), null, 0, -1, false, RegionType.Set_Passable, false) == null)
			{
				JobFailReason.Is("No revered motes.", null);
				return false;
			}
			return true;
		}

		// Token: 0x060041A1 RID: 16801 RVA: 0x00177CD0 File Offset: 0x00175ED0
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn t2 = (Pawn)t;
			Thing thing = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(BTEMY_ThingDefOf.BTEMy_ReverenceFuel), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false), 9999f, (Thing pack) => !pack.IsForbidden(pawn) && pawn.CanReserve(pack, 1, -1, null, false), null, 0, -1, false, RegionType.Set_Passable, false);
			if (thing != null)
			{
				Job job = JobMaker.MakeJob(JobDefOf.FeedPatient, thing, t2);
				job.count = 1;
				return job;
			}
			return null;
		}

		// Token: 0x0400258B RID: 9611
		private const float MinLevelForFeedingReverenceUnforced = 0.25f;

		// Token: 0x0400258C RID: 9612
		private const float ReverencePctMax = 0.95f;
	}
}
