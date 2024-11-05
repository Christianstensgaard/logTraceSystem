@echo off
setlocal enabledelayedexpansion

set REPO_URL=https://github.com/Christianstensgaard/MessageServerClient.git
set SUBTREE_PREFIX=MessageServerClient
set TARGET_BRANCH=master

:: Check if Git is installed
git --version >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo "Git is not installed. Please install Git to continue."
    exit /b 1
)

cd /d %~dp0

:: Remove the subtree if it already exists
echo Checking if %SUBTREE_PREFIX% subtree exists...
git ls-tree -d HEAD %SUBTREE_PREFIX% >nul 2>&1
if %ERRORLEVEL% EQU 0 (
    echo "Existing %SUBTREE_PREFIX% subtree found. Removing..."
    git rm -rf %SUBTREE_PREFIX%
    if %ERRORLEVEL% EQU 0 (
        echo "Successfully staged %SUBTREE_PREFIX% subtree for removal."
    ) else (
        echo "Failed to remove %SUBTREE_PREFIX%. Exiting."
        exit /b 1
    )

    git commit -m "Remove existing %SUBTREE_PREFIX% subtree" >nul 2>&1
    if %ERRORLEVEL% NEQ 0 (
        echo "No changes to commit, skipping commit step."
    )
)

:: Clean up any lingering files in the directory
if exist %SUBTREE_PREFIX% (
    echo "Cleaning up remaining files in %SUBTREE_PREFIX% directory..."
    rmdir /s /q %SUBTREE_PREFIX%
    if exist %SUBTREE_PREFIX% (
        echo "Failed to delete %SUBTREE_PREFIX% directory. Exiting."
        exit /b 1
    )
)

:: Add the subtree afresh
echo Adding %SUBTREE_PREFIX% subtree...
git subtree add --prefix=%SUBTREE_PREFIX% %REPO_URL% %TARGET_BRANCH% --squash
if %ERRORLEVEL% EQU 0 (
    echo "%SUBTREE_PREFIX% subtree has been successfully added."
) else (
    echo "Failed to add %SUBTREE_PREFIX%. Please check the repository URL and try again."
    exit /b 1
)

endlocal
