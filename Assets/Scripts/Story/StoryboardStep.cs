using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class StoryboardStep
{
    public int Id;
    public string ColliderName;
    public List<ItemGroup> Requirements;
    public List<ItemGroup> Acquired;
    public List<int> ItemDependentSteps;
    public List<float> StepCoordinates;
    public int Animation;
    public string Dialog;

    public StoryboardStep(int id, string colliderName, Vector3 stepCoordinates)
    {
        Id = id;
        ColliderName = colliderName;
        StepCoordinates = new List<float>()
        {
            stepCoordinates.x,
            stepCoordinates.y,
            stepCoordinates.z
        };
        Requirements = new List<ItemGroup>();
        Acquired = new List<ItemGroup>();
        ItemDependentSteps = new List<int>();
        Animation = -1;
    }

    public void addAnimation(int animation)
    {
        Animation = animation;
    }

    public void addDialog(string dialog)
    {
        Dialog = dialog;
    }

    public void addRequirement(ItemGroup requirement)
    {
        Requirements.Add(requirement);
    }

    public void addAcquires(ItemGroup acquired)
    {
        Acquired.Add(acquired);
    }

    public void addItemDependentStep(int stepId)
    {
        ItemDependentSteps.Add(stepId);
    }

    public List<int> getItemDependentSteps()
    {
        return ItemDependentSteps;
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

    public int getAnimation()
    {
        return Animation;
    }

    public string getDialog()
    {
        return Dialog;
    }
}
