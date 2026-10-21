using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace VFEC_RepublicLectern
{
    public class Building_RepublicLectern : Building
    {
        public override IEnumerable<Gizmo> GetGizmos()
        {
            foreach (var gizmo in base.GetGizmos()) yield return gizmo;

            yield return new Command_Action
            {
                defaultLabel = "VFECRL.Gizmo.Open".Translate(),
                defaultDesc = "VFECRL.Gizmo.OpenDesc".Translate(),
                icon = TexButton.Info,
                action = () => Find.WindowStack.Add(new Dialog_Lectern())
            };

            // Shortcut to each Republic's senators, quicker than the Factions tab or a settlement's menu.
            foreach (var faction in VfecBridge.SenatorFactions())
            {
                var captured = faction;
                yield return new Command_Action
                {
                    defaultLabel = "VFECRL.Gizmo.Senators".Translate(captured.Name),
                    defaultDesc = "VFECRL.Gizmo.SenatorsDesc".Translate(captured.Name),
                    icon = captured.def.FactionIcon ?? TexButton.Info,
                    defaultIconColor = captured.Color,
                    action = () => VfecBridge.OpenSenators(captured)
                };
            }
        }
    }
}
