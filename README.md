# TextEngine

A choose-your-own-adventure text-based story engine for people who don't wanna code.


# File structure
The working directory (usually the directory where "TextEngine.exe" is located) contains a couple of folder we will go through:
- Config
- Resource
- Story
- Update

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
|String 005 | Player reply color           |
|String 006 | Story text color             |

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



# Big thanks

Big thanks to my friend [ofsho](https://github.com/ofsho) who helped with this.
