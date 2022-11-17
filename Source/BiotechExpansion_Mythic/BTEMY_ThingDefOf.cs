using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;
namespace BTE_MY
{
	[DefOf]
	public static class BTEMY_ThingDefOf
	{
		static BTEMY_ThingDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BTEMY_ThingDefOf));
		}

		public static ThingDef BTEMy_AurumFuel;



	}
}


