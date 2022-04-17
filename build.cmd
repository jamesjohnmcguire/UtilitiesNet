REM %1 - Version (such as 1.0.0.5)
REM %2 - API key

CD %~dp0

dotnet publish --configuration Release

cd Bin\Release\AnyCPU
dotnet nuget push DigitalZenWorks.Common.Utilities.%1.nupkg --api-key %2 --source https://api.nuget.org/v3/index.json
