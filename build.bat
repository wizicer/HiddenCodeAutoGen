@echo off
rd output /s /q
md output
%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\MSBuild IcerWPFSmartGen\IcerWPFSmartGen.csproj /t:rebuild /p:configuration=Release
copy IcerWPFSmartGen\bin\Release\*.dll output\*.dll
copy UnitTests\AutoGen*.cs output\*.cs

