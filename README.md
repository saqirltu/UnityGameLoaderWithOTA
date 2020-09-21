# UnityGameLoaderWithOTA
A loader application for deploying and loading mini games as remote content that can be updated over-the-air. Made with Unity Addressables.

A shell app + remote content solution. 

The beneficial use cases are:

During development: We can publish the new content more often. Then our test fleet can always get the latest update and give feedback. Enabler for quick iterations and agility.

Unmaintained Installations, such as Arcade machine: We can push new content and bug fixes remotely. People sometimes bother to update their apps on phone, so this is perfect solution for doing minor updates without reinstallation.

Large games: This allows to selectively load the pieces which are needed into the game at runtime.   

It is using the Unity Addressables to support the remote content management. Up from the Unity Editor and Addressable package versions used in the project, we can even put the actual Scene asset in the Packages folder. 

HowTo:

[Test In Editor]
1. Open the base scene Loader.unity in Hierachy.
2. Hit play, then it will load the GamesMenu.unity scene.
3. Click around the carousel menu to choose a game, then click the LoadGame button.
4. It will load the selected scene. I didn’t add “return to menu” for simplicity. So just restart the procedure to choose another game.


[Test In Build]
1. Open Unity Menu -> Window -> Asset Management -> Addressables -> Group
2. In the newly open Addressables window menu bar, Build -> New Build -> Default Build Script. This will generate a base version of the content.
3. Back to the Unity Menu -> File -> Build Settings. Make sure the scenes to build list only contains Loader.unity. Hit Build and it will create the shell application. 
4. Use a http server application to host the content generated in step #2. e.g. (1) Open terminal and go to the directory *ProjectFolder*/ServerData/ (2) python -m SimpleHTTPServer 80
5. Run the shell application generated in step #3.

[Customise In Build for actual deployment]
1. In the Addressables Groups window, select “Server”. 
2. Find in the Inspector, a section called Content Packing & Loading. Change the build and load path according to your own setup. 
3. Follow the same steps in the Build guideline. Just note to change the path accordingly.
4. You should be able to test it now. Make sure to keep a good network connection :)
