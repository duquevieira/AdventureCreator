using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadMenuScript : MonoBehaviour
{
    private static Vector3 CAM_POS = new Vector3(605.5f,243.5f,-20);
    private static Vector3 START_CHAR = new Vector3(107.400002f,94.8999939f,0);
    private static Vector3 END_CHAR = new Vector3(1103.5f,94.8999939f,0);
    public GameObject LoadScreen;
    public GameObject LoadingScreen;
    public Image loadingBar;
    public GameObject Character;
    public GameObject LoadingScreenText;

    [SerializeField]
    private Camera _mainCamera;


    public void loadScene(int sceneId)
    {
        LoadingScreenText.SetActive(false);
        _mainCamera.transform.position = CAM_POS;
        _mainCamera.transform.rotation = Quaternion.Euler(0, 0, 0);
        _mainCamera.backgroundColor = Color.black;
        _mainCamera.orthographic = true;
        _mainCamera.orthographicSize = 243.5f;
        LoadScreen.SetActive(false);
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

            loadingBar.fillAmount = progressValue;

            Vector3 CharacterPos = Vector3.Lerp(START_CHAR, END_CHAR, progressValue);
            Character.transform.position = CharacterPos;


            if (progressValue >= 0.9f)
            {
                LoadingScreenText.SetActive(true);
                // Wait for user input to activate the scene
                if (Input.anyKeyDown)
                {
                    operation.allowSceneActivation = true;
                }
            }

            yield return null;
        }
        Character.transform.position = END_CHAR;
    }
}
