using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;
namespace BTE_MY
{
	public class CompProperties_AbilityPurgeMemory : CompProperties_AbilityEffect
	{
		public CompProperties_AbilityPurgeMemory()
		{
			this.compClass = typeof(CompAbilityEffect_PurgeMemory);
		}

		public float minMoodOffset = -10f;

		[MustTranslate]
		public string successMessage;

		[MustTranslate]
		public string successMessageNoNegativeThought;

		[MustTranslate]
		public string failMessage;

	}
}
