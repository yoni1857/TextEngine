# TextEngine

A choose-your-own-adventure text-based story engine for people who don't wanna code.


# File structure
The working directory (usually the directory where "TextEngine.exe" is located) contains a couple of folder we will go through:
- [Config](#conifg)
- [Resource](#resource)
- [Story](#story)
- [Update](#update)

## Config

This folder houses multiple files that have multiple functionalities that we will go through:
- autosave.s
- colorscheme.xml
- config.xml
- game.cfg
- gamedesc.cfg

#### autosave.s
This file saves the user's progress in a story. It is recommended to delete this file when publishing your story since
the file saves the full path to the last visited dialog.

#### colorscheme.xml
This file defines the colorscheme for the viewer using the 256 string YDK Data format.
It is recommended to edit this file in the bundled YDK Data Editor instead of using a normal text editor.
Please note that the colors specified are system colors that are 
referenced by name and not rgb/hex code (Red, Blue, Green, White, etc..).
Here is the order of the elements that are colored:
| ID        | Element                      |
|-----------|------------------------------|
|String 000 | Top menu strip               |
|String 001 | Textbox config menu strip    |
|String 002 | Dialog picture background    |
|String 003 | Textbox background color     |
|String 004 | Dialog options box background|

#### config.xml
This file stores all of the viewer's settings so no need to alter this one.

#### game.cfg
This file stores the game's name.

#### gamedesc.cfg
This file stores the game's description in the about box.

## Resource
This folder contains multiple files and folders that we will go over each:
- sprites
- wav
- game.ico

##### sprites
This folder contains all of the sprites used in the dialogs.
Sprites are stored in transparent png formtat and the maximum size of a sprite must be 150x150.
Sprites are displayed by specfying their file name (including extension) in the 256th (255th index) string in dialog.xml.

##### wav
This folder contains all of the sounds played during dialogs.
It works pretty much the same as the sprites folder but all of the sounds have to be in wav format (I know, I know).
To specify a sound from this folder to be played at the start of a dialog you have to specify it's name (again, including the extension) in the 255th (254th index) string of the dialog's xml file.

###### PLEASE NOTE: 
>As of commit `fb22f384fd88c9875db57b39e89ef90c99c58c47` on the testing branch a new system reserved file has been added to the wav folder. This file is called "selectionchanged.wav" and will play every time the user changes their selection in the reply options menu.

#### game.ico
This file is used as the game's icon.

## Story
This folder stores the story of the game.
This section will explain how stories are structured.

A story is a series of dialogs. Each dialog contains other dialogs. (like a russian doll)
For example, let's say we have a dialog in which the main character needs to choose which balloon to pop:
##### dialog.xml:
| INDEX       | VALUE                                                                  |
|-------------|------------------------------------------------------------------------|
|INDEX 0      | Sir, you have to choose which balloon to pop! The world depends on it! |
|INDEX 1      | > Pop the Green Balloon                                                |
|INDEX 2      | > Pop the Blue Balloon                                                 |
|...INDEX 254 | dialog_ballonchoice.wav                                                |
|INDEX 255    | character_sweating.png                                                 |

##### dialog.xml root folder:
|NAME       |SIZE  |
|-----------|------|
|1          | ~    |
|2          | ~    |
|dialog.xml | 2KB  |

As you can see our dialog contained a few values which were the dialog's text (Index 0), the dialog's reply options 
(Indexes 1 and 2 (Even though dialog reply options go from index 1 to index 149) ), the dialog's sound and the dialog's sprite.

To make a dialog reply lead to another dialog all you have to do is create a folder with the name of the reply's index inside the same folder where your dialog's xml file is located and create another dialog inside of your newly created folder.

To close up this section I would also like to mention that dialog options starting with the right arrow (>) character count as actions and not as replies.

## Update

The update folder contains .ypac archives which are extracted at runtime as a way to update files or just automatically place them.
All .ypac archives are extracted locally.
You can create .ypac archives using the Package Creator Tool.

# YDK Data Editor
this windows application is used to edit the dialog.xml files for the game in TextEngine.
Here is a simple explanation of the controls.
- Start
- Open
- Save
- List
- TextEditor
- Applying Options
- Saving as .ypac packages
- Sample Projects

### Start
Start by making a new folder for your dialog.xml file, then make the dialog.xml
file. After that open VisualStudio and choose as the Startup Project: YDK Data Editor.
Run the YDK Data Editor and read the rest of the short documentation:

### Open
The OPEN control is used to open a dialog.xml file with YDK Data Editor.
This is the only way to edit files (as for now).

### Save
With the save button (not ctrl+s) you save the edits you make for the file
that you had opened.

### List
When you open the application you see a long list from 0 to 255. This list
is the options list. The story data is in List number 0. To make choice options
you must edit number 1 - 255.

Once you DoubleClick on a number it will change the TextEditor data.

### TextEditor
In the TextEditor you edit the story text, and the options. If you edit an
option make sure you put `>` at the start.

### Applying Options
To apply option changes in the dialog.xml folder you must make a new folder inside
it. Make a folder per Option (Option 0 does not count as an option). Option folders
must have the option number in it. For a folder of option 100 the folder name will
be 100. For a folder of option 4 the folder name will be 4. All option folders must be
in the directory of the parent dialog.xml (the dialog.xml that triggers the option).
In the option folders make a new empty dialog.xml file. It is ok to have only 1 option
or none. No options means: Game Over and 1 means that the player has no choice but to
continue.

### Saving as .ypac Packages
To save your story as a package open visual studio, and change the Startup Project to
PackageCreatorTool, and answer the questions like this: (_ = your input)

```
Author: _your_name_
Extraction Path: _folder_name_
Package Name: _package_name_
Package Version: _package_version_
Package Files Folder: _dialog.xml_first_folder_
```

After this in the: PackageCreationTool/Bin/Debug folder you will find your
package (name:  packageName_authorName.ypac). To use your package, copy it
and change the file name to .z to unzip it. After unziping in the folder you
get check for your dialog.xml file and for your option folders (very easy to find).
You will see a package.xml file, just dont touch it. You are free to open your dialog.xml
file with TextEngine, but if you want to publish your package, make a GitHub repository
with the ypac file and the zip file. To include your package in the official
TextEngine repo, put it in the ypac Packages (the .ypac file) and commit (make sure to delete
the folder and the zip file in the PackageCreatorTool folder).

### Sample Projects
There are 2 sample projects included, first one of them is Yoni's `Yoni Meller_Story.ypac` file
found in the ypacPackages folder. The second one is not a story but a sample project which is
Ofek's `SampleProject_OfekBenDavid.ypac`. To use those files, copy them (and paste) change the file
extension to .z and unzip them. To edit them open the YDK Data Editor with Visual Studio or Executable and
open the dialog.xml. To copy their files copy the files to your folder. Delete the zip files and the folders
after being created. Do Not Edit the SamplePackages and commit, if you edit the sample files DONT COMMIT!

# For Package Editors
```
DO NOT EDIT OTHERS PACKAGES AND COMMIT. YOU ARE ALLOWED TO EDIT, COPY, SELL AND REDISTRIBUTE THE
STORIES, BUT DO NOT EDIT OTHERS UNLESS THEY ARE IN THE OFFICIAL REPOSITORY.

IF A PACKAGE IS IN THE TEXTENGINE REPOSITORY IT IS LICENSED UNDER THE MIT LICENSE WITH SOME MODIFICATIONS.
PACKAGES CAN BE EDITED, RESELLED AND COPIED.

ALL PACKAGES ARE COPYRIGHT TO CREATORS. IN YOUR PACKAGE INCLUDE A LICENSE FILE, IF YOU DO NOT IS CAN NOT
BE EDITED AND COMMITED AND IS USING THIS LICENSE.

IF YOU EDIT A PACKAGE IN THE TEXTENGINE REPOSITORY SO DO NOT COMMIT YOUR CHANGES.

DO NOT EDIT OTHERS PACKAGES BEFORE READING THE LICENSE.

IF A PACKAGE IS INSTALLED FROM ANOTHER REPOSITORY YOU MUST READ THE LICENSE AND THE README.

FEEL FREE TO MAKE MANAGE AND EDIT REPOSITORIES, BUT DO NOT COMMIT OTHERS STORIES TO THIS REPOSITORY.
TO ADD A PACKAGE TO THIS REPOSITORY MAKE AN ISSUE AND YONI1857 OR OFSHO WILL ALLOW IT. DONT FORGET TO
LINK YOUR .YPAC FILE TO THE ISSUE.

FEEL FREE TO ASK ISSUES ABOUT PACKAGES ADDING.
```

# Big thanks

Big thanks to my friend [ofsho](https://github.com/ofsho) who helped with this.
