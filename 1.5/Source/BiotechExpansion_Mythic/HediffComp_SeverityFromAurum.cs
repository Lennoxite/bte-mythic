using System;
using RimWorld;
using Verse;

namespace BTE_MY
{
	public class HediffComp_SeverityFromAurum : HediffComp
	{
		public HediffCompProperties_SeverityFromAurum Props
		{
			get
			{
				return (HediffCompProperties_SeverityFromAurum)this.props;
			}
		}

		public override bool CompShouldRemove
		{
			get
			{
				Pawn_GeneTracker genes = base.Pawn.genes;
				return ((genes != null) ? genes.GetFirstGeneOfType<Gene_Aurum>() : null) == null;
			}
		}

		private Gene_Aurum Aurum
		{
			get
			{
				if (this.cachedAurumGene == null)
				{
					this.cachedAurumGene = base.Pawn.genes.GetFirstGeneOfType<Gene_Aurum>();
				}
				return this.cachedAurumGene;
			}
		}

		public override void CompPostTick(ref float severityAdjustment)
		{
			base.CompPostTick(ref severityAdjustment);
			severityAdjustment += ((this.Aurum.Value > 0f) ? this.Props.severityPerHourAurum : this.Props.severityPerHourEmpty) / 2500f;
		}

		private Gene_Aurum cachedAurumGene;
	}
}
