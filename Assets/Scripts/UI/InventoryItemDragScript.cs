using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryItemDragScript : DuplicateDragScript
{
    private PlayerHandlerScript _playerHandlerScript;

    public void setPlayerScript(PlayerHandlerScript playerHandlerScript)
    {
        _playerHandlerScript = playerHandlerScript;
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if(Physics.Raycast(Camera.main.ScreenPointToRay(eventData.position), out RaycastHit hit)) {

            _playerHandlerScript.ProcessEntry(hit, _clone.name);
        }
        Destroy(_clone);
    }
}
