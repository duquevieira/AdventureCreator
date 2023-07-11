using UnityEngine;
using UnityEngine.SceneManagement;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;


public class InGamePauseMenu : MonoBehaviour
{
    public CanvasGroup Overlay;
    public CanvasGroup PauseMenu;

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

    public void QuitToMainMenu() {
        SceneManager.LoadScene(0);
        GameManager.Current.Pause(PauseMethods.PauseMenu);
    }

    private void OpenPauseMenu()
    {
        PauseMenu.blocksRaycasts = true;

        _pauseMenuOpen = true;
        GameManager.Current.Pause(PauseMethods.PauseMenu);

        StartCoroutine(MMFade.FadeCanvasGroup(PauseMenu, 0.2f, 1f));
        StartCoroutine(MMFade.FadeCanvasGroup(Overlay, 0.2f, 0.85f));
    }

    public void ClosePauseMenu()
    {
        PauseMenu.blocksRaycasts = false;

        _pauseMenuOpen = false;
        GameManager.Current.Pause(PauseMethods.PauseMenu);

        StartCoroutine(MMFade.FadeCanvasGroup(PauseMenu, 0.2f, 0f));
        StartCoroutine(MMFade.FadeCanvasGroup(Overlay, 0.2f, 0f));
    }
}
