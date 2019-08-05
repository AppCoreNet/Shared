[CmdletBinding()]
param (
  [Parameter(ValueFromRemainingArguments=$True)][string[]] $Arguments
)

$ErrorActionPreference = "Stop"

$ToolsPath = "./build/tools"
$CakeVersion = "0.34.1"

$Cake = "$ToolsPath/dotnet-cake"
$CakeArgs = @("--paths_tools=$ToolsPath")

function Exec {
  param ([ScriptBlock] $ScriptBlock)
  & $ScriptBlock
  If ($LASTEXITCODE -ne 0) {
    Write-Error "Execution failed with exit code $LASTEXITCODE"
  }
}

If (!(Test-Path "$ToolsPath/.store/cake.tool")) {
  Exec { & dotnet tool install --tool-path $ToolsPath Cake.Tool --version $CakeVersion }
  Exec { & $Cake @CakeArgs --bootstrap }
}

Exec { & $Cake @CakeArgs @Arguments }
