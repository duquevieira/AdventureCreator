using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryboardEvent
{
    private int id;
    private string actionName;
    private string colliderName;
    private string inventoryItemName;
    private int inventoryItemChange;
    private string dialog;
    private List<int> nextEvents;

    public StoryboardEvent(int id, string actionName, string colliderName, string inventoryItemName,
        int inventoryItemChange, string dialog, List<int> nextEvents)
    {
        this.id = id;
        this.actionName = actionName;
        this.colliderName = colliderName;
        this.inventoryItemName = inventoryItemName;
        this.inventoryItemChange = inventoryItemChange;
        this.dialog = dialog;
        this.nextEvents = nextEvents;
    }

    public int getId()
    {
        return id;
    }

    public string getActionName()
    {
        return actionName;
    }

    public string getColliderName()
    {
        return colliderName;
    }

    public string getInventoryItemName()
    {
        return inventoryItemName;
    }

    public int getInventoryItemChange()
    {
        return inventoryItemChange;
    }

    public string getDialog()
    {
        return dialog;
    }

    public List<int> getNextEvents()
    {
        return nextEvents;
    }

    public void addNextEvent(int id)
    {
        nextEvents.Add(id);
    }
}
