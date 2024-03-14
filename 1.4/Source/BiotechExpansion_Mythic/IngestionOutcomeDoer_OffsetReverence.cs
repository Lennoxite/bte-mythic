using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using RimWorld;

namespace BTE_MY
{
	public class IngestionOutcomeDoer_OffsetReverence : IngestionOutcomeDoer
	{
		protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested)
		{
			ReverenceUtility.OffsetReverenceWithModifier(pawn, this.offset * (float)ingested.stackCount);
		}

		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef)
		{
				string arg = (this.offset >= 0f) ? "+" : string.Empty;
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsNonPawnImportant, "Reverence", arg + Mathf.RoundToInt(this.offset * 100f), "Amount of Reverence this restores.", 1000, null, null, false);
			yield break;
		}

		public float offset;
	}
}
