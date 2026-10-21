<#
.SYNOPSIS
    Copies the mod to the RimWorld Mods folder, keeping only what is needed for playing / publishing.

.DESCRIPTION
    Mirrors this folder to <RimWorld>\Mods\<ModName> and leaves out sources, debug symbols,
    git / Visual Studio files and this script. Files in the destination that are not in the
    source (e.g. an old Source folder) are removed.

.PARAMETER Destination
    Target folder. Default: C:\Program Files (x86)\Steam\steamapps\common\RimWorld\Mods\VFEClassical_RepublicLectern

.PARAMETER Versions
    RimWorld versions whose DLL must exist before deploying. Default: 1.6 (add 1.7 once it is built).

.PARAMETER WhatIf
    Lists what would be copied / deleted without changing anything.

.EXAMPLE
    .\Deploy-Mod.ps1 -WhatIf
    .\Deploy-Mod.ps1
#>
[CmdletBinding()]
param(
    [string]$Destination = 'C:\Program Files (x86)\Steam\steamapps\common\RimWorld\Mods\VFEClassical_RepublicLectern',
    [string[]]$Versions = @('1.6'),
    [switch]$WhatIf
)

$ErrorActionPreference = 'Stop'
$source = $PSScriptRoot
$dll = 'VFEC_RepublicLectern.dll'

# The mod is useless without its DLLs: stop if a requested version has none.
$missing = foreach ($v in $Versions) {
    if (-not (Test-Path (Join-Path $source "$v\Assemblies\$dll"))) { $v }
}
if ($missing) {
    throw "$dll missing for version(s): $($missing -join ', '). Build it first."
}

# Folders excluded (matched by name at any depth)
$excludeDirs = @('Source', '.git', '.vs', '.vscode', '.idea', 'obj', 'bin')
# Files excluded (names / patterns)
$excludeFiles = @('*.pdb', '*.zip', '*.ps1', '.gitignore', '.gitattributes', 'README.md', 'DESIGN.md', '*.slnx', '*.sln', '*.csproj', '*.user')

# /MIR mirrors (and deletes extras in destination); /XD /XF exclude. /NJH /NJS /NP = quieter output.
$args = @($source, $Destination, '/MIR', '/XD') + $excludeDirs + @('/XF') + $excludeFiles + @('/NJH', '/NJS', '/NP', '/R:2', '/W:2')
if ($WhatIf) { $args += '/L' }

Write-Host "Source     : $source"
Write-Host "Destination: $Destination"
if ($WhatIf) { Write-Host '(dry run, nothing is changed)' -ForegroundColor Yellow }

& robocopy @args
# robocopy exit codes 0-7 are success (bit flags), 8+ means errors
if ($LASTEXITCODE -ge 8) { throw "robocopy failed (exit code $LASTEXITCODE)" }

Write-Host 'Done.' -ForegroundColor Green
$global:LASTEXITCODE = 0
