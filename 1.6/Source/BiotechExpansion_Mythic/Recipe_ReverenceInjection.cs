using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using RimWorld;
namespace BTE_MY
{
	public class Recipe_ReverenceInjection : Recipe_Surgery
	{
		public override bool CompletableEver(Pawn surgeryTarget)
		{
			return base.CompletableEver(surgeryTarget);
		}

		public override bool AvailableOnNow(Thing thing, BodyPartRecord part = null)
		{
			//Pawn pawn;
			return thing.MapHeld != null && thing.MapHeld.listerThings.ThingsOfDef(BTEMY_ThingDefOf.BTEMy_ReverenceFuel).Count != 0 && base.AvailableOnNow(thing, part);
		}

		public override void ConsumeIngredient(Thing ingredient, RecipeDef recipe, Map map)
		{
		}

		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			if (!ModsConfig.BiotechActive)
			{
				return;
			}
			float num2 = 0f;
			for (int i = 0; i < ingredients.Count; i++)
			{
				if (!ingredients[i].def.IsMedicine)
				{
					num2 += JobGiver_GetReverence.ReverencePackReverenceGain * (float)ingredients[i].stackCount;
				}
			}
			if (num2 > 0f)
			{
				Pawn_GeneTracker genes = pawn.genes;
				if (((genes != null) ? genes.GetFirstGeneOfType<Gene_Reverence>() : null) != null)
				{
					ReverenceUtility.OffsetReverence(pawn, num2);
				}
			}
			for (int j = 0; j < ingredients.Count; j++)
			{
				ingredients[j].Destroy(DestroyMode.Vanish);
			}
		}

		public override float GetIngredientCount(IngredientCount ing, Bill bill)
		{
			return base.GetIngredientCount(ing, bill);
		}

	}
}