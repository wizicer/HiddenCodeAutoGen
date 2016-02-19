@echo off
rd output /s /q
md output
path %PATH%;C:\Program Files\MSBuild\14.0\Bin;
path %PATH%;C:\Program Files (x86)\MSBuild\14.0\Bin;


MSBuild Generator\Generator.csproj /t:rebuild /p:configuration=Release
copy Generator\bin\Release\HiddenCodeAutoGenerator.vsix

pause
