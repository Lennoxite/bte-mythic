using RimWorld;
using Verse;

namespace BTE_MY
{
	[DefOf]
	public static class BTEMY_ThoughtDefOf
	{
		static BTEMY_ThoughtDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BTEMY_ThoughtDefOf));
		}

		public static ThoughtDef BTEMy_Thought_MemoryPurge;
	}

}