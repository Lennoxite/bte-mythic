using RimWorld;
using Verse;

namespace BTE_MY
{
	[DefOf]
	public static class BTEMY_StatDefOf
	{
		static BTEMY_StatDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BTEMY_StatDefOf));
		}

		public static StatDef BTEMy_AurumGainFactor;
	}

}