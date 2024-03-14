using System;
using Verse;

namespace BTE_MY
{
	public class HediffCompProperties_SeverityFromReverence : HediffCompProperties
	{
		public HediffCompProperties_SeverityFromReverence()
		{
			this.compClass = typeof(HediffComp_SeverityFromReverence);
		}

		public float severityPerHourEmpty;

		public float severityPerHourReverence;
	}
}