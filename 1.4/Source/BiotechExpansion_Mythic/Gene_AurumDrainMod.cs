using System;
using System.Collections.Generic;
using Verse;
using RimWorld;


namespace BTE_MY
{
	public class Gene_AurumDrainMod : Gene, IGeneResourceDrain
	{
		// Token: 0x17001AC1 RID: 6849
		// (get) Token: 0x06009B91 RID: 39825 RVA: 0x003825CF File Offset: 0x003807CF
		public Gene_Resource Resource
		{
			get
			{
				if (this.cachedAurumGene == null)
				{
					this.cachedAurumGene = this.pawn.genes.GetFirstGeneOfType<Gene_Aurum>();
				}
				return this.cachedAurumGene;
			}
		}

		// Token: 0x17001AC2 RID: 6850
		// (get) Token: 0x06009B92 RID: 39826 RVA: 0x00382363 File Offset: 0x00380563
		public bool CanOffset
		{
			get
			{
				return !this.pawn.Deathresting;
			}
		}

		// Token: 0x17001AC3 RID: 6851
		// (get) Token: 0x06009B93 RID: 39827 RVA: 0x003823A3 File Offset: 0x003805A3
		public float ResourceLossPerDay
		{
			get
			{
				return this.def.resourceLossPerDay;
			}
		}

		// Token: 0x17001AC4 RID: 6852
		// (get) Token: 0x06009B94 RID: 39828 RVA: 0x0038235B File Offset: 0x0038055B
		public Pawn Pawn
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x17001AC5 RID: 6853
		// (get) Token: 0x06009B95 RID: 39829 RVA: 0x00382373 File Offset: 0x00380573
		public string DisplayLabel
		{
			get
			{
				return this.Label + " (" + "Gene".Translate() + ")";
			}
		}

		// Token: 0x06009B96 RID: 39830 RVA: 0x0038247C File Offset: 0x0038067C
		public override void Tick()
		{
			base.Tick();
			AurumUtility.TickResourceDrain(this);
		}

		// Token: 0x06009B97 RID: 39831 RVA: 0x003825F5 File Offset: 0x003807F5
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in AurumUtility.GetResourceDrainGizmos(this))
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x040061DC RID: 25052
		[Unsaved(false)]
		private Gene_Aurum cachedAurumGene;

		// Token: 0x040061DD RID: 25053
		private const float MinAgeForDrain = 3f;
	}
}
