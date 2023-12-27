# Define the path to the deployments directory
$DEPLOYMENTS_DIRECTORY = "C:\Deployments"
$ORIGINAL_LOCATION = Get-Location

# Get the latest zip file
$ZIP_FILE_PATH = Get-ChildItem -Path $DEPLOYMENTS_DIRECTORY -Filter "*.zip" |
                 Sort-Object -Property LastWriteTime -Descending |
                 Select-Object -First 1 -ExpandProperty FullName

# Define the path to the directory where the zip file will be unzipped
$UNZIP_DIRECTORY = "C:\Deployments\unzipped"

# Get the processes that are running the Pi.Api.exe and Jobs.OldScheduler.exe files
$API_PROCESS = Get-Process -Name "Pi.Api" -ErrorAction SilentlyContinue
$SCHEDULER_PROCESS = Get-Process -Name "Jobs.OldScheduler" -ErrorAction SilentlyContinue

# Stop the processes
Write-Host "Stopping processes $API_PROCESS | $SCHEDULER_PROCESS"
if ($API_PROCESS) { Stop-Process -Id $API_PROCESS.Id -Force -PassThru | Wait-Process }
if ($SCHEDULER_PROCESS) { Stop-Process -Id $SCHEDULER_PROCESS.Id -Force -PassThru | Wait-Process }


# Clear the unzip directory
Write-Host "Clearing unzip directory $UNZIP_DIRECTORY"
Remove-Item -Path "$UNZIP_DIRECTORY\*" -Recurse -Force

# Unzip the file
Expand-Archive -Path $ZIP_FILE_PATH -DestinationPath $UNZIP_DIRECTORY
Write-Host "Zip file has been unzipped to $UNZIP_DIRECTORY"

# Get the timestamp from the zip file path
$TIMESTAMP = [System.IO.Path]::GetFileNameWithoutExtension($ZIP_FILE_PATH)

# Define the paths to the Api and Scheduler projects
$API_PROJECT_PATH = "$UNZIP_DIRECTORY\$TIMESTAMP\Api\Pi.Api.exe"
$SCHEDULER_PROJECT_DIRECTORY = "$UNZIP_DIRECTORY\$TIMESTAMP\Scheduler"
$SCHEDULER_PROJECT_PATH = "Jobs.OldScheduler.exe"

# Run the Api project in a new PowerShell terminal and keep the terminal open after the command finishes running
Write-Host "Running command: powershell -NoExit -Command & '$API_PROJECT_PATH'"
Start-Process -FilePath "powershell" -ArgumentList "-NoExit -Command & '$API_PROJECT_PATH'"

# Navigate to the Scheduler project's directory and run the Scheduler project in a new PowerShell terminal, keeping the terminal open after the command finishes running
Write-Host "Navigating to directory: $SCHEDULER_PROJECT_DIRECTORY"
Set-Location -Path $SCHEDULER_PROJECT_DIRECTORY

Write-Host "Running command: powershell -NoExit -Command & '$SCHEDULER_PROJECT_PATH'"
Start-Process -FilePath "powershell" -ArgumentList "-NoExit -Command & '.\$SCHEDULER_PROJECT_PATH'"
Set-Location -Path $ORIGINAL_LOCATION

Write-Host "Api and Scheduler projects have been run in new terminals"

# UI
$UI_UNZIP_DIRECTORY = "$UNZIP_DIRECTORY\$TIMESTAMP\UI\*"
$WEBAPP_DIRECTORY = "C:\web\webapp"

# Remove the contents of the webapp directory
Remove-Item -Path "$WEBAPP_DIRECTORY\*" -Recurse -Force

# Move the unzipped contents of the UI folder to the webapp directory
Copy-Item -Path $UI_UNZIP_DIRECTORY -Destination $WEBAPP_DIRECTORY -Recurse -Force

# Kill the nginx process
Write-Host "Killing nginx process"
Start-Process -FilePath "taskkill" -ArgumentList "/f /im nginx.exe"

# Change the current directory to the nginx directory
Set-Location -Path "C:\web\nginx-1.21.1"

# Run nginx
Write-Host "Starting nginx"
Start-Process -FilePath "nginx"

# Restore the current location
Set-Location -Path $ORIGINAL_LOCATION