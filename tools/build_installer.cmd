@echo off

echo Publish in Release configuration
dotnet publish -c Release ..\QuickSheet\QuickSheet\QuickSheet.csproj

if %errorlevel% neq 0 (
    echo Publish failed. Exiting...
    exit /b 1
)

echo Publish successful
echo Compile inno setup script

iscc setup_script.iss

if %errorlevel% neq 0 (
    echo Setup script compile failed. Exiting...
    exit /b 1
)

echo Setup script compiled successfully
exit /b 0