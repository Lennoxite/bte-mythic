using System;
using Verse;
using Verse.AI;
using RimWorld;
namespace BTE_MY
{
	public class CompAbilityEffect_ReverenceCost : CompAbilityEffect
	{
		public new CompProperties_AbilityReverenceCost Props
		{
			get
			{
				return (CompProperties_AbilityReverenceCost)this.props;
			}
		}

		private bool HasEnoughReverence
		{
			get
			{
				Pawn_GeneTracker genes = this.parent.pawn.genes;
				Gene_Reverence gene_Reverence = (genes != null) ? genes.GetFirstGeneOfType<Gene_Reverence>() : null;
				return gene_Reverence != null && gene_Reverence.Value >= this.Props.reverenceCost;
			}
		}

		public override bool CanCast
		{
			get
			{
				return this.HasEnoughReverence && base.CanCast;
			}
		}

		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			ReverenceUtility.OffsetReverence(this.parent.pawn, -this.Props.reverenceCost);
		}

		public override bool GizmoDisabled(out string reason)
		{
			Pawn_GeneTracker genes = this.parent.pawn.genes;
			Gene_Reverence gene_Reverence = (genes != null) ? genes.GetFirstGeneOfType<Gene_Reverence>() : null;
			if (gene_Reverence == null)
			{
				reason = "Ability Disabled: No Reverence Gene";
				return true;
			}
			if (gene_Reverence.Value < this.Props.reverenceCost)
			{
				reason = "Ability Disabled: No Reverence";
				return true;
			}
			float num = this.TotalReverenceCostOfQueuedAbilities();
			float num2 = this.Props.reverenceCost + num;
			if (this.Props.reverenceCost > 1E-45f && num2 > gene_Reverence.Value)
			{
				reason = "Ability Disabled: No Reverence";
				return true;
			}
			reason = null;
			return false;
		}

		public override bool AICanTargetNow(LocalTargetInfo target)
		{
			return this.HasEnoughReverence;
		}

		private float TotalReverenceCostOfQueuedAbilities()
		{
			Pawn_JobTracker jobs = this.parent.pawn.jobs;
			object obj;
			if (jobs == null)
			{
				obj = null;
			}
			else
			{
				Job curJob = jobs.curJob;
				obj = ((curJob != null) ? curJob.verbToUse : null);
			}
			Verb_CastAbility verb_CastAbility = obj as Verb_CastAbility;
			float num;
			if (verb_CastAbility == null)
			{
				num = 0f;
			}
			else
			{
				Ability ability = verb_CastAbility.ability;
				num = ((ability != null) ? ReverenceUtility.ReverenceCost(ability) : 0f);
			}
			float num2 = num;
			if (this.parent.pawn.jobs != null)
			{
				for (int i = 0; i < this.parent.pawn.jobs.jobQueue.Count; i++)
				{
					Verb_CastAbility verb_CastAbility2;
					if ((verb_CastAbility2 = (this.parent.pawn.jobs.jobQueue[i].job.verbToUse as Verb_CastAbility)) != null)
					{
						float num3 = num2;
						Ability ability2 = verb_CastAbility2.ability;
						num2 = num3 + ((ability2 != null) ? ReverenceUtility.ReverenceCost(ability2) : 0f);
					}
				}
			}
			return num2;
		}
	}
}
