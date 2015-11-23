# Install mongodb as a windows service
cmd /c C:\mongodb\mongod.exe --logpath=c:\mongodb\log --dbpath=c:\mongodb\data\db\ --smallfiles --install

# Sleep as a hack to fix an issue where the service sometimes does not finish installing quickly enough
Start-Sleep -Seconds 5

# Start mongodb service
net start mongodb

# Return to last location, to run the build
Pop-Location

Write-Host
Write-Host "monogdb installation complete"