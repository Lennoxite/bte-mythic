using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;
using Verse;
using Verse.Sound;
using UnityEngine;
namespace BTE_MY
{
	public class JobDriver_CastAbilityChannelStaticLaser : JobDriver_CastVerbOnce
	{
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			this.FailOnDowned(TargetIndex.A);
			this.FailOn(delegate ()
			{
				return !this.job.verbToUse.TryFindShootLineFromTo(this.GetActor().Position, new LocalTargetInfo(this.job.GetTarget(TargetIndex.A).Thing), out shtLne);
					
			});
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.initAction = delegate ()
			{
				this.pawn.pather.StopDead();
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return toil;
			Toil toil2 = Toils_Combat.CastVerb(TargetIndex.A, TargetIndex.B, false);
			if (this.job.ability != null && this.job.ability.def.showCastingProgressBar && this.job.verbToUse != null)
			{
				toil2.WithProgressBar(TargetIndex.A, () => this.job.verbToUse.WarmupProgress, false, -0.5f, false);
			}
			yield return toil2;
			Toil toil3 = ToilMaker.MakeToil("MakeNewToils");
			toil3.initAction = delegate ()
			{
				this.lasInfo = this.job.ability.def.GetModExtension<AbilityLaserInfo>();
				if (this.lasInfo != null)
                {
					if (this.lasInfo.beamMoteDef != null)
					{

						this.mote = MoteMaker.MakeInteractionOverlay(this.lasInfo.beamMoteDef, new TargetInfo(toil3.actor.Position, toil3.actor.Map),
							new TargetInfo(this.job.GetTarget(TargetIndex.A).Thing.Position, toil3.actor.Map));
						this.mote.Maintain();
					}
					if (this.lasInfo.beamEndEffDef != null)
						this.endEff = this.lasInfo.beamEndEffDef.Spawn(this.job.GetTarget(TargetIndex.A).Thing.Position, toil3.actor.Map, Vector3.zero, this.lasInfo.laserWidth);

					if (this.lasInfo.damageType != null)
						this.lasDamage = new DamageInfo(this.lasInfo.damageType, this.lasInfo.damage, 1, -1, toil3.actor);

				}
			};

			toil3.WithProgressBar(TargetIndex.A, delegate () { return this.ticksCast / this.lasInfo.tickDuration; });
			toil3.tickAction = delegate ()
			{
				if (this.lasInfo != null) {
					this.ticks++;
					this.ticksCast++;
					if (this.ticks >= this.lasInfo.ticksPerHit)
                    {
						this.ticks = 0;
						((Pawn)this.job.GetTarget(TargetIndex.A).Thing).TakeDamage(this.lasDamage);

					}
				}


				if (this.mote != null)
				{

					this.mote.UpdateTargets(new TargetInfo(toil3.actor.Position, toil3.actor.Map),
							new TargetInfo(this.job.GetTarget(TargetIndex.A).Thing.Position, toil3.actor.Map), Vector3.zero, Vector3.zero);
					this.mote.Maintain();
				}

				if (this.endEff != null)
                {
					this.endEff.EffectTick(new TargetInfo(this.job.GetTarget(TargetIndex.A).Thing.Position, toil3.actor.Map), TargetInfo.Invalid);
					this.endEff.ticksLeft--;
                }

				if (this.ticksCast > this.lasInfo.tickDuration)
                {
					toil3.actor.jobs.EndCurrentJob(JobCondition.Succeeded, true, true);
				}

			};

			toil3.AddFinishAction(delegate ()
			{

				if (this.endEff != null)
				{
					this.endEff.Cleanup();
					this.endEff = null;
				}

			});

			yield break;
		}

		public override void Notify_Starting()
		{
			base.Notify_Starting();
			Ability ability = this.job.ability;
			if (ability == null)
			{
				return;
			}
			ability.Notify_StartedCasting();
		}

		public override string GetReport()
		{
			string text;
			if (this.job.ability != null && !this.job.ability.def.targetRequired)
			{
				text = "UsingVerbNoTarget".Translate(this.job.verbToUse.ReportLabel);
			}
			else
			{
				text = base.GetReport();
			}
			if (this.job.ability != null && this.job.ability.def.showCastingProgressBar)
			{
				text += " " + "DurationLeft".Translate(this.job.verbToUse.WarmupTicksLeft.ToStringSecondsFromTicks()) + ".";
			}
			return text;
		}

		protected ShootLine shtLne;
		protected MoteDualAttached mote;
		protected Effecter endEff;
		protected DamageInfo lasDamage;
		protected AbilityLaserInfo lasInfo;
        protected int ticks = 0;
		protected int ticksCast = 0;
	}
}
