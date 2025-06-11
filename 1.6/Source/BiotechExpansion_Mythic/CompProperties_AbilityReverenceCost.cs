using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using RimWorld;
namespace BTE_MY
{
	public class CompProperties_AbilityReverenceCost : CompProperties_AbilityEffect
	{
		public CompProperties_AbilityReverenceCost()
		{
			this.compClass = typeof(CompAbilityEffect_ReverenceCost);
		}

		public override IEnumerable<string> ExtraStatSummary()
		{
			yield return "Reverence cost"+ ": " + Mathf.RoundToInt(this.reverenceCost * 100f);
			yield break;
		}

		public float reverenceCost;
	}
}
