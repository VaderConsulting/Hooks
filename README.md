# Hooks

VS 2008 VB.NET WinForms (.NET 3.5) working copy that on Form1 load constructs LowLevelMouseHook, which SetWindowsHookEx-installs a WH_MOUSE_LL hook and Debug.Prints left-button downs. LowLevelKBHook can install a WH_KEYBOARD_LL hook that returns 1 for Ctrl+Esc, Alt+Tab, and Alt+Esc; Form1 never constructs it. Timer1 (250 ms) is meant to dump cursor position, WindowFromPoint handle, root caption, and child class names, but Handles Timer1.Tick is commented out so it does not run. Open `Hooks.sln`. This is a historical working copy from Dave Robinson / VaderConsulting.

**Source last updated:** 2010-02-04  
**Language:** VB.NET  
**Target:** v3.5  
**Output:** WinExe

## What it is

VS 2008 VB.NET WinForms (.NET 3.5) working copy that on Form1 load constructs LowLevelMouseHook, which SetWindowsHookEx-installs a WH_MOUSE_LL hook and Debug.Prints left-button downs. LowLevelKBHook can install a WH_KEYBOARD_LL hook that returns 1 for Ctrl+Esc, Alt+Tab, and Alt+Esc; Form1 never constructs it. Timer1 (250 ms) is meant to dump cursor position, WindowFromPoint handle, root caption, and child class names, but Handles Timer1.Tick is commented out so it does not run. Open `Hooks.sln`. This is a historical working copy from Dave Robinson / VaderConsulting.

## Solution structure

| Project | Language | Path |
|---------|----------|------|
| `Hooks` | VB.NET | `Hooks/Hooks.vbproj` |

## How to open

Open `Hooks.sln` in Visual Studio.

## Attribution and provenance

- **Assembly company:** Microsoft
- **Assembly copyright:** Copyright © Microsoft 2010

## License

MIT. See `LICENSE`.
