using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace VFEC_RepublicLectern
{
    public class Dialog_Lectern : Window
    {
        private LecternPanelDef selected;

        public override Vector2 InitialSize => new Vector2(900f, 620f);

        public Dialog_Lectern()
        {
            doCloseX = true;
            doCloseButton = false;
            forcePause = false;
            absorbInputAroundWindow = false;
            closeOnClickedOutside = false;
            draggable = true;
            resizeable = false;
        }

        private static List<LecternPanelDef> VisiblePanels() =>
            DefDatabase<LecternPanelDef>.AllDefsListForReading
                .Where(d => d.Worker.Visible)
                .OrderBy(d => d.order)
                .ThenBy(d => d.label)
                .ToList();

        public override void DoWindowContents(Rect inRect)
        {
            Text.Font = GameFont.Medium;
            Widgets.Label(new Rect(inRect.x, inRect.y, inRect.width - 40f, 36f), "VFECRL.Lectern.Title".Translate());
            Text.Font = GameFont.Small;

            var panels = VisiblePanels();
            if (panels.Count == 0)
            {
                Widgets.Label(new Rect(inRect.x, inRect.y + 50f, inRect.width, 30f), "VFECRL.Lectern.NoPanels".Translate());
                return;
            }
            if (selected == null || !panels.Contains(selected)) selected = panels[0];

            var content = new Rect(inRect.x, inRect.y + 80f, inRect.width, inRect.height - 80f);
            var tabs = panels.Select(p => new TabRecord(p.LabelCap, () => selected = p, selected == p)).ToList();
            TabDrawer.DrawTabs(content, tabs);

            Widgets.DrawMenuSection(content);
            selected.Worker.DoPanel(content.ContractedBy(10f));
        }

        public override void PostClose()
        {
            base.PostClose();
            foreach (var def in DefDatabase<LecternPanelDef>.AllDefsListForReading) def.Worker.Notify_Closed();
        }
    }
}
