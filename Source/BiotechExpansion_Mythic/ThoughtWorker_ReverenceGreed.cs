using System;
using Verse;
using RimWorld;

namespace BTE_MY
{
	public class ThoughtWorker_ReverenceGreed : ThoughtWorker
	{
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			Gene_Reverence revDem = p.genes.GetFirstGeneOfType<Gene_Reverence>();
			if (revDem == null)
			{
				//Log.Message("Revered Gene missing");
				return ThoughtState.Inactive;
			}
			if (revDem.Value > 0.75 * revDem.Max)
			{
				//Log.Message("Revered too high");
				return ThoughtState.Inactive;

			}
			else
			{
				//Log.Message("Revered Low");
				return ThoughtState.ActiveAtStage(0);
			}
			return ThoughtState.Inactive;
		}
	}
}