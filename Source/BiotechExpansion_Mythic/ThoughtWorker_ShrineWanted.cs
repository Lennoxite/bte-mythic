using System;
using Verse;
using RimWorld;

namespace BTE_MY
{
	public class ThoughtWorker_ShrineWanted: ThoughtWorker
	{
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			Gene_Reverence revDem = p.genes.GetFirstGeneOfType<Gene_Reverence>();
			if (revDem == null)
			{
				//Log.Message("Revered Gene missing");
				return ThoughtState.Inactive;
			}
			if (revDem.shrine != null)
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