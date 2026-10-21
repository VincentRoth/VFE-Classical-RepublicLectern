using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using VFEC.Senators;

namespace VFEC_RepublicLectern
{
    // Every access to VFE Classical types is here, so the 1.7 adaptation (Unity type mismatch,
    // reflection through Traverse / AccessTools) stays in one file.
    internal static class VfecBridge
    {
        public static IEnumerable<Faction> SenatorFactions() =>
            Find.FactionManager == null
                ? Enumerable.Empty<Faction>()
                : Find.FactionManager.AllFactionsVisible.Where(f => f.ShouldHaveSenators());

        public static bool IsRallied(Faction faction) =>
            WorldComponent_Senators.Instance != null
            && WorldComponent_Senators.Instance.Permanent.TryGetValue(faction, out bool permanent)
            && permanent;

        public static void FavoredCount(Faction faction, out int favored, out int total)
        {
            favored = 0;
            total = 0;
            var instance = WorldComponent_Senators.Instance;
            if (instance == null) return;
            instance.CheckInit();
            if (!instance.SenatorInfo.TryGetValue(faction, out var infos)) return;
            total = infos.Count;
            favored = infos.Count(i => i.Favored);
        }

        // Read-only, like the "View Senators" button of the Factions tab.
        public static void OpenSenators(Faction faction)
        {
            var instance = WorldComponent_Senators.Instance;
            if (instance == null) return;
            instance.CheckInit();
            Find.WindowStack.Add(new Dialog_SenatorInfo(
                faction.def.GetModExtension<FactionExtension_SenatorInfo>(),
                instance.SenatorInfo[faction],
                false)
            {
                Faction = faction
            });
        }
    }
}
