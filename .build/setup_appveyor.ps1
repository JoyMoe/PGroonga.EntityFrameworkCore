Write-Host Enabling PGroonga...
If (!(Test-Path $env:PGROONGA_ZIP)) {
  Write-Host Downloading PGroonga...
  (New-Object Net.WebClient).DownloadFile("https://github.com/pgroonga/pgroonga/releases/download/2.2.1/$env:PGROONGA_ZIP", "$env:PGROONGA_ZIP")
}
Add-Type -AssemblyName System.IO.Compression.FileSystem
[System.IO.Compression.ZipFile]::ExtractToDirectory(".\$env:PGROONGA_ZIP", "C:\Program Files\PostgreSQL\10")
