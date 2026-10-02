@echo off
setlocal
pushd "%~dp0"

set "SHELL_PROJECT=src\Asv.Mavlink.Shell\Asv.Mavlink.Shell.csproj"
set "TEMPLATE=src\Asv.Mavlink.Shell\Resources\csharp.liquid"
set "DIALECTS=src\Asv.Mavlink\Protocol\Dialects"
set "MESSAGES=src\Asv.Mavlink\Protocol\Messages"

dotnet build "%SHELL_PROJECT%" -c Debug --no-restore || goto :fail

call :generate asv_gbs.xml || goto :fail
call :generate asv_sdr.xml || goto :fail
call :generate asv_audio.xml || goto :fail
call :generate asv_radio.xml || goto :fail
call :generate asv_chart.xml || goto :fail
call :generate asv_rsga.xml || goto :fail

goto :success

:generate
dotnet run --project "%SHELL_PROJECT%" -c Debug --no-build --no-restore -- gen -e cs -tmpl "%TEMPLATE%" -t %1 -i "%DIALECTS%" -o "%MESSAGES%"
exit /b %ERRORLEVEL%

:success
popd
exit /b 0

:fail
set "RESULT=%ERRORLEVEL%"
popd
exit /b %RESULT%
