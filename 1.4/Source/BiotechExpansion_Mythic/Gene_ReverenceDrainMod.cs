using System;
using System.Collections.Generic;
using Verse;
using RimWorld;


namespace BTE_MY
{
	public class Gene_ReverenceDrainMod : Gene, IGeneResourceDrain
	{
		public Gene_Resource Resource
		{
			get
			{
				if (this.cachedReverenceGene == null)
				{
					this.cachedReverenceGene = this.pawn.genes.GetFirstGeneOfType<Gene_Reverence>();
				}
				return this.cachedReverenceGene;
			}
		}

		public bool CanOffset
		{
			get
			{
				return !this.pawn.Deathresting;
			}
		}

		public float ResourceLossPerDay
		{
			get
			{
				return this.def.resourceLossPerDay;
			}
		}

		public Pawn Pawn
		{
			get
			{
				return this.pawn;
			}
		}

		public string DisplayLabel
		{
			get
			{
				return this.Label + " (" + "Gene".Translate() + ")";
			}
		}

		public override void Tick()
		{
			base.Tick();
			ReverenceUtility.TickResourceDrain(this);
		}

		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in ReverenceUtility.GetResourceDrainGizmos(this))
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			yield break;
			yield break;
		}

		[Unsaved(false)]
		private Gene_Reverence cachedReverenceGene;

		private const float MinAgeForDrain = 3f;
	}
}
