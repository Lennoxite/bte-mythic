using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace BTE_MY
{
    class CompUseEffect_OffsetAurumLevel : CompUseEffect
    {

		private CompProperties_UseEffectOffsetAurumLevel Props
		{
			get
			{
				return (CompProperties_UseEffectOffsetAurumLevel)this.props;
			}
		}

		public override void DoEffect(Pawn user)
		{
			if (!ModsConfig.BiotechActive)
			{
				return;
			}
			base.DoEffect(user);
			Pawn_GeneTracker genes = user.genes;
			if (genes == null)
			{
				return;
			}
			Gene_Aurum firstGeneOfType = genes.GetFirstGeneOfType<Gene_Aurum>();
			if (firstGeneOfType == null)
			{
				return;
			}
			firstGeneOfType.OffsetAurumLevel(this.Props.offset);
		}

		public override bool CanBeUsedBy(Pawn p, out string failReason)
		{
			Pawn_GeneTracker genes = p.genes;
			if (((genes != null) ? genes.GetFirstGeneOfType<Gene_Aurum>() : null) == null)
			{
				failReason = "Pawn does not have an active aurum system.";
				return false;
			}
			return base.CanBeUsedBy(p, out failReason);
		}

		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			if (ModsConfig.BiotechActive)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsNonPawnImportant, "Aurum Level", this.Props.offset.ToStringWithSign(), "How much this item increases the aurum level, and max aurum, of a pawn.", 1010, null, null, false);
			}
			yield break;
		}

	}
}
