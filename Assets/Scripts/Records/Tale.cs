using UnityEngine;

public class Tale : MonoBehaviour
{
    //The Tale class represents the story and player info
    public PositionCoordinates Player;
    public Storyboard Storyboard;
    public World TaleWorld;

    public Tale(StoryEngineScript storyEngine, World world) {
        GameObject playerObject = storyEngine.Player;
        Player = new PositionCoordinates(playerObject.transform.position.x, playerObject.transform.position.z);
        Storyboard = storyEngine.Storyboard;
        TaleWorld = world;
    }
}
