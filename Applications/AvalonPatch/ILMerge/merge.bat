@echo off
IF EXIST AvalonPatch_UNMERGED.exe del AvalonPatch_UNMERGED.exe
ren AvalonPatch.exe AvalonPatch_UNMERGED.exe
ilmerge /out:AvalonPatch.exe AvalonPatch_UNMERGED.exe ICSharpCode.SharpZipLib.dll