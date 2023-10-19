REM %1 - Version (such as 1.0.0.5)
REM %2 - API key

CD %~dp0
CD ..

IF "%1"=="publish" GOTO publish

:default
dotnet build --configuration Release

GOTO end

:publish

if "%~2"=="" GOTO error1
if "%~3"=="" GOTO error2

msbuild -property:Configuration=Release -restore -target:rebuild;pack UtilitiesNET.csproj

CD bin\Release

nuget push DigitalZenWorks.Common.Utilities.%2.nupkg %3 -source https://api.nuget.org/v3/index.json

cd ..\..\..
GOTO end

:error1
ECHO No version tag specified
GOTO end

:error2
ECHO No API key specified

:end
