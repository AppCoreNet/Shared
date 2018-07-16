#!/usr/bin/env powershell
#requires -version 4

Param([string]$Configuration="Debug")

$ErrorActionPreference = "Stop"
$ArtifactsDir = Join-Path $PSScriptRoot 'artifacts'

dotnet vstest ((Get-ChildItem -Recurse *.Tests.dll | % FullName) -NotMatch "obj" -Match "netcoreapp") "/Framework:.NETCoreApp,Version=v2.0" "/Logger:trx;LogFileName=test-results-netcore.trx" /ResultsDirectory:$ArtifactsDir
If ($LastExitCode -ne 0) { throw "Tests failed." }

If ($env:CI_LINUX -ne "true") {
  dotnet vstest ((Get-ChildItem -Recurse *.Tests.dll | % FullName) -NotMatch "obj" -Match "net452") "/Framework:.NETFramework,Version=v4.5.2" "/Logger:trx;LogFileName=test-results-net452.trx" /ResultsDirectory:$ArtifactsDir
  If ($LastExitCode -ne 0) { throw "Tests failed." }
}
