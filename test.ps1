#!/usr/bin/env powershell
#requires -version 4

Param([string]$Configuration="Debug")

$ErrorActionPreference = "Stop"
$ArtifactsDir = Join-Path $PSScriptRoot 'artifacts'

dotnet vstest ((Get-ChildItem -Recurse *.Tests.dll | % FullName) -Match "bin") "/Logger:trx;LogFileName=test-results.trx" /ResultsDirectory:$ArtifactsDir
If ($LastExitCode -ne 0) { throw "Tests failed." }
