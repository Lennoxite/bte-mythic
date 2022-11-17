using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using RimWorld;

namespace BTE_MY
{
	// Token: 0x0200096E RID: 2414
	public class WorkGiver_Warden_DeliverAurum : WorkGiver_Warden
	{
		// Token: 0x0600414A RID: 16714 RVA: 0x00175478 File Offset: 0x00173678
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!ModsConfig.BiotechActive)
			{
				return null;
			}
			if (!base.ShouldTakeCareOfPrisoner(pawn, t, false))
			{
				return null;
			}
			Pawn prisoner = (Pawn)t;
			if (!prisoner.guest.CanBeBroughtFood || !prisoner.Position.IsInPrisonCell(prisoner.Map))
			{
				return null;
			}
			if (WardenFeedUtility.ShouldBeFed(prisoner))
			{
				return null;
			}
			Pawn_GeneTracker genes = prisoner.genes;
			Gene_Aurum gene_Aurum = ((genes != null) ? genes.GetGene(BTEMY_GeneDefOf.BTEMy_AurumThirst) : null) as Gene_Aurum;
			if (gene_Aurum == null)
			{
				return null;
			}
			if (!gene_Aurum.aurumFuelAllowed)
			{
				return null;
			}
			if (!gene_Aurum.ShouldConsumeAurumNow())
			{
				return null;
			}
			if (this.AurumPackAlreadyAvailableFor(prisoner))
			{
				return null;
			}
			Thing thing = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(BTEMY_ThingDefOf.BTEMy_AurumFuel), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false), 9999f, (Thing pack) => !pack.IsForbidden(pawn) && pawn.CanReserve(pack, 1, -1, null, false) && pack.GetRoom(RegionType.Set_All) != prisoner.GetRoom(RegionType.Set_All), null, 0, -1, false, RegionType.Set_Passable, false);
			if (thing == null)
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.DeliverFood, thing, prisoner);
			job.count = 1;
			job.targetC = RCellFinder.SpotToChewStandingNear(prisoner, thing, null);
			return job;
		}

		// Token: 0x0600414B RID: 16715 RVA: 0x001755D4 File Offset: 0x001737D4
		private bool AurumPackAlreadyAvailableFor(Pawn prisoner)
		{
			if (prisoner.carryTracker.CarriedCount(BTEMY_ThingDefOf.BTEMy_AurumFuel) > 0)
			{
				return true;
			}
			if (prisoner.inventory.Count(BTEMY_ThingDefOf.BTEMy_AurumFuel) > 0)
			{
				return true;
			}
			Room room = prisoner.GetRoom(RegionType.Set_All);
			if (room != null)
			{
				List<Region> regions = room.Regions;
				for (int i = 0; i < regions.Count; i++)
				{
					if (regions[i].ListerThings.ThingsOfDef(BTEMY_ThingDefOf.BTEMy_AurumFuel).Count > 0)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
