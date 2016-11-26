@echo off
pushd "%~dp0"

if exist RedistPackages rd /s /q RedistPackages
if not exist RedistPackages mkdir RedistPackages

.nuget\NuGet.exe pack Package.nuspec -OutputDirectory RedistPackages\
.nuget\NuGet.exe push RedistPackages\ "https://www.nuget.org/api/v2/package"

pause
popd
@echo on