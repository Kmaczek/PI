# Define the base path
$BASE_PATH = "C:\Proj\PI"

# Save the current location
$ORIGINAL_LOCATION = Get-Location

# Define the project locations
$PROJECT_PATH1 = "$BASE_PATH\Presentation\Pi.Api\Pi.Api.csproj"
$PROJECT_PATH2 = "$BASE_PATH\Jobs\Jobs.OldScheduler\Jobs.OldScheduler.csproj"
$ANGULAR_PROJECT_PATH = "$BASE_PATH\dkapp"

# Get the current timestamp
$TIMESTAMP = Get-Date -Format "yyyyMMddHHmmss"

# Define the publish locations
$PUBLISH_PATH1 = "$BASE_PATH\Build\$TIMESTAMP\Api"
$PUBLISH_PATH2 = "$BASE_PATH\Build\$TIMESTAMP\Scheduler"
$ANGULAR_PUBLISH_PATH = "$BASE_PATH\Build\$TIMESTAMP\Ui"

# Restore the projects
dotnet restore $PROJECT_PATH1
dotnet restore $PROJECT_PATH2

# Build the projects
dotnet build $PROJECT_PATH1 --configuration Release
dotnet build $PROJECT_PATH2 --configuration Release

# Publish the projects
dotnet publish $PROJECT_PATH1 --configuration Release --output $PUBLISH_PATH1
dotnet publish $PROJECT_PATH2 --configuration Release --output $PUBLISH_PATH2

Write-Host "API has been published to $PUBLISH_PATH1"
Write-Host "Scheduler has been published to $PUBLISH_PATH2"

# Change to the Angular project directory
Set-Location -Path $ANGULAR_PROJECT_PATH

# Install the Angular project dependencies
npm install

# Build the Angular project
ng build --configuration production --output-path $ANGULAR_PUBLISH_PATH

Write-Host "Angular project has been built and published to $ANGULAR_PUBLISH_PATH"

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