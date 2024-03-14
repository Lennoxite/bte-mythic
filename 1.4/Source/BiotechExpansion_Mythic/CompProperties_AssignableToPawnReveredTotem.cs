using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;
namespace BTE_MY
{
    class CompProperties_AssignableToPawnReveredTotem : CompProperties_AssignableToPawn
    {
        public CompProperties_AssignableToPawnReveredTotem()
        {
            this.compClass = typeof(CompAssignableToPawn_ReveredTotem);
        }

        public List<AbilityDef> abilities;

        public float maxRevOffset = 0f;

    }
}
