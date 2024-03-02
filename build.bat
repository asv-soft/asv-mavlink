@echo off
rem ====== projects ======

set projects=Asv.Mavlink Asv.Mavlink.Shell

rem ====== projects ======

rem set git to global PATH
SET BASH_PATH="%SYSTEMDRIVE%\Program Files\Git\bin"
SET PATH=%BASH_PATH%;%PATH%

rem copy version to text file, then in variable
git describe --tags --abbrev=4 >>version.txt
SET /p VERSION=<version.txt
del version.txt

rem Remove first v from version
SET VERSION=%VERSION:v=%

rem build all projects
(for %%p in (%projects%) do (
  	echo %%p
	dotnet restore ./src/%%p/%%p.csproj
	dotnet build /p:SolutionDir=../;ProductVersion=%VERSION% ./src/%%p/%%p.csproj -c Release
	dotnet pack /p:SolutionDir=../;ProductVersion=%VERSION% ./src/%%p/%%p.csproj -c Release
)) 




