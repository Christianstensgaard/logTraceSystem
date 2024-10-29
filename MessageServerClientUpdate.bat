@echo off
:: Handling Updating the Message Server and Client from external Git

set REPO_URL=https://github.com/Christianstensgaard/MessageServerClient.git
set SUBTREE_PREFIX=messageServerClient
set TARGET_BRANCH=master

git --version >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo "Git is not installed. Please install Git to continue."
    exit /b 1
)

cd /d %~dp0

echo Updating the messageServerClient repository...
git subtree pull --prefix=%SUBTREE_PREFIX% %REPO_URL% %TARGET_BRANCH% --squash

if %ERRORLEVEL% EQU 0 (
    echo "messageServerClient has been successfully updated."
) else (
    echo "Failed to update messageServerClient. Please check the repository URL and try again."
)
