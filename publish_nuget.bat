@echo off
rem ====== projects ======

set projects=Asv.Mavlink Asv.Mavlink.Shell Asv.Mavlink.Payload Asv.Mavlink.Vehicle

rem ====== projects ======

rem set git to global PATH
SET BASH_PATH="%SYSTEMDRIVE%\Program Files\Git\bin"
SET PATH=%BASH_PATH%;%PATH%

rem copy version to text file, then in variable
%BASH_PATH%\bash.exe tools/get_version.sh > ./version.txt
SET /p VERSION=<version.txt
DEL version.txt

(for %%p in (%projects%) do (
	cd src\%%p\bin\Release\
	dotnet nuget push %%p.%VERSION%.nupkg --source https://api.nuget.org/v3/index.json
	dotnet nuget push %%p.%VERSION%.nupkg --source https://nuget.pkg.github.com/asvol/index.json
	cd ../../../../
)) 





