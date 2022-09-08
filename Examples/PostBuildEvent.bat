@echo off

Rem Assuming that output files already copied to the target mechine (by changing the BaseOutputPath of that project)

set linux_command="sudo chmod -R 777 ~/My; sudo killall -s SIGKILL dotnet; cd ~/My/App/ARM64/Debug/net6.0; ~/dotnet/dotnet App.dll -d"
set destination="pi@192.168.1.7"
set password="raspberry"
set process_name="/home/pi/dotnet/dotnet"
set kill_dotnet=plink -batch -ssh %destination% -pw %password% sudo killall -s SIGKILL dotnet

start cmd /c "plink -batch -ssh %destination% -pw %password% %linux_command% & %kill_dotnet% & timeout 3"

echo.
echo Waiting for process to start, press a key when process is ready.
timeout 7

pushd %~dp0\..\RemoteDebugging

echo Attaching..
AttachDebugger -t SSH -e "Managed (.NET Core for Unix)" -d %destination% -n %process_name%

if %ERRORLEVEL% NEQ 0 timeout 10

pushd %~dp0
