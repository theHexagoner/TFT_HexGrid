param(
    [string] $pathToIndexHtml,
    [string] $toReplace = '<base href="/" />',
    [string] $replaceWith = '<base href="/TFT_HexGrid/" />'
)

$fileContent = Get-Content -Path $pathToIndexHtml
$newContent = $fileContent.Replace($toReplace, $replaceWith)
Set-Content -Path $pathToIndexHtml -Value $newContent