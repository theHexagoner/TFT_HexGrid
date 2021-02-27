param(
    [string] $pathToIndexHtml,
    [string] $toReplace = '<base href="/TFT_HexGrid/" />',
    [string] $replaceWith = '<base href="/" />'
)

$fileContent = Get-Content -Path $pathToIndexHtml
$newContent = $fileContent.Replace($toReplace, $replaceWith)
Set-Content -Path $pathToIndexHtml -Value $newContent