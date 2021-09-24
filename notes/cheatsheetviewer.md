# QuickSheet

Technical documentation and project notes.

[toc]

## About the project

### Mission

I want a desktop application that will use full size of the screen to display cheat sheets for me. I want those cheat sheets to be easily flippable with arrow keys.

I also want to edit those cheat sheets with simple text format. The layout should happen automatically, and the application should  apply style and let me configure it a bit.

### Glossary and technical  terms

Quick Sheet File

Text file with .qsheet file extension. The contents  of the file should follow Cheat Sheet File Format.

#### Sheet Title

The title of the cheat sheet, probably displayed  somewhere on the top.

#### Sheet Section

Cheat Sheets can be divided into sections that group  together related cheats.

#### Cheat

A cheat is an entry in the Cheat Sheet File. A cheat  typically explains a concept or shows how to carry out specific  action.

#### Cheat Problem

A cheat problem is the short text explaining what  problem the cheat solves or what concept it explains.

#### Cheat Solution

A cheat can contain one or more solutions. Solution  is the explanation text or code sample. 

#### Cheat Sheet File Format

```
# Title of the cheat sheet

### Cheat in anonymous section
First entry
Second entry

## Named section

### Cheat in named section
Entry in the cheat
```

### Layout constraints

- Font size in all sections is the same
- CheatSheet tries to fit the content using  largest font possible.
- Cheat entries go in single column
- Cheat entry and caption lines are never  broken
- Single column sections
- Layout tries to arrange sections so that aspect ratio of  final result is similar to aspect ratio of the container panel.



## Product Backlog

| Story                                                        | Status                                   |
| ------------------------------------------------------------ | ---------------------------------------- |
| As a User I want to automatically  load all cheatsheet files from the default directory so that I don’t need to  take any action besides starting the application. | <span style="color:green">Done</span>    |
| As a User I want to see the cheat  sheet title on the top so that I can see which one I’m looking at. | <span style="color:green">Done</span>    |
| As a User I want to see all the  cheats and sections on the current CheatSheet so that the whole app makes some  sense at all. | <span style="color:green">Done</span>    |
| As a User I want cheats and  sections laid out and scaled on the screen so that I don’t need to scroll  anything. | <span style="color:green">Done</span>    |
| As a User I want the cheat sheet  to be formatted for readability so that I can easily see what’s  what. | <span style="color:green">Done</span>    |
| As a User I want font size to  adjust to window size and amount of content so that the sheet uses all available  space and tries to fit on one page. | <span style="color:green">Done</span>    |
| As a User I want to easily switch  between loaded cheat sheets with the keyboard so that I don’t have to move my  hands to the mouse. | <span style="color:green">Done</span>    |
| As a User I want the program to  have a color scheme that does not hurt when I look at it. | <span style="color:green">Done</span>    |
| As a User I want to be notified if  some quick sheet files could not be loaded, but without the application  crashing. | <span style="color:green">Done</span>    |
| As a User I want to exit the  application with an escape key so that I don’t need to use the  mouse. | <span style="color:green">Done</span>    |
| As a User I want the application  to have a recognizable icon so that I can find it easily when it’s  minimized. | <span style="color:green">Done</span>    |
| As a User I want to hide the  window title bar to have more space for chearsheets. | <span style="color:green">Done</span>    |
| Separate repository for the  project.                        | <span style="color:green">Done</span>    |
| Few of my own complete cheat  sheets.                        | <span style="color:green">Done</span>    |
| As a User I want the header to be  fixed size and depending only on screen size and not amount of content, so that  it feels nicer when I switch sheets. | <span style="color:green">Done</span>    |
| As a GitHub user I want a readme  that describes what this project is about and what are the main features of  it. | <span style="color:green">Done</span>    |
| As a User I want to manually  increase or decrease font size so that I can adjust it if automatic calculation  screws up. | <span style="color:green">Done</span>    |
| As a User I want to reload  quicksheets without closing the application so that I can make changes to my  sheets and see them quickly. | <span style="color:green">Done</span>    |
| As a User I want dark mode so that  the screen doesn’t glare at me when I work in the evening. | <span style="color:green">Done</span>    |
| As a User I want a web page where  I can learn about QuickSheet and qsheet file format. | <span style="color:green">Done</span>    |
| Bug. Fonts don’t seem to be  updating after changing a sheet. | <span style="color:green">Done</span>    |
| As a User I want QuickSheet to  remember manually adjusted font size for given sheet, so I don’t have to adjust  it every time if nothing else changes. | <span style="color:green">Done</span>    |
| As a User I want to download  sample quicksheets from the program page so that I can get some value without  authoring it first. | <span style="color:green">Done</span>    |
| As a User I want QuickSheet to  install with sample sheets in the default directory so that I am certain about  its location and file format. | <span style="color:green">Done</span>    |
| As a User I want to have a popup  menu of loaded cheat sheets so that I can find one I need without stepping to  each one loaded. | <span style="color:green">Done</span>    |
| Project documentation explaining  the file format and the UI. | <span style="color:green">Done</span>    |
| Redistributable installers and a  download page.             | <span style="color:green">Done</span>    |
| As a User I want to download an  installation file of the latest version of the program from the project  page. | <span style="color:green">Done</span>    |
| Nicer styling.                                               | <span style="color:green">Done</span>    |
| Standard windows  dialogs                                    | <span style="color:green">Done</span>    |
| Nice icon                                                    | <span style="color:green">Done</span>    |
| Next release                                                 | <span style="color:green">Done</span>    |
| Documentation update                                         | <span style="color:green">Done</span>    |
| Open folder in explorer on sheet  load failures              | <span style="color:black">Pending</span> |
| Open sheet in  notepad                                       | <span style="color:black">Pending</span> |
| Open failed to load sheet in  notepad                        | <span style="color:black">Pending</span> |
| As a User I want to be able to  switch between default view and table view so that I can choose the best layout  for each of my sheets. | <span style="color:black">Pending</span> |

