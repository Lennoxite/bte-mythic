using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using RimWorld;
namespace BTE_MY
{
	public class CompProperties_AbilityAurumCost : CompProperties_AbilityEffect
	{
		public CompProperties_AbilityAurumCost()
		{
			this.compClass = typeof(CompAbilityEffect_AurumCost);
		}

		public override IEnumerable<string> ExtraStatSummary()
		{
			yield return "Aurum cost"+ ": " + Mathf.RoundToInt(this.aurumCost * 100f);
			yield break;
		}

		public float aurumCost;
	}
}
