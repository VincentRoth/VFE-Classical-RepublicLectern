using HarmonyLib;
using Verse;

namespace VFEC_RepublicLectern
{
    // Skeleton: patches and the panel system are added in later steps (see DESIGN.md).
    [StaticConstructorOnStartup]
    public static class RepublicLecternMod
    {
        public const string HarmonyId = "dibolic.vfec.republiclectern";

        private static readonly Harmony Harm = new Harmony(HarmonyId);

        static RepublicLecternMod()
        {
            Log.Message("[VFEC RepublicLectern] Loaded.");
        }
    }
}
