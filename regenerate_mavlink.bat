@echo off
setlocal
pushd "%~dp0"

set "SHELL_PROJECT=src\Asv.Mavlink.Shell\Asv.Mavlink.Shell.csproj"
set "TEMPLATE=src\Asv.Mavlink.Shell\Resources\csharp.liquid"
set "DIALECTS=src\Asv.Mavlink\Protocol\Dialects"
set "MESSAGES=src\Asv.Mavlink\Protocol\Messages"

dotnet build "%SHELL_PROJECT%" -c Debug --no-restore || goto :fail
dotnet run --project "%SHELL_PROJECT%" -c Debug --no-build --no-restore -- gen -e cs -tmpl "%TEMPLATE%" -t all.xml -i "%DIALECTS%" -o "%MESSAGES%" || goto :fail

goto :success

:success
popd
exit /b 0

:fail
set "RESULT=%ERRORLEVEL%"
popd
exit /b %RESULT%
