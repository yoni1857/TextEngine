# TextEngine

A choose-your-own-adventure text-based story engine for people who don't wanna code.


# File structure
The working directory (usually the directory where "TextEngine.exe" is located) contains a couple of key folders and archives we will go through:
- Config
- res64.pak
- Story
- Update

## Config

This folder houses multiple files that have multiple functionalities that we will go through:
- autosave.s
- colorscheme.xml
- config.xml

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

## res64.pak
This is a compressed version of the folder previously known as "Resource", since it contains files that
can be relatively big, we've decided to make it compressed. We will go over those files and folders each:
- sprites (Folder)
- wav (Folder)
- game.ico (File)
- gametitle (File)
- gamedesc (File)

##### sprites
This folder contains all of the sprites used in the dialogs.
Sprites are stored in transparent png formtat and the maximum size of a sprite must be 150x150.
Sprites are displayed by specfying their file name (including extension) in the 256th (255th index) string in dialog.xml.

##### wav
This folder contains all of the sounds played during dialogs.
It works pretty much the same as the sprites folder but all of the sounds have to be in wav format (I know, I know).
To specify a sound from this folder to be played at the start of a dialog you have to specify it's name (again, including the extension) in the 255th (254th index) string of the dialog's .ydkl file.

#### gametitle
This file stores the game's name.

#### gamedesc
This file stores the game's description in the about box.

###### PLEASE NOTE: 
>As of commit `fb22f384fd88c9875db57b39e89ef90c99c58c47` on the testing branch a new system reserved file has been added to the wav folder. This file is called "selectionchanged.wav" and will play every time the user changes their selection in the reply options menu.

#### game.ico
This file is used as the game's icon.

## Story
This folder stores the story of the game.
This section will explain how stories are structured.

The previous section that was here was obsolete so instead I am going to just add a section about StoryMaker since it is the recommended way of creating stories but isn't required (if you wanna torture yourself of course).

### StoryMaker
StoryMaker is a tool introduced in build **[0.17708.27156](https://github.com/yoni1857/TextEngine/releases/tag/0.1.7708.27156)** (aka The Creator Update).
Which allows users to easily create stories as well as save them for future use before exporting them.

Exporting a story is pretty easy. First make sure you've saved it as a project, then, just hit File>>Export and pick a folder.
After the story exports, you can take the resulting `.ypac` file and put it in your Update folder.

SM has a very neat project management system which allows you to create new projects and name them as you'd like.
The program itself is pretty easy to use so you can try it out yourself!

To close up this section I would also like to mention that dialog options starting with the right arrow (>) character count as actions and not as replies.

## Update

The update folder contains .ypac archives which are extracted at runtime as a way to update files or just automatically place them.
All .ypac archives are extracted locally.
You can create .ypac archives using the Package Creator Tool.



# Big thanks

Big thanks to my friend [ofsho](https://github.com/ofsho) who helped with this.
