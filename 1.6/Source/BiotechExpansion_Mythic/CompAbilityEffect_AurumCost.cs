using System;
using Verse;
using Verse.AI;
using RimWorld;
namespace BTE_MY
{
	public class CompAbilityEffect_AurumCost : CompAbilityEffect
	{
		public new CompProperties_AbilityAurumCost Props
		{
			get
			{
				return (CompProperties_AbilityAurumCost)this.props;
			}
		}

		private bool HasEnoughAurum
		{
			get
			{
				Pawn_GeneTracker genes = this.parent.pawn.genes;
				Gene_Aurum gene_Aurum = (genes != null) ? genes.GetFirstGeneOfType<Gene_Aurum>() : null;
				return gene_Aurum != null && gene_Aurum.Value >= this.Props.aurumCost;
			}
		}

		public override bool CanCast
		{
			get
			{
				return this.HasEnoughAurum && base.CanCast;
			}
		}

		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			AurumUtility.OffsetAurum(this.parent.pawn, -this.Props.aurumCost);
		}

		public override bool GizmoDisabled(out string reason)
		{
			Pawn_GeneTracker genes = this.parent.pawn.genes;
			Gene_Aurum gene_Aurum = (genes != null) ? genes.GetFirstGeneOfType<Gene_Aurum>() : null;
			if (gene_Aurum == null)
			{
				reason = "Ability Disabled: No Aurum Gene";
				return true;
			}
			if (gene_Aurum.Value < this.Props.aurumCost)
			{
				reason = "Ability Disabled: No Aurum";
				return true;
			}
			float num = this.TotalAurumCostOfQueuedAbilities();
			float num2 = this.Props.aurumCost + num;
			if (this.Props.aurumCost > 1E-45f && num2 > gene_Aurum.Value)
			{
				reason = "Ability Disabled: No Aurum";
				return true;
			}
			reason = null;
			return false;
		}

		public override bool AICanTargetNow(LocalTargetInfo target)
		{
			return this.HasEnoughAurum;
		}

		private float TotalAurumCostOfQueuedAbilities()
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
				num = ((ability != null) ? AurumUtility.AurumCost(ability) : 0f);
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
						num2 = num3 + ((ability2 != null) ? AurumUtility.AurumCost(ability2) : 0f);
					}
				}
			}
			return num2;
		}
	}
}
