#!/usr/bin/env powershell
#requires -version 4

$ArtifactsDir = Join-Path $PSScriptRoot 'artifacts'

dotnet restore
dotnet build --no-restore
dotnet pack --no-build --no-restore -o $ArtifactsDir
