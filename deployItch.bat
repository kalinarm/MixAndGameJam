@echo off
set PROJECT_PATH=%CD%
set BUILD_PATH=%PROJECT_PATH%\Build\SpeedDicing
set LOG_PATH=%CD%\deployItch.log

set ITCH_KEY=%APPDATA%/itch/butler_creds
set USER=kalinka
set GAME=speeddicing
set CHANNEL=Web

echo ---------------------------------------------------------------
echo      Deploy to Itch.io
echo Current dir = %CD%
echo Path to project = %PROJECT_PATH%
echo Export log to = %LOG_PATH%
echo ---------------------------------------------------------------

echo pushing to itch %USER%/%GAME%:%CHANNEL% 
echo build finded here : %BUILD_PATH%

butler push %BUILD_PATH% %USER%/%GAME%:%CHANNEL%
