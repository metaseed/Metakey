
$dir = "$PSScriptRoot\LibToolDemo"
$target = "$PSScriptRoot\..\..\app\Templates\LibToolDemo.zip"
$entries = New-Object System.Collections.Generic.List[System.Object]
$sub = Get-ChildItem $dir -Exclude "bin", "obj" | ForEach-Object { $_.FullName }

foreach ($itm in $sub)
{
    $entries.Add($itm)
}
Compress-Archive -LiteralPath $entries -CompressionLevel Optimal -DestinationPath $target -Force

