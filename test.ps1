#!/usr/bin/env powershell
#requires -version 4

$ArtifactsDir = Join-Path $PSScriptRoot 'artifacts'

dotnet vstest ((ls -Recurse *.Tests.dll | % FullName) -Match "bin") "/Logger:trx;LogFileName=test-results.trx" /ResultsDirectory:$ArtifactsDir
