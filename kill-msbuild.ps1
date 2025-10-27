# Jetbrains Rider will hold onto your plugin's DLL until you close it.
# This script will kill MSBuild design-time build processes
Get-Process dotnet -ErrorAction SilentlyContinue | Where-Object {
    $cmdLine = (Get-CimInstance Win32_Process -Filter "ProcessId = $($_.Id)").CommandLine
    $cmdLine -like "*MSBuild.dll*"
} | ForEach-Object {
    Write-Host "Killing MSBuild process: PID $($_.Id)"
    Stop-Process -Id $_.Id -Force
}
Write-Host "Done."
