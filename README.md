# MarkdownToRW Converter

A portable tool to convert any markdown document to Ray Wenderlich WordPress ready HTML.
It can also batch upload all locally sourced images and replace those sources with the image URLs.

There are currently 3 versions of the app:

**The .NET Framework / Mono GUI version:**

![](READMEImages/UI.png)

This version has a graphical interface and runs natively on Windows. It can also run on macOS and linux with the help of the Mono Runtime.

**The .NET Core Portable version:**

This is a "portable" console version as in the binaries & libraries are small and can be shared between different platforms. You do need an install of the .NET Core Framework for your platform for it to work though.

**The .NET Core Self Contained version:**

The same console application as above, but with all the .NET Core libraries packed with it for every particular platform. This version includes native binaries and libraries and isn't dependent on any other software, but it's the largest in size.

# Requirements

**.NET Framework / Mono GUI Version**

- Windows: .NET Framework 4.6.1
- macOS & linux: Mono Framework

**.NET Core Portable**

For the portable version to work, you need the .NET Core libraries for your platform:

- Windows: https://www.microsoft.com/net/download/windows/run
- macOS: https://www.microsoft.com/net/download/macos/run
- linux: https://www.microsoft.com/net/download/linux/run

Make sure to open a console/terminal afterwards and run `dotnet -v` to verify it's installed correctly.

**.NET Core Self Contained**

- Windows: Windows 7 SP1 or later
- macOS: macOS 10.12 Sierra or later
- linux: https://docs.microsoft.com/en-us/dotnet/core/linux-prerequisites?tabs=netcore2x

For up to date list, see: https://github.com/dotnet/core/blob/master/release-notes/2.0/2.0-supported-os.md

The self contained version includes native binaries & libraries for your platform ans thus doesn't require any additional software.
If it doesn't work for any reason, try the **.NET Core Portable** version instead.

(**Note:** The self contained version is the largest by far in file size as it contains a minimal set of .NET Core 2.0 framework libraries)

All of these use the same shared library for converting markdown files and communicating with WordPress so they mostly share the same functionality (and bugs).

# How To Run The Application

For all platforms and versions, download the latest release zip for your platform (if applicable) and unpack it somewhere convenient.
Releases: https://github.com/BlackDragonBE/MarkdownToRW_Converter/releases

## .NET Framework / Mono GUI

### Windows

Run **MarkdownToRW.exe**.

### macOS

1. Download and install Mono: http://www.mono-project.com/download/stable/
2. Restart your machine.
3. Open a terminal.
4. Run this command: `mono --arch=32 PATH-TO-MarkdownToRW.exe`

### linux

1. Open a terminal
2. Install the mono-complete package. (e.g. sudo apt-get install mono-complete).
3. Run this command: `mono PATH-TO-MarkdownToRW.exe`

You can create a shortcut to run this command to make it easier in the future.
On linux, you can let .exe files open with the Mono Runtime by default.

## .NET Core Portable

Open a command prompt or terminal and run:

`dotnet PATH-TO-MarkdownToRW.dll`

## .NET Core Self Contained

Run the native executable named **MarkdownToRW** in a console/terminal. You have to mark it as executable if on linux (and macOS?).

# How To Use

## .NET Framework / Mono GUI

Click on the **Open Markdown...** button and choose a markdown file.
The file will now be read and used as input. It will also convert it to a special RW compatible flavor of html and put that on the right side of the window.
You can now preview the result by pressing the **Show Preview...** button or copy the html to your clipboard by pressing the **Copy HTML to clipboard** button.

You can also upload all images locally sourced in your markdown file and update the markdown and HTML with the new image URLs by pressing the Upload Images... button.
This will open up the **Image Uploader** window. To start uploading you need to verify your account first.
Put in your RW WordPress credentials and click the **Verify** button. If your credentials are valid, you'll be greeted and the upload button will become enabled.

**Check that all found images are correct in the list on the right side before moving on and make sure you only upload the images once per tutorial or article.**

Press the upload button, read the warning and make your choice to move on to the actual uploading.
Each image will be separately uploaded to the WordPress Media Library. Check the progress bar at the bottom to see the progress.
Once all images are uploaded, you'll get a prompt and be taken back to the main form. The markdown file, markdown preview and html preview will all be updated with the URLs to the newly uploaded images.

If the uploading fails for any reason, a prompt will show and the rollback will start. This will attempt to delete all uploaded images so you can try again later. **Make sure you can access the RW WordPress before clicking OK in the prompt, not doing so will require you to do manual cleanup of the uploaded images on WordPress.**

Now copy and paste the html in your post and check if the formatting is correct. There may be some edge cases where things can get weird or incorrect. If that's the case, be sure to make an issue here if there isn't one already.

## .NET Core (all versions)

Follow the instructions in the console. It should be pretty straight forward.
If you're confused about anything, be sure to tell me so I can improve the instructions.

# To-Do & Nice To Have

**To-Do:**

General:

- Fix line ending difference between Windows & Unix based systems. Windows uses \r\n while Unix systems use \n, this can break the conversion. See the known issue about notes.

.NET Core:

- Command line arguments for .NET Core versions to allow for automation & building a native UI around it.
- Add image optimization to .NET Core versions.

**Nice To Have:**

- UI for .NET Core version & remove the WinForms/Mono version.

# Known Issues

**.NET Framework / Mono UI Version**:
- The app will take a few minutes to load the first time you open it on mono platforms (macOS & linux). This is because mono comes without any certificates and needs to download these in orde to access Github (for updates) & raywenderlich.com (for uploading images).
- The preview is not a 1:1 preview with what you would get on the website itself.
- Code in the preview will appear incorrect if the following characters are used: < > &. The converted HTML is correct, this only affects the preview.
- Due to a mono limitation, the applications needs to be run in 32-bit mode on macOS & linux. This should be fixed by Mono somewhere this year (2018) though.

**.NET Core**:
- No command line arguments can be given at this time, so building a native UI around it isn't possible yet. This is in research.
- No updater (yet) for .NET Core, please download new releases manually.

**All versions (shared library bugs)**:
- The spoiler tag isn't supported yet in markdown, you'll need to add it in as HTML for now.
- Notes may be parsed incorrect on macOS & linux.
