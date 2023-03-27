using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storyboard
{

    private List<StoryboardStep> steps;

    public Storyboard()
    {
        steps = new List<StoryboardStep>();
    }

    public void addStep (StoryboardStep step)
    {
        steps.Add(step);
    }

    public List<StoryboardStep> getStorySteps()
    {
        return steps;
    }
}
