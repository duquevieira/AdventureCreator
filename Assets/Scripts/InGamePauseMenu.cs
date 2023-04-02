using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.InventoryEngine;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;


public class InGamePauseMenu : MonoBehaviour
{
    public InventoryInputManager InventoryScript;
    public CanvasGroup Overlay;
    public CanvasGroup TargetInventoryContainer;

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

    private void OpenPauseMenu()
    {
        TargetInventoryContainer.blocksRaycasts = true;

        _pauseMenuOpen = true;
        MMGameEvent.Trigger("inventoryOpens");

        StartCoroutine(MMFade.FadeCanvasGroup(TargetInventoryContainer, 0.2f, 1f));
        StartCoroutine(MMFade.FadeCanvasGroup(Overlay, 0.2f, 0.85f));
    }

    private void ClosePauseMenu()
    {
        TargetInventoryContainer.blocksRaycasts = false;

        _pauseMenuOpen = false;
        MMGameEvent.Trigger("inventoryCloses");

        StartCoroutine(MMFade.FadeCanvasGroup(TargetInventoryContainer, 0.2f, 0f));
        StartCoroutine(MMFade.FadeCanvasGroup(Overlay, 0.2f, 0f));
    }
}
