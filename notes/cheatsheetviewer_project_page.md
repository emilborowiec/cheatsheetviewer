# CheatSheetViewer

#### *A lightweight cheat sheet viewer for Windows Desktop.* 

[toc]

### About CheatSheetViewer

Cheat sheets are great in situations where you might need a quick reference. For example, for keyboard shortcuts or command line options in programs you sometimes use. 

Having your cheat sheets strewn across browser tabs is, however, not ideal. Static cheat sheet documents don't adapt to real estate of your screen, forcing you to either squint or scroll to read them. You also cannot edit them easily to add entries that you miss or remove those you don't need.

CheatSheetViewer addresses those problems and adds few extra features on top.

CheatSheetViewer reads .csmup files to load cheat sheets of the user. These are plain-text files with a very simple markup allowing you to modify or write your own cheat sheets easily. The CheatSheetMarkup language is somewhat inspired by markdown but is very, very simple and much less powerful.

The app will dynamically adapt to your screen or window size and try to adjust layout and font size for best results. 

You will have all your cheat sheets loaded automatically in a single application, which also allows to switch between them much easier than flipping browser tabs.

See below for full list of features.

### Getting started

To give CheatSheetViewer a go, start by downloading the installer from releases.

The installation will create a few samples of CheatSheetMarkup files for you. You might find them useful as they are, modify them, use as markup reference or just delete them. 

The app features built-in reference for available commands and key mappings. You can also find this information here on the page. Together with description of features, markup and more downloadable samples.

### Using CheatSheetViewer

The application will automatically load all files with .csmup extension from "My CheatSheets" directory inside of your users documents folder. You will be notified if some files could not be read.

Things you can do in the application, you do with the keyboard. Here are all the commands:

- **F1** - Online help. Opens browser tab and navigates to the project page.
- **F2** - Toggle keymap panel. Expands side panel with key commands.
- **F3** - Toggle index panel. Expands side panel with numbers of loaded cheat sheets.
- **Left / Right Arrows** - Switch between opened cheat sheets
- **1-9** - Switch to cheat sheet corresponding to given number
- **R** - reload quicksheet files from "My CheatSheets" directory
- **Up / Down Arrows** - Increase / decrease font size on current cheat sheet
- **L** - Toggle font size lock
- **M** - toggle dark mode
- **Esc** - Close dialog or exit application

### Writing your own cheat sheets

The installer will create "My CheatSheets" directory for you and put some sample quicksheet files there. In order to create your own cheat sheet just create a text file with .qsheet extension in that folder. The markup is very simple, and here's a simplest example to get you started:

```
# Title

## First section
### Cheat A
First entry
Second entry
### Cheat B
Some content here as well

## Second section
### Cheat C
And entry for it
```

Consult reference below if get any errors, but error message should also give you a hint of what might be wrong in your quicksheet files.

### CheatSheetMarkup reference

#### Elements of CheatSheetMarkup document

**Title**

Title of the sheet displayed on top of the window.

**Cheat**

Cheats are the first-class citizens of a cheat sheet. A cheat communicates problem and possible solutions. It has a caption and one or more entry.

In keyboard shortcut cheat sheets, a caption would be a command. Entries would be possible keyboard shortcuts to execute that command.

In recipe style cheat sheets, a caption would describe a goal, and entries would be possible ways of how to achieve it.

**Entry**

Single line solution of a cheat.

**Section**

Cheats are grouped in sections. Sections are displayed as separated cards. Sections are named, but there can also be one anonymous section without a name. Cheats that belong directly to the sheet are grouped together in an anonymous section.

#### Markup rules

- Line starting with '# ' is **Title** **Line**. 
- Line starting with '## ' is **Section Start Line**. 
- Line starting with '### ' is **Cheat Start Line**.
- Empty lines are ignored.
- Other lines are treated as **Cheat Entries**. 
- File must start with Title Line.
- File cannot have more than one Title Lines.
- Cheat Entry must be preceded by Cheat Start or another Entry.
- Cheat Start must be followed by at least one Entry.
- Section Start must be followed by at least one Cheat Start.





