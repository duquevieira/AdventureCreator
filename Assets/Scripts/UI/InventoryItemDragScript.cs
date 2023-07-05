using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryItemDragScript : DuplicateDragScript
{
    private StoryEngineScript _storyEngineScript;

    public void SetStoryEngineScript(StoryEngineScript storyEngineScript)
    {
        _storyEngineScript = storyEngineScript;
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        Destroy(_clone);
        if(Physics.Raycast(Camera.main.ScreenPointToRay(eventData.position), out RaycastHit hit)) {
            _storyEngineScript.ProcessEntry(hit.collider.name);
        }
    }
}
