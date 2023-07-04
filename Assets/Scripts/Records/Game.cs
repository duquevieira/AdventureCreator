using System;
using System.Collections.Generic;

[Serializable]
public class Game
{
    public PositionCoordinates Player;
    public List<StoryboardStep> Storyboard;
    //Placeholder have to check how inventory is saved
    public List<string> Inventory;

    public Game(StoryEngineScript story) {
        Player = new PositionCoordinates(story.Player.transform.position.x, story.Player.transform.position.z);
        Storyboard = story.Storyboard;
        Inventory = new List<string>();
    }
}
