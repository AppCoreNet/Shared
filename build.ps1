#!/usr/bin/env powershell
#requires -version 4

Param([string]$VersionSuffix="",[Int32]$BuildNumber=0)

$ArtifactsDir = Join-Path $PSScriptRoot 'artifacts'

$ExtraArgs = ""
If ($VersionSuffix.length -gt 0)
{
	$ExtraArgs = "/p:VersionSuffix=$VersionSuffix{0:0000}" -f $BuildNumber
}

dotnet restore $ExtraArgs
dotnet build --no-restore $ExtraArgs
dotnet pack --no-build --no-restore -o $ArtifactsDir $ExtraArgs
