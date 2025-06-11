using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.Sound;
using RimWorld;

namespace BTE_MY
{
	public class CompCreateReveredMote : ThingComp
	{
		public CompProperties_CreateReveredMote Props
		{
			get
			{
				return (CompProperties_CreateReveredMote)this.props;
			}
		}


		public List<Thing> MeditationSpots {
			get
            {
				return meditationSpots;

            }
		}

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);

			if (!respawningAfterLoad)
				GetMeditationSpots();
        }

        public void AddProgress(float progress, bool ignoreMultiplier = false)
		{
			this.progressToNextSubplant += progress;
			this.TrySpawnMote();
		}

        public override void CompTickRare()
        {
            base.CompTickRare();


			//Log.Message("Tick Rare!");

			foreach (Thing c in MeditationSpots)
            {
				//Log.Message("Spot Seen");
				if (c == null)
					continue;
				if (c.Map == null)
					continue;
				//if (c.Position == null)
					//continue;

				//Log.Message("Spot Found");
				Pawn p = GridsUtility.GetFirstPawn(c.Position, c.Map);
				if (p == null)
					continue;
				if (p.jobs == null)
					continue;
				if (p.jobs.curJob == null)
					continue;
				//Log.Message("Pawn Test");
				if ((p.jobs.curJob.def == JobDefOf.Meditate || p.jobs.curJob.def == JobDefOf.MeditatePray))
				{
					//Log.Message("Pawn Found");
					this.AddProgress((1f / 80f));

                }
            }
        }

        public void Cleanup()
		{
		}

		public override string CompInspectStringExtra()
		{
			return "Mote Formation: " + (this.progressToNextSubplant).ToString();
		}

		private void TrySpawnMote()
		{
			while (this.progressToNextSubplant >= 1f)
			{
				this.DoSpawnMote();
				this.progressToNextSubplant -= 1f;
			}
		}

		private void DoSpawnMote()
		{
			IntVec3 position = this.parent.Position;
			GenSpawn.Spawn(this.Props.spawnedThing, position, this.parent.Map, WipeMode.VanishOrMoveAside);
			Action action = this.onGrassGrown;
			if (action == null)
			{
				return;
			}
			action();
			return;

		}

		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			yield return new Command_Action
			{
				defaultLabel = "Scan for meditation spots",
				defaultDesc = "Get nearby meditation spots. Required to use totem.",
				activateSound = SoundDefOf.Click,
				icon = BTEMY_ThingDefOf.BTEMy_ReverenceFuel.uiIcon,
				action = delegate ()
				{
					this.GetMeditationSpots();
				}
			};
			if (!Prefs.DevMode || !DebugSettings.godMode)
			{
				yield break;
			}
			yield return new Command_Action
			{
				defaultLabel = "DEV: Add 100% progress",
				action = delegate ()
				{
					this.AddProgress(1f, true);
				}
			};
			yield break;
		}

		public override void PostExposeData()
		{
			Scribe_Values.Look<float>(ref this.progressToNextSubplant, "progressToNextSubplant", 0f, false);
			Scribe_Collections.Look<Thing>(ref this.meditationSpots, "medispots", LookMode.Reference);
		}

		private void GetMeditationSpots() {
			meditationSpots = this.parent.Map.listerBuldingOfDefInProximity.GetForCell(this.parent.Position, 4.9f, ThingDefOf.MeditationSpot);

		}

		private float progressToNextSubplant;

		private List<Thing> meditationSpots;

		public Action onGrassGrown;

	}
}
