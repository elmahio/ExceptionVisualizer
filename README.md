# <img src="src/ExceptionVisualizer/icon.png" style="height: 1em; width: 1em;"> Exception Visualizer [![Build](https://github.com/elmahio/ExceptionVisualizer/actions/workflows/dotnet.yml/badge.svg)](https://github.com/elmahio/ExceptionVisualizer/actions) [![Visual Studio Marketplace Downloads](https://img.shields.io/visual-studio-marketplace/d/elmahio.exceptioninspector)](https://marketplace.visualstudio.com/items?itemName=elmahio.exceptioninspector)

Exception Debug Visualizer for Visual Studio.

When debugging applications and an exception is thrown you can view the exception details in a more structured and visually pleasing form:

![Exception Visualizer](screenshot.png)

The popup offers a range of features like parsing of HResults:

![Show parsed HResult](screenshot2.png)

Aggreate and inner exceptions are nested in the left side of the screen:

![Nested exceptions](screenshot3.png)

The Exception Visualizer can be opened from the *Locals* or *QuickView* windows by clicking *View* next to the exception:

![View Exception Visualizer](https://github.com/elmahio/ExceptionVisualizer/assets/563206/8dd68fc6-85f2-416d-a5b2-8f78de57fdbf)

## Updating

I'm suspecting there's a bug in Visual Studio when trying to update extensions based on the new extensions system. The bug will cause the Visual Studio Updater to report that there are no available updates when trying to update to a new Exception Visualizer version.

To correctly update this extension from a previous version, you can follow these steps:

1. Launch Visual Studio Installer.
2. Click 'Modify' on your Visual Studio version.
3. Go to the Individual components tab.
4. Search for 'exception'.
5. Uncheck the 'Exception Visualizer' (might have a different name if you have an older version).
6. Click 'Modify'
7. Then install the extension from scratch.

---

Sponsored by [elmah.io](https://elmah.io).
