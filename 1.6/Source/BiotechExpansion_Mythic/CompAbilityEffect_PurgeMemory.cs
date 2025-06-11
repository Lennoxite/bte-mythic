using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;
using Verse.Sound;

namespace BTE_MY
{
	public class CompAbilityEffect_PurgeMemory : CompAbilityEffect
	{
		public new CompProperties_AbilityPurgeMemory Props
		{
			get
			{
				return (CompProperties_AbilityPurgeMemory)this.props;
			}
		}

		public override bool HideTargetPawnTooltip
		{
			get
			{
				return true;
			}
		}

		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			Pawn pawn = target.Pawn;

			List<Thought> list = new List<Thought>();
			pawn.needs.mood.thoughts.GetAllMoodThoughts(list);
			Thought_Memory thought_Memory = (Thought_Memory)(from t in list
			where t is Thought_Memory && t.MoodOffset() <= this.Props.minMoodOffset
			select t).MaxByWithFallback((Thought t) => -t.MoodOffset(), null);
			if (thought_Memory != null)
			{
				Thought_MemoryPurged thought_Counselled = (Thought_MemoryPurged)ThoughtMaker.MakeThought(BTEMY_ThoughtDefOf.BTEMy_Thought_MemoryPurge);
				thought_Counselled.durationTicksOverride = thought_Memory.DurationTicks - thought_Memory.age;
				thought_Counselled.moodOffsetOverride = -thought_Memory.MoodOffset();
				pawn.needs.mood.thoughts.memories.TryGainMemory(thought_Counselled, this.parent.pawn);
				Messages.Message(this.Props.successMessage.Formatted(this.parent.pawn.Named("INITIATOR"), pawn.Named("RECIPIENT"), thought_Memory.MoodOffset()), new LookTargets(new Pawn[]
				{
					this.parent.pawn,
					pawn
				}), MessageTypeDefOf.PositiveEvent, false);
			}
			if (this.Props.sound != null)
			{
				this.Props.sound.PlayOneShot(new TargetInfo(target.Cell, this.parent.pawn.Map, false));
			}

		}

		// Token: 0x06005C46 RID: 23622 RVA: 0x001F5384 File Offset: 0x001F3584
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			Pawn pawn = target.Pawn;
			if (pawn == null)
			{
				return false;
			}
			if (!AbilityUtility.ValidateMustBeHuman(pawn, throwMessages, this.parent))
			{
				return false;
			}
			if (!AbilityUtility.ValidateNoMentalState(pawn, throwMessages, this.parent))
			{
				return false;
			}
			if (!AbilityUtility.ValidateIsAwake(pawn, throwMessages, this.parent))
			{
				return false;
			}
			List<Thought> list = new List<Thought>();
			pawn.needs.mood.thoughts.GetAllMoodThoughts(list);
			if (list.Any((Thought t) => t.def == BTEMY_ThoughtDefOf.BTEMy_Thought_MemoryPurge))
			{
				if (throwMessages)
				{
					Messages.Message("Target already has a purged memory.", pawn, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}

	}
}
