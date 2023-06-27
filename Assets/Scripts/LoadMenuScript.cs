using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadMenuScript : MonoBehaviour
{
    private static Vector3 CAM_POS = new Vector3(0,0,0);
    private static Vector3 START_CHAR = new Vector3(25,135,0);
    private static Vector3 END_CHAR = new Vector3(775,135,0);
    private const float ORTHO_SIZE = 383;
    private const int CAM_Z_OFFSET = -100;
    private const int REFERENCE_WIDTH = 1366;
    public GameObject StartMenu;
    public GameObject LoadingScreen;
    public Image LoadingBar;
    public GameObject Character;
    public GameObject LoadingScreenText;

    [SerializeField]
    private Camera _mainCamera;


    public void loadScene(int sceneId)
    {
        LoadingScreenText.SetActive(false);
        LoadingScreen.GetComponent<CanvasGroup>().blocksRaycasts = true;
        CAM_POS = new Vector3(Screen.width/2, Screen.height/2, CAM_Z_OFFSET);
        _mainCamera.transform.position = CAM_POS;
        _mainCamera.transform.rotation = Quaternion.Euler(0, 0, 0);
        _mainCamera.backgroundColor = Color.black;
        _mainCamera.orthographic = true;
        float scaledSize = CalculateScale();
        _mainCamera.orthographicSize = scaledSize;
        CaculateCharacterScale();
        StartMenu.SetActive(false);
        StartCoroutine(LoadSceneAsync(sceneId));
    }

    IEnumerator LoadSceneAsync(int sceneId)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);
        operation.allowSceneActivation = false;

        LoadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9F);

            LoadingBar.fillAmount = progressValue;

            Vector3 CharacterPos = Vector3.Lerp(START_CHAR, END_CHAR, progressValue);
            Character.transform.position = CharacterPos;


            if (progressValue >= 0.9f)
            {
                LoadingScreenText.SetActive(true);
                Debug.Log(_mainCamera.orthographicSize);
                // Wait for user input to activate the scene
                if (Input.anyKeyDown)
                {
                    operation.allowSceneActivation = true;
                }
                Debug.Log(END_CHAR);
            }

            yield return null;
        }
        Character.transform.position = END_CHAR;
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
