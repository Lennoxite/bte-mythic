using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;
namespace BTE_MY
{
    class CompProperties_UseEffectOffsetAurumLevel : CompProperties_UseEffect
    {


		// Token: 0x0600852C RID: 34092 RVA: 0x002D4C11 File Offset: 0x002D2E11
		public CompProperties_UseEffectOffsetAurumLevel()
		{
			this.compClass = typeof(CompUseEffect_OffsetAurumLevel);
		}

		// Token: 0x04004A4D RID: 19021
		public int offset;



	}

}
