using UnityEngine;
using UnityEngine.SceneManagement;

public class Create : MonoBehaviour, MenuObject
{
    public LoadList SelectedLoad;
    private const string CREATE_SCENE = "CreateScene";
    private const string LOADING_SCENE = "LoadingScreenScene";

    public void runFunction() {
        LoadMenuScript.SaveId = SelectedLoad.SelectedSave;
        LoadMenuScript.SceneToLoad = CREATE_SCENE;
        SceneManager.LoadScene(LOADING_SCENE);
    }
}
