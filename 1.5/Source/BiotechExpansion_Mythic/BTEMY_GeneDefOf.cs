using RimWorld;
using Verse;


namespace BTE_MY
{
    [DefOf]
    public static class BTEMY_GeneDefOf
    {
        static BTEMY_GeneDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(BTEMY_GeneDefOf));
        }

        public static GeneDef BTEMy_AurumThirst;
        public static GeneDef BTEMy_ReveredEmbodiment;

    }
}
