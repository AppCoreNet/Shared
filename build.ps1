#!/usr/bin/env powershell
#requires -version 4

Param([string]$Configuration="Debug",[string]$VersionSuffix="",[string]$BuildNumber="")

$ArtifactsDir = Join-Path $PSScriptRoot 'artifacts'

$ExtraArgs = @()
$ExtraBuildArgs = @()

If ($VersionSuffix.length -gt 0)
{
	$ExtraArgs += "/p:VersionSuffix=$VersionSuffix"
}

If ($BuildNumber.length -gt 0)
{
	$ExtraArgs += "/p:BuildNumber={0:0000}" -f [convert]::ToInt32($BuildNumber, 10)
}

If ($Configuration.length -gt 0)
{
	$ExtraBuildArgs += "--configuration", "$Configuration"
}

dotnet restore $ExtraArgs
dotnet build --no-restore $ExtraBuildArgs $ExtraArgs
dotnet pack --no-build --no-restore $ExtraBuildArgs -o $ArtifactsDir $ExtraArgs
