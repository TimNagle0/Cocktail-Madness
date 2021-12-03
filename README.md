# Cocktail-Madness
Stress inducing game about serving cocktails

## Installation
In order to run the game in the browser in combination with the shaker you need to set up your unity project and esp8266 with the accelerometer sketch on it.

### Unity

- Download the Unity editor (v 2020.3.20f1) and make sure that you install the WebGL platform support
- Download the repository and open the project in the Unity editor. 
- Go to File > Build Settings and make sure that the platform is set to WebGL
- Click on Build and Run and select the Cocktail Madness 01 folder inside the Builds folder
- Wait for the game to build and open up in your webbrowser (should happen automatically)
- Go to your Builds folder in your file explorer and open up the "Firebase template.html" file in a text editor, do the same for the "index.html" file in the "Cocktail Madness 01" folder.
- Replace all the text within the index file with the text in the firebase template file and save it.
- Reload the page with the unity game and it should now receive and process the data from the Firebase realtime database

### ESP8266

- Download the arduino Editor and follow this tutorial on how to add the ESP board to your editor https://randomnerdtutorials.com/how-to-install-esp8266-board-arduino-ide/
- Add the right WIFI credentials to the code
- Upload the code to the ESP and it should connect to the database automatically.

