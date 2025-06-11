using System;
using Verse;

namespace BTE_MY
{
	public class HediffCompProperties_SeverityFromAurum : HediffCompProperties
	{
		public HediffCompProperties_SeverityFromAurum()
		{
			this.compClass = typeof(HediffComp_SeverityFromAurum);
		}

		public float severityPerHourEmpty;

		public float severityPerHourAurum;
	}
}