using System.Linq;
using UnityEngine;
using Verse;

namespace VFEC_RepublicLectern
{
    public class Panel_Senators : LecternPanelWorker
    {
        private const float RowHeight = 60f;

        public override void DoPanel(Rect inRect)
        {
            var factions = VfecBridge.SenatorFactions().ToList();
            if (factions.Count == 0)
            {
                Widgets.Label(inRect, "VFECRL.Senators.None".Translate());
                return;
            }

            float y = inRect.y;
            foreach (var faction in factions)
            {
                var row = new Rect(inRect.x, y, inRect.width, RowHeight);
                Widgets.DrawHighlightIfMouseover(row);

                var icon = new Rect(row.x + 5f, row.y + 5f, 50f, 50f);
                if (faction.def.FactionIcon != null)
                {
                    GUI.color = faction.Color;
                    GUI.DrawTexture(icon, faction.def.FactionIcon);
                    GUI.color = Color.white;
                }

                Text.Anchor = TextAnchor.MiddleLeft;
                Widgets.Label(new Rect(row.x + 65f, row.y, 300f, RowHeight), faction.Name);

                VfecBridge.FavoredCount(faction, out int favored, out int total);
                string status = "VFECRL.Senators.Favored".Translate(favored, total);
                if (VfecBridge.IsRallied(faction)) status = "VFECRL.Senators.Rallied".Translate() + " - " + status;
                Widgets.Label(new Rect(row.x + 370f, row.y, 300f, RowHeight), status);
                Text.Anchor = TextAnchor.UpperLeft;

                if (Widgets.ButtonText(new Rect(row.xMax - 160f, row.y + 15f, 150f, 30f), "VFECRL.Senators.View".Translate()))
                    VfecBridge.OpenSenators(faction);

                y += RowHeight + 5f;
            }
        }
    }
}
