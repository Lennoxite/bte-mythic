using System;
using System.Collections.Generic;
using Verse;
using RimWorld;

namespace BTE_MY
{
	// Token: 0x02001482 RID: 5250
	public class CompProperties_CreateReveredMote : CompProperties
	{
		public ThingDef spawnedThing;


		public CompProperties_CreateReveredMote()
		{
			this.compClass = typeof(CompCreateReveredMote);
		}
	}
}
