using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class Tale
{
    //The Tale class represents the story and player info
    public string Name;
    public string Screenshot;
    public PositionCoordinates Player;
    public List<StoryboardStep> Storyboard;
    public World TaleWorld;

    public Tale(StoryEngineScript storyEngine, World world) {
        GameObject playerObject = storyEngine.Player;
        Player = new PositionCoordinates(playerObject.transform.position.x, playerObject.transform.position.z);
        Storyboard = storyEngine.Storyboard;
        TaleWorld = world;
        Texture2D screenshotTexture = ScreenCapture.CaptureScreenshotAsTexture();
        var bytes = screenshotTexture.EncodeToPNG();
        Screenshot = Convert.ToBase64String(bytes);
    }
}
