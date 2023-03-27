using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryboardStep
{
    private int _id;
    private string _colliderName;
    private List<ItemGroup> _requirements;
    private string _dialog;
    private List<ItemGroup> _acquired;

    public StoryboardStep(int id, string colliderName, string dialog)
    {
        _id = id;
        _colliderName = colliderName;
        _requirements = new List<ItemGroup>();
        _dialog = dialog;
        _acquired = new List<ItemGroup>();
    }

    public void addRequirement(ItemGroup requirement)
    {
        _requirements.Add(requirement);
    }

    public void addAcquires(ItemGroup acquired)
    {
        _acquired.Add(acquired);
    }

    public int getId()
    {
        return _id;
    }

    public string getColliderName()
    {
        return _colliderName;
    }

    public List<ItemGroup> getRequirements()
    {
        return _requirements;
    }

    public string getDialog()
    {
        return _dialog;
    }

    public List<ItemGroup> getAcquired()
    {
        return _acquired;
    } 
}
