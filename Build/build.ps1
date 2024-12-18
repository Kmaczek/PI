# Define the base path
$BASE_PATH = "C:\Proj\PI"

# Save the current location
$ORIGINAL_LOCATION = Get-Location

# Define the project locations
$PROJECT_API = "$BASE_PATH\Presentation\Pi.Api\Pi.Api.csproj"
$PROJECT_SCHEDULER = "$BASE_PATH\Jobs\Jobs.OldScheduler\Jobs.OldScheduler.csproj"
$PROJECT_MONITORING = "$BASE_PATH\Monitoring\Monitoring.csproj"
$PROJECT_ANGULAR = "$BASE_PATH\dkapp"

# Get the current timestamp
$TIMESTAMP = Get-Date -Format "yyyyMMddHHmmss"

# Define the publish locations
$PUBLISH_PATH_API = "$BASE_PATH\Build\$TIMESTAMP\Api"
$PUBLISH_PATH_SCHEDULER = "$BASE_PATH\Build\$TIMESTAMP\Scheduler"
$PUBLISH_PATH_MONITORING = "$BASE_PATH\Build\$TIMESTAMP\Monitoring"
$PUBLISH_PATH_ANGULAR = "$BASE_PATH\Build\$TIMESTAMP\Ui"

# Restore the projects
dotnet restore $PROJECT_API
dotnet restore $PROJECT_SCHEDULER
dotnet restore $PROJECT_MONITORING

# Build the projects
dotnet build $PROJECT_API --configuration Release
dotnet build $PROJECT_SCHEDULER --configuration Release
dotnet build $PROJECT_MONITORING --configuration Release

# Publish the projects
dotnet publish $PROJECT_API --configuration Release --output $PUBLISH_PATH_API
dotnet publish $PROJECT_SCHEDULER --configuration Release --output $PUBLISH_PATH_SCHEDULER
dotnet publish $PROJECT_MONITORING --configuration Release --output $PUBLISH_PATH_MONITORING

Write-Host "API has been published to $PUBLISH_PATH_API"
Write-Host "Scheduler has been published to $PUBLISH_PATH_SCHEDULER"
Write-Host "Monitoring has been published to $PUBLISH_PATH_MONITORING"

# Change to the Angular project directory
Set-Location -Path $PROJECT_ANGULAR

# Install the Angular project dependencies
npm install

# Build the Angular project
ng build --configuration production --output-path $PUBLISH_PATH_ANGULAR

Write-Host "Angular project has been built and published to $PUBLISH_PATH_ANGULAR"

# Change back to the original location
Set-Location -Path $ORIGINAL_LOCATION

# ZIP
# Define the path to the directory to zip
$DIRECTORY_TO_ZIP = "$BASE_PATH\Build\$TIMESTAMP"

# Define the path to the output zip file
$ZIP_FILE_PATH = "$BASE_PATH\Build\$TIMESTAMP.zip"

# Zip the directory
Compress-Archive -Path $DIRECTORY_TO_ZIP -DestinationPath $ZIP_FILE_PATH

Write-Host "Directory has been zipped to $ZIP_FILE_PATH"


# Define the path to the destination folder on the other PC
$DESTINATION_PATH = "\\SERWEREK\Deployments\$TIMESTAMP.zip"

# Move the zip file to the other PC
Move-Item -Path $ZIP_FILE_PATH -Destination $DESTINATION_PATH

Write-Host "Zip file has been moved to $DESTINATION_PATH"