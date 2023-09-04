using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class Tale
{
    private const int WIDTH = 256;
    private const int HEIGHT = 256;

    //The Tale class represents the story and player info
    public string Name;
    public string Screenshot;
    public PositionCoordinates Player;
    public List<StoryboardStep> Storyboard;
    public World TaleWorld;

    public Tale(StoryEngineScript storyEngine, World world, Camera camera) {
        GameObject playerObject = storyEngine.Player;
        Player = new PositionCoordinates(playerObject.transform.position.x, playerObject.transform.position.z, playerObject.transform.position.y);
        Storyboard = storyEngine.Storyboard;
        Debug.Log(Storyboard);
        TaleWorld = world;
        CaptureScreenshot(camera);
    }

    private void CaptureScreenshot(Camera camera) {
        RenderTexture screenTexture = new RenderTexture(WIDTH, HEIGHT, 16);
        camera.targetTexture = screenTexture;
        RenderTexture.active = screenTexture;
        
        camera.Render();

        Texture2D renderedTexture = new Texture2D(WIDTH, HEIGHT);
        renderedTexture.ReadPixels(new Rect(0, 0, WIDTH, HEIGHT), 0, 0);
        renderedTexture.Apply();

        camera.targetTexture = null;
        RenderTexture.active = null;
        
        byte[] byteArray = renderedTexture.EncodeToPNG();

        ReleaseResources(screenTexture, renderedTexture);

        Screenshot = Convert.ToBase64String(byteArray);
        Debug.Log(Screenshot);
    }

    private void ReleaseResources(RenderTexture screenTexture, Texture2D renderedTexture) {
        if(screenTexture != null) {
            screenTexture.Release();
            screenTexture = null;
        }
        if(renderedTexture != null) {
            UnityEngine.Object.Destroy(renderedTexture);
            renderedTexture = null;
        }
    }
}
