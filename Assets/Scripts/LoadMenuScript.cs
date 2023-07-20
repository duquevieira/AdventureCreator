using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadMenuScript : MonoBehaviour
{
    private static Vector3 CAM_POS = new Vector3(0,0,0);
    private static Vector3 START_CHAR;
    private static Vector3 END_CHAR;
    private const float ORTHO_SIZE = 383;
    private const int CAM_Z_OFFSET = -100;
    private const int REFERENCE_WIDTH = 1366;
    private const string CREATE_SCENE = "CreateScene";
    public Image LoadingBar;
    public GameObject Character;
    public GameObject LoadingScreenText;

    [HideInInspector]
    public static string SaveId;
    [HideInInspector]
    public static string GameId;
    [HideInInspector]
    public static string SceneToLoad;
    [SerializeField]
    private Camera _mainCamera;

    void Start() {
        START_CHAR = new Vector3(25,135,0);
        END_CHAR = new Vector3(775,135,0);
        loadScene(SceneToLoad);
    }


    public void loadScene(string scene)
    {
        LoadingScreenText.SetActive(false);
        CAM_POS = new Vector3(Screen.width/2, Screen.height/2, CAM_Z_OFFSET);
        _mainCamera.transform.position = CAM_POS;
        _mainCamera.transform.rotation = Quaternion.Euler(0, 0, 0);
        _mainCamera.backgroundColor = Color.black;
        _mainCamera.orthographic = true;
        float scaledSize = CalculateScale();
        LoadingBar.fillAmount = 0;
        _mainCamera.orthographicSize = scaledSize;
        CaculateCharacterScale();
        StartCoroutine(LoadSceneAsync(scene));
    }

    IEnumerator LoadSceneAsync(string scene)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);
        operation.allowSceneActivation = false;
        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9F);

            LoadingBar.fillAmount = progressValue;

            Vector3 CharacterPos = Vector3.Lerp(START_CHAR, END_CHAR, progressValue);
            Debug.Log("Start: " + START_CHAR + " End: " + END_CHAR + " Progress: " + progressValue);
            Debug.Log(CharacterPos);
            Character.transform.position = CharacterPos;

            if (progressValue >= 0.9f)
            {
                LoadingScreenText.SetActive(true);
                // Wait for user input to activate the scene
                if (Input.anyKeyDown)
                {
                    operation.allowSceneActivation = true;
                    if(scene.Equals(CREATE_SCENE)) loadCreatorScene();
                    else loadPlayScene();
                }
            }

            yield return null;
        }
        Character.transform.position = END_CHAR;
        yield return null;
    }

    private void loadCreatorScene() {
        SaveLevel.SaveId = SaveId;
    }

    private void loadPlayScene() {
        SaveGame.SaveId = SaveId;
        SaveGame.GameId = GameId;
    }

    private float CalculateScale() {
        float scaleFactor = (float) Screen.width/REFERENCE_WIDTH;
        return ORTHO_SIZE*scaleFactor;
    }

    private void CaculateCharacterScale() {
        float currentWidthRatio = (float) Screen.width/800;
        float currentHeightRatio = (float) Screen.height/450;
        START_CHAR = new Vector3(START_CHAR.x*currentWidthRatio, START_CHAR.y*currentHeightRatio, 0);
        END_CHAR = new Vector3(END_CHAR.x*currentWidthRatio, END_CHAR.y*currentHeightRatio, 0);
    }
}
