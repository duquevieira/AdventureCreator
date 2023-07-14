using System;
using System.Collections.Generic;

[Serializable]
public class Game
{
    public string LevelId;
    public PositionCoordinates Player;
    public List<StoryboardStep> Storyboard;
    public List<ItemGroup> StoryItems;
    public List<ItemGroup> InventoryItems;

    public Game(string level, StoryEngineScript story) {
        LevelId = level;
        Player = new PositionCoordinates(story.Player.transform.position.x, story.Player.transform.position.z);
        Storyboard = story.Storyboard;
        StoryItems = story.StoryItems;
        InventoryItems = story.InventoryItems;
    }
}
