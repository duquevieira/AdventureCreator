using UnityEngine;
using UnityEngine.SceneManagement;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using System.Threading.Tasks;


public class InGamePauseMenu : MonoBehaviour
{
    private const string MAIN_MENU_SCENE = "MainMenu";
    private const string LOADING_SCENE = "LoadingScreenScene";
    public CanvasGroup Overlay;
    public CanvasGroup PauseMenu;
    public GameObject MenuObjects;

    private bool _pauseMenuOpen;

    // Start is called before the first frame update
    void Start()
    {
        _pauseMenuOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            if (_pauseMenuOpen)
            {
                ClosePauseMenu();
            }
            else
            {
                OpenPauseMenu();
            }
        }
    }

    public void TogglePauseMenu() {
        if(_pauseMenuOpen) {
            ClosePauseMenu();
        } else {
            OpenPauseMenu();
        }
    }

    public void QuitToMainMenu() {
        if(!AbstractSave.CanQuit) return;
        LoadMenuScript.SceneToLoad = MAIN_MENU_SCENE;
        GameManager.Current.Pause(PauseMethods.NoPauseMenu);
        SceneManager.LoadScene(LOADING_SCENE);
    }

    private void OpenPauseMenu()
    {        
        PauseMenu.blocksRaycasts = true;
        _pauseMenuOpen = true;
        GameManager.Current.Pause(PauseMethods.PauseMenu);
        MenuObjects.SetActive(true);
        StartCoroutine(MMFade.FadeCanvasGroup(PauseMenu, 0.2f, 1f));
        StartCoroutine(MMFade.FadeCanvasGroup(Overlay, 0.2f, 0.85f));
    }

    public void ClosePauseMenu()
    {
        PauseMenu.blocksRaycasts = false;
        _pauseMenuOpen = false;
        GameManager.Current.Pause(PauseMethods.PauseMenu);
        MenuObjects.SetActive(false);
        StartCoroutine(MMFade.FadeCanvasGroup(PauseMenu, 0.2f, 0f));
        StartCoroutine(MMFade.FadeCanvasGroup(Overlay, 0.2f, 0f));
    }
}
