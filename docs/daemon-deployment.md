# Daemon Deployment Steps

1. Build and publish the Daemon project.

   `dotnet publish --configuration Release .\src\Daemon\Daemon.csproj`

2. Copy the contents of the publish directory to the web server.

3. Register the app as a Windows service.

   `sc.exe create "Air Quality Notification Service" binpath="D:\path-to-services\AirQualityAlerts.Daemon.exe"`

4. Start the service.

   `sc.exe start "Air Quality Notification Service"`
