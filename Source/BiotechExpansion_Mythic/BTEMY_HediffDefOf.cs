using RimWorld;
using Verse;

namespace BTE_MY
{
	[DefOf]
	public static class BTEMY_HediffDefOf
	{
		static BTEMY_HediffDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BTEMY_HediffDefOf));
		}

		public static HediffDef BTEMy_AurumThirst;
	}

}