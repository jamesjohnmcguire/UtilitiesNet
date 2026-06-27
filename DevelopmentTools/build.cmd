REM %2 - Version (such as 1.5.1)
REM %3 - API key

CD %~dp0
CD ..\UtilitiesNetLibrary

:default
dotnet build --configuration Release -p:PlatformTarget=AnyCPU -p:Prefer32Bit=false

IF "%1"=="publish" GOTO publish
GOTO end

:publish
if "%~2"=="" GOTO error1
if "%~3"=="" GOTO error2

dotnet pack --configuration Release --output nupkg -p:IncludeSource=true -p:IncludeSymbols=true

CD nupkg

nuget push DigitalZenWorks.Common.Utilities.%2.nupkg %3 -source https://api.nuget.org/v3/index.json

CD ..
GOTO end

:error1
ECHO No version tag specified
GOTO end

:error2
ECHO No API key specified

:end
