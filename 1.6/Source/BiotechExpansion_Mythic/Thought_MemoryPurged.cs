using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;
namespace BTE_MY
{
	public class Thought_MemoryPurged : Thought_Memory
	{
		public override int DurationTicks
		{
			get
			{
				return this.durationTicksOverride;
			}
		}

		public override float MoodOffset()
		{
			return this.moodOffsetOverride;
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.moodOffsetOverride, "moodOffsetOverride", 0f, false);
		}

		public float moodOffsetOverride;
	}
}
