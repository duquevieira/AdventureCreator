using UnityEngine;
using UnityEngine.SceneManagement;
using MoreMountains.InventoryEngine;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;


public class InGamePauseMenu : MonoBehaviour
{
    public InventoryInputManager InventoryScript;
    public CanvasGroup Overlay;
    public CanvasGroup TargetInventoryContainer;

    [SerializeField]
    private GameObject _character;
    [SerializeField]
    private GameObject _dunderMifflin;
    [SerializeField]
    private Inventory _items;
    private PlayerHandlerScript _playerScript;

    private bool _pauseMenuOpen;

    // Start is called before the first frame update
    void Start()
    {
        _pauseMenuOpen = false;
        _playerScript = _character.GetComponent<PlayerHandlerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            if (!InventoryScript.InventoryIsOpen)
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
    }

    public void SaveGame() {
        TalesLevel save = new TalesLevel();
        save.CharacterPosX = _character.transform.position.x;
        save.CharacterPosY = _character.transform.position.y;
        save.CharacterPosZ = _character.transform.position.z;

        save.TargetPosX = _playerScript.Target.x;
        save.TargetPosY = _playerScript.Target.y;
        save.TargetPosZ = _playerScript.Target.z;

        save.CharacterRotX = _character.transform.rotation.x;
        save.CharacterRotY = _character.transform.rotation.y;
        save.CharacterRotZ = _character.transform.rotation.z;

        _items.SaveInventory();

        MMSaveLoadManager.Save(save, SceneManager.GetActiveScene().name, "Saves");
    }

    public void LoadGame() {
        TalesLevel load = (TalesLevel) MMSaveLoadManager.Load(typeof(TalesLevel), SceneManager.GetActiveScene().name, "Saves");
        _items.LoadSavedInventory();

        _character.transform.position = new Vector3(load.CharacterPosX, load.CharacterPosY, load.CharacterPosZ);
        _character.transform.rotation = Quaternion.Euler(load.CharacterPosX, load.CharacterPosY, load.CharacterPosZ);
        _playerScript.Target = new Vector3(load.TargetPosX, load.TargetPosY, load.TargetPosZ);
    }

    public void QuitToMainMenu() {
        SceneManager.LoadScene(0);
    }

    private void OpenPauseMenu()
    {
        TargetInventoryContainer.blocksRaycasts = true;

        _pauseMenuOpen = true;
        GameManager.Current.Pause(PauseMethods.PauseMenu);

        StartCoroutine(MMFade.FadeCanvasGroup(TargetInventoryContainer, 0.2f, 1f));
        StartCoroutine(MMFade.FadeCanvasGroup(Overlay, 0.2f, 0.85f));
    }

    private void ClosePauseMenu()
    {
        TargetInventoryContainer.blocksRaycasts = false;

        _pauseMenuOpen = false;
        GameManager.Current.Pause(PauseMethods.PauseMenu);

        StartCoroutine(MMFade.FadeCanvasGroup(TargetInventoryContainer, 0.2f, 0f));
        StartCoroutine(MMFade.FadeCanvasGroup(Overlay, 0.2f, 0f));
    }
}
