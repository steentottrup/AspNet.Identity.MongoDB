$root = (split-path -parent $MyInvocation.MyCommand.Definition) + '\..'
$version = [System.Reflection.Assembly]::LoadFile("$root\src\AspNet.Identity.MongoDB\bin\Release\AspNet.Identity.MongoDB.dll").GetName().Version
$versionStr = "{0}.{1}.{2}" -f ($version.Major, $version.Minor, $version.Build)

Write-Host "Setting .nuspec version tag to $versionStr"

$content = (Get-Content $root\src\AspNet.Identity.MongoDB.nuspec) 
$content = $content -replace '\$version\$',$versionStr

$content | Out-File $root\src\AspNet.Identity.MongoDB.nuspec

& NuGet.exe pack $root\src\AspNet.Identity.MongoDB.nuspec