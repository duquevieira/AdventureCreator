using UnityEngine;
using UnityEngine.SceneManagement;

public class Play : MonoBehaviour, MenuObject
{
    public LoadList SelectedLoad;
    private const string PLAY_SCENE = "GameScene";
    private const string LOADING_SCENE = "LoadingScreenScene";
    public void runFunction() {
        LoadMenuScript.SaveId = SelectedLoad.SelectedSave;
        LoadMenuScript.GameId = SelectedLoad.UserSave;
        LoadMenuScript.SceneToLoad = PLAY_SCENE;
        SceneManager.LoadScene(LOADING_SCENE);
    }
}
