using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class StoryboardStep : Object
{
    public int Id;
    public string ColliderName;
    public List<ItemGroup> Requirements;
    public List<ItemGroup> Acquired;

    public List<float> StepCoordinates;


    public StoryboardStep(int id, string colliderName, Vector3 stepCoordinates)
    {
        Id = id;
        ColliderName = colliderName;
        StepCoordinates = new List<float>()
        {
            stepCoordinates.x,
            stepCoordinates.y,
            stepCoordinates.z,
        };
        Requirements = new List<ItemGroup>();
        Acquired = new List<ItemGroup>();
    }

    public void addRequirement(ItemGroup requirement)
    {
        Requirements.Add(requirement);
    }

    public void addAcquires(ItemGroup acquired)
    {
        Acquired.Add(acquired);
    }

    public int getId()
    {
        return Id;
    }

    public string getColliderName()
    {
        return ColliderName;
    }

    public List<ItemGroup> getRequirements()
    {
        return Requirements;
    }

    public List<ItemGroup> getAcquired()
    {
        return Acquired;
    } 

    public List<float> getStepCoordinates()
    {
        return StepCoordinates;
    }
}