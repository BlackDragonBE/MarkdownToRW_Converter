REM create folder 
D:\000_Github Repos\MarkdownToRW_Converter\MarkdownToRW\Release

REM GUI
REM Build Release in Visual Studio first
"D:\000_Github Repos\MarkdownToRW_Converter\MarkdownToRW\MarkdownToRW\bin\Release"
"D:\000_Github Repos\MarkdownToRW_Converter\MarkdownToRW\Release\MarkdownToRW_DotNetFramework_Mono_GUI.zip"

REM Portable
call "D:\000_Github Repos\MarkdownToRW_Converter\MarkdownToRW\MarkdownToRWCore\QuickPublish.cmd"

"D:\000_Github Repos\MarkdownToRW_Converter\MarkdownToRW\MarkdownToRWCore\bin\Debug\netcoreapp2.0\publish"
"D:\000_Github Repos\MarkdownToRW_Converter\MarkdownToRW\Release\MarkdownToRW_DotNetCore_Portable.zip"

REM Self Contained
call "D:\000_Github Repos\MarkdownToRW_Converter\MarkdownToRW\MarkdownToRWCore\CreateReleases.cmd"

"D:\000_Github Repos\MarkdownToRW_Converter\MarkdownToRW\MarkdownToRWCore\bin\Release\netcoreapp2.0\win10-x64\publish"
"D:\000_Github Repos\MarkdownToRW_Converter\MarkdownToRW\Release\MarkdownToRW_DotNetCore_Windowsx_SelfContained.zip"

"D:\000_Github Repos\MarkdownToRW_Converter\MarkdownToRW\MarkdownToRWCore\bin\Release\netcoreapp2.0\osx-x64\publish"
"D:\000_Github Repos\MarkdownToRW_Converter\MarkdownToRW\Release\MarkdownToRW_DotNetCore_macOS_SelfContained.zip"


"D:\000_Github Repos\MarkdownToRW_Converter\MarkdownToRW\MarkdownToRWCore\bin\Release\netcoreapp2.0\linux-x64\publish"
"D:\000_Github Repos\MarkdownToRW_Converter\MarkdownToRW\Release\MarkdownToRW_DotNetCore_linux_SelfContained.zip"