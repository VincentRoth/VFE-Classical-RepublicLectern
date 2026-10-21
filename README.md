# VFE Classical – Republic Lectern

Hub for the Republic factions of Vanilla Factions Expanded - Classical: a lectern building that opens a tabbed window, with read-only shortcuts to each Republic's senators and an extension point (`Lectern_PanelDef`) for other mods.

Status: **skeleton** (RimWorld 1.6 first, 1.7 prepared in `Source\1.7`).

## Build (1.6)
```
dotnet build Source\VFEC_RepublicLectern.csproj
```
The DLL goes to `1.6\Assemblies`. Adjust `RimWorldPath` / `RimWorldModPath` if your install differs (`-p:RimWorldPath=...`).

## Deploy
```
.\Deploy-Mod.ps1 -WhatIf
.\Deploy-Mod.ps1
```
