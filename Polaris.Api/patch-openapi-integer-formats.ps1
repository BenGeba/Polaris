$inputFile = "immich-openapi.json"
$outputFile = "immich-openapi.patched.json"

$json = Get-Content $inputFile -Raw | ConvertFrom-Json -Depth 100

function Patch-OpenApiIntegerFormats {
    param ($node)

    if ($null -eq $node) {
        return
    }

    if ($node -is [System.Collections.IEnumerable] -and $node -isnot [string]) {
        foreach ($item in $node) {
            Patch-OpenApiIntegerFormats $item
        }

        return
    }

    if ($node -is [pscustomobject]) {
        $properties = $node.PSObject.Properties.Name

        if (
            $properties -contains "type" -and
            $node.type -eq "integer" -and
            -not ($properties -contains "format") -and
            $properties -contains "maximum" -and
            [double]$node.maximum -gt [int]::MaxValue
        ) {
            $node | Add-Member -NotePropertyName "format" -NotePropertyValue "int64"
        }

        foreach ($property in $node.PSObject.Properties) {
            Patch-OpenApiIntegerFormats $property.Value
        }
    }
}

Patch-OpenApiIntegerFormats $json

$json | ConvertTo-Json -Depth 100 | Set-Content $outputFile -Encoding UTF8