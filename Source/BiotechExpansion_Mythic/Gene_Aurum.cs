using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using RimWorld;
using Verse;
using BTE_MMLA;

namespace BTE_MY
{

	public class Gene_Aurum : Gene_Resource, IGeneResourceDrain
	{
		public Gene_Resource Resource
		{
			get
			{
				return this;
			}
		}

		public Pawn Pawn
		{
			get
			{
				return this.pawn;
			}
		}

		public bool CanOffset
		{
			get
			{
				return !this.pawn.Deathresting;
			}
		}

		public string DisplayLabel
		{
			get
			{
				return this.Label + " (" + "Gene" + ")";
			}
		}

		public float ResourceLossPerDay
		{
			get
			{
				return this.def.resourceLossPerDay;
			}
		}

		public override float InitialResourceMax
		{
			get
			{
				return 1f;
			}
		}

		public override float MinLevelForAlert
		{
			get
			{
				return 0.15f;
			}
		}

		public override float MaxLevelOffset
		{
			get
			{
				return 0.1f;
			}
		}

		protected override Color BarColor
		{
			get
			{
				return new ColorInt(2000, 200, 10).ToColor;
			}
		}

		protected override Color BarHighlightColor
		{
			get
			{
				return new ColorInt(255, 255, 122).ToColor;
			}
		}

		public override void PostAdd()
		{
			base.PostAdd();
			this.Reset();
		}

		/*public override void Notify_IngestedThing(Thing thing, int numTaken)
		{
			if (thing.def.IsMeat && thing.def.ingestible.sourceDef.race.Humanlike)
			{
				GeneUtility.OffsetHemogen(this.pawn, 0.0375f * thing.GetStatValue(StatDefOf.Nutrition, true, -1) * (float)numTaken, true);
			}
		}*/

		public override void Tick()
		{
			base.Tick();
			AurumUtility.TickResourceDrain(this);
		}

		public override void SetTargetValuePct(float val)
		{
			this.targetValue = Mathf.Clamp(val * this.Max, 0f, this.Max - this.MaxLevelOffset);
		}

		public bool ShouldConsumeAurumNow()
		{
			return this.Value < this.targetValue;
		}

		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			foreach (Gizmo gizmo2 in AurumUtility.GetResourceDrainGizmos(this))
			{
				yield return gizmo2;
			}
			enumerator = null;
			yield break;
			yield break;
		}
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.aurumFuelAllowed, "aurumFuelAllowed", true, false);
		}

		public bool aurumFuelAllowed = true;
	}

}
