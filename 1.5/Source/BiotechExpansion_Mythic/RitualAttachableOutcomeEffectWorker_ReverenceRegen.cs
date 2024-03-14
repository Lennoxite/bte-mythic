using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;
namespace BTE_MY
{
    class RitualAttachableOutcomeEffectWorker_ReverenceRegen : RitualAttachableOutcomeEffectWorker
	{
		public override void Apply(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, RitualOutcomePossibility outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
		{
			extraOutcomeDesc = this.def.letterInfoText;
			foreach (Pawn pawn in totalPresence.Keys)
			{
				Gene_Reverence rev = pawn.genes.GetFirstGeneOfType<Gene_Reverence>();
				if (rev != null)
				{
					rev.Value = rev.Max;
				}
			}
		}
	}
}
