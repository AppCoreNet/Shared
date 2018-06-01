#!/usr/bin/env powershell
#requires -version 4

Param([string]$Configuration="Debug",[string]$VersionSuffix="",[Int32]$BuildNumber=0)

$ArtifactsDir = Join-Path $PSScriptRoot 'artifacts'

$ExtraArgs = @()
$ExtraBuildArgs = @()

If ($VersionSuffix.length -gt 0)
{
	$ExtraArgs += "/p:VersionSuffix=$VersionSuffix{0:0000}" -f $BuildNumber
}

If ($BuildNumber.length -gt 0)
{
	$ExtraArgs += "/p:BuildNumber={0:0000}" -f $BuildNumber
}

If ($Configuration.length -gt 0)
{
	$ExtraBuildArgs += "--configuration", "$Configuration"
}

dotnet restore $ExtraArgs
dotnet build --no-restore $ExtraBuildArgs $ExtraArgs
dotnet pack --no-build --no-restore $ExtraBuildArgs -o $ArtifactsDir $ExtraArgs
