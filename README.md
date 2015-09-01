# KaomojiTray
A system tray application to provide quick access to japanese kaomoji

# Progress
Basic functionality is done. Can load into the system tray. Left clicking the icon opens the library where you can click on items to copy them to the clipboard and closes the library.

# Roadmap
- Rebuild the internals to have the application run a simple web server that will offer up a web page that will be the new UI for the application. Changing the UI into a web page should solve the unicode rendering issue I've encountered in Windows 10 and provide plenty of flexibility for easily styling the look of the app
- Have the application provide a simple REST API that the served web page can use to:
	1. Access the library of emoji and render that as the bulk of the pages content.
	2. Access the recently copied emoji and show them at the top of the page
	3. Tell the application which emoji get copied so that a list of recently used emoji can be maintained.
- Make a proper icon. Currently has a placeholder icon for the system tray

# TODO: 
- Extend the library by adding some useful western ones, such as ¯\_(ツ)_/¯