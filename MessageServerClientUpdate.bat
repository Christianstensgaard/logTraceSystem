@echo off
set REPO_URL=https://github.com/Christianstensgaard/MessageServerClient.git
set SUBTREE_PREFIX=messageServerClient
set TARGET_BRANCH=packageBuilder

git --version >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo "Git is not installed. Please install Git to continue."
    exit /b 1
)

cd /d %~dp0

echo Checking if messageServerClient subtree exists...
git ls-tree -d HEAD %SUBTREE_PREFIX% >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo "messageServerClient subtree not found. Adding subtree..."
    git subtree add --prefix=%SUBTREE_PREFIX% %REPO_URL% %TARGET_BRANCH% --squash
    if %ERRORLEVEL% EQU 0 (
        echo "messageServerClient subtree has been successfully added."
    ) else (
        echo "Failed to add messageServerClient. Please check the repository URL and try again."
        exit /b 1
    )
) else (
    echo "messageServerClient subtree found. Updating..."
    git subtree pull --prefix=%SUBTREE_PREFIX% %REPO_URL% %TARGET_BRANCH% --squash
    if %ERRORLEVEL% EQU 0 (
        echo "messageServerClient has been successfully updated."
    ) else (
        echo "Failed to update messageServerClient. Please check the repository URL and try again."
    )
)
