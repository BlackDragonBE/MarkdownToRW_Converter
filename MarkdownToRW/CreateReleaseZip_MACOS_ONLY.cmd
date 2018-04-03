set /P version=Enter release number (e.g.: 1.28): 

:: create folder 
mkdir Release

(
echo namespace DragonMarkdown
echo {
echo     public static class DragonVersion
echo     {
echo         public static readonly decimal VERSION = %version%m;
echo     }
echo }
) > "%~dp0\MarkdownConverter\DragonVersion.cs"

:: Winforms / Mono GUI
:: "C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\Common7\IDE\devenv.exe" "%~dp0\MarkdownToRW.sln" /Build Release
::"%~dp0/7z/7za.exe" a -tzip "%~dp0\Release\MarkdownToRW_DotNetFramework_Mono_GUI_%version%.zip" "%~dp0\MarkdownToRW\bin\Release\*"

:: CORE Updater

cd CoreUpdater
call QuickPublish.cmd
call CreateMacStandalone.cmd
cd ..

"%~dp0/7z/7za.exe" a -tzip "%~dp0\MarkdownToRWGUI\bin\Debug\netcoreapp2.0\publish\CoreUpdater.zip" "%~dp0\CoreUpdater\bin\Debug\netcoreapp2.0\netcoreapp2.0\publish\*"

:: CORE Portable GUI

cd MarkdownToRWGUI
call QuickPublish.cmd
call CreateMacStandalone.cmd
cd ..

:: CORE Self Contained GUI
"%~dp0/7z/7za.exe" a -tzip "%~dp0\MarkdownToRWGUI\bin\Release\netcoreapp2.0\osx-x64\publish\CoreUpdater.zip" "%~dp0\CoreUpdater\bin\Release\netcoreapp2.0\netcoreapp2.0\osx-x64\publish\*"
"%~dp0/7z/7za.exe" a -tzip "%~dp0\Release\MarkdownToRW_GUI_macOS_SelfContained_%version%.zip" "%~dp0\MarkdownToRWGUI\bin\Release\netcoreapp2.0\osx-x64\publish\*"
