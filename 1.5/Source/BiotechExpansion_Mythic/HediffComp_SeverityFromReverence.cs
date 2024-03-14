using System;
using RimWorld;
using Verse;

namespace BTE_MY
{
	public class HediffComp_SeverityFromReverence : HediffComp
	{
		public HediffCompProperties_SeverityFromReverence Props
		{
			get
			{
				return (HediffCompProperties_SeverityFromReverence)this.props;
			}
		}

		public override bool CompShouldRemove
		{
			get
			{
				Pawn_GeneTracker genes = base.Pawn.genes;
				return ((genes != null) ? genes.GetFirstGeneOfType<Gene_Reverence>() : null) == null;
			}
		}

		private Gene_Reverence Reverence
		{
			get
			{
				if (this.cachedReverenceGene == null)
				{
					this.cachedReverenceGene = base.Pawn.genes.GetFirstGeneOfType<Gene_Reverence>();
				}
				return this.cachedReverenceGene;
			}
		}

		public override void CompPostTick(ref float severityAdjustment)
		{
			base.CompPostTick(ref severityAdjustment);
			severityAdjustment += ((this.Reverence.Value > 0f) ? this.Props.severityPerHourReverence : this.Props.severityPerHourEmpty) / 2500f;
		}

		private Gene_Reverence cachedReverenceGene;
	}
}
