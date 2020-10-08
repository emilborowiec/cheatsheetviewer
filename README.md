# QuickSheet

QuickSheet is a WPF application for displaying and easily switching between cheat sheets.

Main features:
- Simple, textual file format for ease of writing your own cheat sheets.
- Automatic font size and layout calibration to optimal use of screen size.
- Operated mostly from keyboard

## Key commands
- Esc - closes popup dialogs or closes the application
- Left and Right arrow keys - switch to previous and next loaded sheet correspondingly
### TODO
- Going to specific sheet with number keys
- Manually increasing/decrasing font size with Up/Down arrow keys
- Dark mode
- Alternate formatting with tables
- Automatic reloading of quick sheet files
- User specified source directory
- Help page or default help cheat sheet

## Quick Sheet file format

Here's the example:

```
# Quick Sheet Cheat Sheet

### Sheet title
First line must contain sheet title
It must begin with # and title goes after single whitespace

### Cheat structure
Cheat is composed of Caption and Entries
Caption starts with ### and whitespace
Entries are lines below caption and before another caption or section

## Section on sections

### Section name
Sections start with ## whitespace and then section name

### Cheats in section
Every cheat after section start belongs to that section
Until next section starts

### Root section
Cheats can be added under sheet title and before first section start
Those cheats are considered to be in group section
Group section is rendered without a name because it doesn't have any
```