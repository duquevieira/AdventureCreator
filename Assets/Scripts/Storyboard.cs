using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storyboard
{

    private List<StoryboardStep> _steps;

    public Storyboard()
    {
        _steps = new List<StoryboardStep>();
    }

    public void addStep (StoryboardStep step)
    {
        _steps.Add(step);
    }

    public List<StoryboardStep> getStorySteps()
    {
        return _steps;
    }
}
