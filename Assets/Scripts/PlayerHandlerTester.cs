using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandlerTester : PlayerHandlerScript
{
    [SerializeField]
    private SwitchCreateMode _createMode;

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (_createMode.currentMode == SwitchCreateMode.CreateMode.TestingMode)
        {
            _character.SetActive(true);
            _canMove = true;
            if (Input.GetMouseButton(1))
            {
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    _playerAnimator.SetBool(ANIMATION_WALK, true);
                    Target = hit.point;
                }
            }
        }
        else
        {
            _canMove = false;
            _character.SetActive(false);
        }
    }
}
