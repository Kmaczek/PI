# Define the path to the deployments directory
$DEPLOYMENTS_DIRECTORY = "C:\Deployments"
$ORIGINAL_LOCATION = Get-Location
$UNZIP_DIRECTORY = "C:\Deployments\current"

# Function to stop processes and wait for them to fully terminate
function Stop-ProcessSafely {
    param (
        [string]$ProcessName,
        [int]$TimeoutSeconds = 30
    )
    
    $process = Get-Process -Name $ProcessName -ErrorAction SilentlyContinue
    if ($process) {
        Write-Host "Stopping $ProcessName process..."
        Stop-Process -Id $process.Id -Force
        
        # Wait for process to fully terminate
        $timeout = [DateTime]::Now.AddSeconds($TimeoutSeconds)
        while ((Get-Process -Name $ProcessName -ErrorAction SilentlyContinue) -and ([DateTime]::Now -lt $timeout)) {
            Start-Sleep -Milliseconds 500
        }
        
        if (Get-Process -Name $ProcessName -ErrorAction SilentlyContinue) {
            Write-Host "Warning: $ProcessName process did not terminate within $TimeoutSeconds seconds"
        }
    }
}

# Function to restart nginx reliably
function Restart-Nginx {
    Write-Host "Stopping nginx..."
    
    # Try graceful stop first
    $nginxProcess = Get-Process -Name "nginx" -ErrorAction SilentlyContinue
    if ($nginxProcess) {
        # Try graceful stop
        Start-Process -FilePath "C:\web\nginx-1.21.1\nginx.exe" -ArgumentList "-s quit" -Wait
        Start-Sleep -Seconds 2
        
        # Force kill if still running
        $nginxProcess = Get-Process -Name "nginx" -ErrorAction SilentlyContinue
        if ($nginxProcess) {
            Stop-Process -Name "nginx" -Force
            Start-Sleep -Seconds 2
        }
    }
    
    # Start nginx
    Write-Host "Starting nginx..."
    Set-Location -Path "C:\web\nginx-1.21.1"
    Start-Process -FilePath "nginx.exe" -NoNewWindow
    Start-Sleep -Seconds 2
    
    # Verify nginx is running
    if (Get-Process -Name "nginx" -ErrorAction SilentlyContinue) {
        Write-Host "Nginx successfully restarted"
    }
    else {
        Write-Host "Warning: Nginx failed to start"
    }
}

# Get the latest zip file
$ZIP_FILE_PATH = Get-ChildItem -Path $DEPLOYMENTS_DIRECTORY -Filter "*.zip" |
Sort-Object -Property LastWriteTime -Descending |
Select-Object -First 1 -ExpandProperty FullName

# Stop all related processes
Stop-ProcessSafely -ProcessName "Pi.Api"
Stop-ProcessSafely -ProcessName "Jobs.OldScheduler"

# Stop the monitoring service first
$serviceName = "ApplicationMonitorService"
if (Get-Service $serviceName -ErrorAction SilentlyContinue) {
    Write-Host "Stopping monitoring service..."
    Stop-Service $serviceName -Force
    Start-Sleep -Seconds 2
    sc.exe delete $serviceName
    Start-Sleep -Seconds 2
}

# Clear the current deployment directory
Write-Host "Clearing current deployment directory $UNZIP_DIRECTORY"
if (Test-Path $UNZIP_DIRECTORY) {
    try {
        Remove-Item -Path "$UNZIP_DIRECTORY\*" -Recurse -Force -ErrorAction Stop
    }
    catch {
        Write-Host "Error clearing directory. Waiting 5 seconds and trying again..."
        Start-Sleep -Seconds 5
        Remove-Item -Path "$UNZIP_DIRECTORY\*" -Recurse -Force -ErrorAction Stop
    }
}
else {
    New-Item -ItemType Directory -Path $UNZIP_DIRECTORY -Force
}

# Create a temporary directory for unzipping
$TEMP_UNZIP_DIR = "C:\Deployments\temp"
if (Test-Path $TEMP_UNZIP_DIR) {
    Remove-Item -Path "$TEMP_UNZIP_DIR\*" -Recurse -Force
}
else {
    New-Item -ItemType Directory -Path $TEMP_UNZIP_DIR -Force
}

# Unzip to temp directory first
Write-Host "Unzipping deployment package..."
Expand-Archive -Path $ZIP_FILE_PATH -DestinationPath $TEMP_UNZIP_DIR -Force

# Get the timestamp directory name
$TIMESTAMP_DIR = Get-ChildItem -Path $TEMP_UNZIP_DIR | Select-Object -First 1

# Move contents from timestamped folder to current directory
Write-Host "Moving files to deployment directory..."
try {
    Copy-Item -Path "$TEMP_UNZIP_DIR\$($TIMESTAMP_DIR.Name)\*" -Destination $UNZIP_DIRECTORY -Recurse -Force -ErrorAction Stop
}
catch {
    Write-Host "Error during file copy. Waiting 10 seconds and trying again..."
    Start-Sleep -Seconds 10
    Copy-Item -Path "$TEMP_UNZIP_DIR\$($TIMESTAMP_DIR.Name)\*" -Destination $UNZIP_DIRECTORY -Recurse -Force
}

# Clean up temp directory
Remove-Item -Path $TEMP_UNZIP_DIR -Recurse -Force -ErrorAction SilentlyContinue

# Define the paths to the Api and Scheduler projects
$API_PROJECT_DIRECTORY = "$UNZIP_DIRECTORY\Api"
$API_PROJECT_PATH = "Pi.Api.exe"
$SCHEDULER_PROJECT_DIRECTORY = "$UNZIP_DIRECTORY\Scheduler"
$SCHEDULER_PROJECT_PATH = "Jobs.OldScheduler.exe"
$MONITORING_PROJECT_DIRECTORY = "$UNZIP_DIRECTORY\Monitoring"

# Run the Api project in a new PowerShell terminal
Write-Host "Starting Api project..."
Set-Location -Path $API_PROJECT_DIRECTORY
Start-Process -FilePath "powershell" -ArgumentList "-NoExit -Command & '.\$API_PROJECT_PATH'"

# Start the Scheduler project
Write-Host "Starting Scheduler project..."
Set-Location -Path $SCHEDULER_PROJECT_DIRECTORY
Start-Process -FilePath "powershell" -ArgumentList "-NoExit -Command & '.\$SCHEDULER_PROJECT_PATH'"
Set-Location -Path $ORIGINAL_LOCATION

# Install and start monitoring service
Write-Host "Setting up monitoring service..."
Set-Location -Path $MONITORING_PROJECT_DIRECTORY

$serviceDisplayName = "Application Monitor Service"
$serviceDescription = "Monitors and auto-restarts .NET, Angular, and Scheduler applications"
$servicePath = "$MONITORING_PROJECT_DIRECTORY\Monitoring.exe"

# Create and start the new service
New-Service -Name $serviceName `
    -DisplayName $serviceDisplayName `
    -Description $serviceDescription `
    -BinaryPathName $servicePath `
    -StartupType Automatic

Start-Sleep -Seconds 2
Start-Service $serviceName

Set-Location -Path $ORIGINAL_LOCATION
Write-Host "Monitoring service started"

# UI Deployment
$UI_SOURCE_DIRECTORY = "$UNZIP_DIRECTORY\UI\*"
$WEBAPP_DIRECTORY = "C:\web\webapp"

# Remove the contents of the webapp directory
Write-Host "Updating UI files..."
Remove-Item -Path "$WEBAPP_DIRECTORY\*" -Recurse -Force

# Move the unzipped contents of the UI folder to the webapp directory
Copy-Item -Path $UI_SOURCE_DIRECTORY -Destination $WEBAPP_DIRECTORY -Recurse -Force

# Restart nginx
Restart-Nginx

# Restore the current location
Set-Location -Path $ORIGINAL_LOCATION

Write-Host "Deployment completed successfully"