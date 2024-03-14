using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;
namespace BTE_MY
{
    public class AbilityLaserInfo : DefModExtension
    {
        public DamageDef damageType;

        public int damage = 12;

        public int tickDuration = 300; //5 seconds

        public int ticksPerHit = 60;

        public float laserWidth = 1f;

        public ThingDef beamMoteDef;

        public EffecterDef beamEndEffDef;
    }
}
