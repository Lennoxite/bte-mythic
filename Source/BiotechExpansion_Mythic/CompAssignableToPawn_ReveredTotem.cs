using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;
namespace BTE_MY
{
    class CompAssignableToPawn_ReveredTotem : CompAssignableToPawn
    {

        new public CompProperties_AssignableToPawnReveredTotem Props
        {
            get
            {
                return (CompProperties_AssignableToPawnReveredTotem)this.props;
            }
        }

        public override void TryAssignPawn(Pawn pawn)
        {
            Gene_Reverence rev = pawn.genes.GetFirstGeneOfType<Gene_Reverence>();
            if (rev != null)
            {
                if (rev.shrine != null)
                {
                    CompAssignableToPawn_ReveredTotem oldShrine = rev.shrine.TryGetComp<CompAssignableToPawn_ReveredTotem>();
                    if (oldShrine != null)
                        oldShrine.TryUnassignPawn(pawn);
                }

                rev.shrine = this.parent;

                if (this.assignedPawns.Any<Pawn>())
                {
                    this.TryUnassignPawn(this.assignedPawns[0]);
                }

                //Offset max reverence.
                rev.SetMax(1+Props.maxRevOffset);
                //Give any abilities.
                if (pawn.abilities != null && Props.abilities != null)
                    foreach (AbilityDef ab in Props.abilities)
                    {
                        pawn.abilities.GainAbility(ab);
                    }
                base.TryAssignPawn(pawn);
            }


        }
        public override AcceptanceReport CanAssignTo(Pawn pawn)
        {
            if (!pawn.genes.HasGene(BTEMY_GeneDefOf.BTEMy_ReveredEmbodiment))
                return "Not revered";
            return AcceptanceReport.WasAccepted;
        }
        public override void TryUnassignPawn(Pawn pawn, bool sort = true, bool uninstall = false)
        {
            Gene_Reverence rev = pawn.genes.GetFirstGeneOfType<Gene_Reverence>();
            if (rev != null)
            {
                if (rev.shrine == this.parent)
                {
                    rev.shrine = null;
                }


                //Reset max reverence.
                rev.SetMax(1);
                //Remove any given abilities.
                if (pawn.abilities != null && Props.abilities != null)
                    foreach (AbilityDef ab in Props.abilities)
                    {
                        pawn.abilities.RemoveAbility(ab);
                    }

            }
            

            
            base.TryUnassignPawn(pawn, sort, uninstall);


        }
    }
}
