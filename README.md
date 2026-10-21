# VFE Classical – Republic Lectern

A management hub for the Republic factions of Vanilla Factions Expanded - Classical.

## What it does
- **Republic lectern** (Misc build menu): a 1x1 building costing 40 of a metallic, woody or stony material. With VFE Tribals it requires the *Culture* research (`VFET_Culture`); without it, no research is needed.
- **Window with tabs**: select the lectern and click *Open the lectern*. The built-in tab lists each Republic with its rallied state and how many senators are favorable.
- **Senator shortcuts**: the lectern also has one gizmo per Republic that opens its senators window, read-only, like the *View Senators* button of the Factions tab. Interactive actions (favors, bribes) remain available only to a caravan at the settlement.
- **Extension point**: other mods can add their own tab with a `LecternPanelDef` (see below). No patch needed.

## Requirements
- RimWorld 1.6 or 1.7
- Harmony
- Vanilla Factions Expanded - Classical

## Adding a tab from another mod
Reference `VFEC_RepublicLectern.dll`, write a worker, and declare the tab in XML:
```csharp
public class Panel_MyTab : VFEC_RepublicLectern.LecternPanelWorker
{
    public override bool Visible => true;              // hide the tab when it makes no sense
    public override void DoPanel(Rect inRect) { /* draw with Widgets */ }
    public override void Notify_Closed() { }           // optional
}
```
```xml
<VFEC_RepublicLectern.LecternPanelDef>
  <defName>MyMod_MyTab</defName>
  <label>My tab</label>
  <order>10</order>   <!-- the senators tab is 0 -->
  <workerClass>MyMod.Panel_MyTab</workerClass>
</VFEC_RepublicLectern.LecternPanelDef>
```
Add this mod to your `loadAfter` and `modDependencies`.

## Layout
```
About/ LoadFolders.xml
1.6/Assemblies/  1.7/Assemblies/        compiled DLLs (one per game version)
Defs/ThingDefs/                         the building
Defs/PanelDefs/                         the senators tab
Textures/Things/Building/RepublicLectern/   textures (_north, _east, _south) and masks (_northm, _eastm, _southm)
Languages/English|French/               Keyed strings; French also has DefInjected (building and tab)
Source/                                 C# (shared by 1.6 and 1.7)
```
All access to VFE Classical types is in `Source/VfecBridge.cs`.

Textures: `Graphic_Multi` + `CutoutComplex`. The building takes its material colour on the red area of the mask; the west view is the east one mirrored. Mask files must be named `<name>_northm.png`, not `<name>_north_m.png`.

## Build
Each game version has its own project, both compile the same sources into the mod's `Assemblies` folder:
```
dotnet build Source\VFEC_RepublicLectern.csproj        # 1.6 (net472)
dotnet build Source\1.7\VFEC_RepublicLectern.csproj    # 1.7 (net10.0)
```
- The 1.6 project needs the 1.6 game files (`RimWorldWin64_Data\Managed`). A 1.7 installation does not have them: switch Steam to the 1.6 branch to rebuild it.
- The 1.7 project uses the game root (`Assembly-CSharp.dll`, `UnityStubs.dll`, `Waylith.UnityCompat.dll`).
- VFE Classical has no 1.7 folder: both projects reference its 1.6 `VFEC.dll`.
- Paths can be overridden: `-p:RimWorldPath=... -p:RimWorldModPath=...`.

## Deploy
```
.\Deploy-Mod.ps1 -WhatIf
.\Deploy-Mod.ps1
```
Copies the mod to `RimWorld\Mods\VFEClassical_RepublicLectern` without sources, symbols or scripts, after checking that both DLLs exist (`-Versions 1.7` to check only one). Close the game first, or the DLL is locked.
