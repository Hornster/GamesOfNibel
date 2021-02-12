//Document's solely purpose is to gather ideas that can be used later on.

//TODO: Map loading
//Create set of scripts that make Unity a nice map edit tool.
//The placement of spawn points, bases and other valuable for gameplay objects can be
//marked with special markers - objects that contain dummy script which is searched for later on.
//In the end, for example purple base will have its position set to the point of the marker.
//
//Any controllers and factories should be added during runtime, taking into account the type of the map.
//There would be a don'tDestroyOnLoad GO that would connect an event handler for OnSceneLoad event. The GO would
//be present in the menu already, so upon change of scenes - the event would trigger. This object would have
//refs to required prefabs and simply instantiate them upon match startup.

//Addressables:
//https://docs.unity3d.com/Packages/com.unity.addressables@1.16/manual/LoadContentCatalogAsync.html
//https://forum.unity.com/threads/modding-with-addressables.539926/

//mod tool:
//https://github.com/Hello-Meow/ModTool

//Structure:
//https://blog.theknightsofunity.com/7-ways-keep-unity-project-organized/

//Adding gameobjects to scene from script: https://medium.com/@gonzalez.martin90/unity-tips-scriptable-objects-3e0e19349c05

//Cinemachine multiple cameras:
//Add 2 regular cameras, add cinemachine brains to them.
//Add 2 Virtual Cameras from Cinemachine
//Set the regular camera gameobject and virtual camera gameobject to playerCamera1 layer (or whatever is it called)
//Do it respectively for cameras for secondplayer - assign to layer playerCamera2.
//Make sure that layer playerCamera1 has turned off layer playerCamera1 in the culling mask and vice-versa.
//This ought make cinemachine work with two separate cameras.

