#!/usr/bin/env powershell
#requires -version 4

Param([string]$Configuration="Debug")

$ErrorActionPreference = "Stop"
$ArtifactsDir = Join-Path $PSScriptRoot 'artifacts'
$TestResultsDir = Join-Path $ArtifactsDir 'tests'

Get-ChildItem -Path test -Recurse *.csproj | % {
  dotnet test "$($_.FullName)" -c "$Configuration" --no-build -l trx -r "$TestResultsDir"
  $TestsFailed = $TestsFailed -or ($LastExitCode -ne 0)
}
