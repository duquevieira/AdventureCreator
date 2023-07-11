using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using MoreMountains.InventoryEngine;

public class StoryTests
{

    [Test]
    public void StoryboardTests()
    {
        List<StoryboardStep> storyboard = new List<StoryboardStep>();
        StoryboardStep step0 = new StoryboardStep(storyboard.Count, "collider", Vector3.zero);
        step0.addAcquires(new ItemGroup("acquires", 1));
        step0.addRequirement(new ItemGroup("requires", 2));
        storyboard.Add(step0);


        StoryboardStep step1 = new StoryboardStep(storyboard.Count, "collider1", Vector3.zero);
        step1.addAcquires(new ItemGroup("acquires1", 2));
        step1.addRequirement(new ItemGroup("requires1", 3));
        storyboard.Add(step1);

        Assert.That(storyboard[0].getId(), Is.EqualTo(0));
        Assert.That(storyboard[0].getColliderName(), Is.EqualTo("collider"));
        Assert.That(storyboard[0].getAcquired()[0].getItemName(), Is.EqualTo("acquires"));
        Assert.That(storyboard[0].getAcquired()[0].getItemAmount(), Is.EqualTo(1));
        Assert.That(storyboard[0].getRequirements()[0].getItemName(), Is.EqualTo("requires"));
        Assert.That(storyboard[0].getRequirements()[0].getItemAmount(), Is.EqualTo(2));

        Assert.That(storyboard[1].getId(), Is.EqualTo(1));
        Assert.That(storyboard[1].getColliderName(), Is.EqualTo("collider1"));
        Assert.That(storyboard[1].getAcquired()[0].getItemName(), Is.EqualTo("acquires1"));
        Assert.That(storyboard[1].getAcquired()[0].getItemAmount(), Is.EqualTo(2));
        Assert.That(storyboard[1].getRequirements()[0].getItemName(), Is.EqualTo("requires1"));
        Assert.That(storyboard[1].getRequirements()[0].getItemAmount(), Is.EqualTo(3));
    }

    [Test]
    public void StoryEngineTests()
    {
        var gameObject = new GameObject();
        var storyengine = gameObject.AddComponent<StoryEngineScript>();
        storyengine.Storyboard = new List<StoryboardStep>();
        storyengine.StoryItems = new List<ItemGroup>();
        gameObject = new GameObject();
        
        StoryboardStep step = new StoryboardStep(storyengine.Storyboard.Count, "FAttendant", Vector3.zero);
        step.addAcquires(new ItemGroup("id0", 1));
        storyengine.Storyboard.Add(step);

        step = new StoryboardStep(storyengine.Storyboard.Count, "Basket", Vector3.zero);
        step.addRequirement(new ItemGroup("id0", 1));
        step.addAcquires(new ItemGroup("id1", 5));
        storyengine.Storyboard.Add(step);

        step = new StoryboardStep(storyengine.Storyboard.Count, "Bread", Vector3.zero);
        step.addRequirement(new ItemGroup("id1", 1));
        step.addAcquires(new ItemGroup("BreadItem", 1));
        step.addAcquires(new ItemGroup("id2", 1));
        storyengine.Storyboard.Add(step);

        step = new StoryboardStep(storyengine.Storyboard.Count, "Cheese", Vector3.zero);
        step.addRequirement(new ItemGroup("id1", 1));
        step.addAcquires(new ItemGroup("CheeseItem", 1));
        step.addAcquires(new ItemGroup("id2", 1));
        storyengine.Storyboard.Add(step);

        step = new StoryboardStep(storyengine.Storyboard.Count, "Cake", Vector3.zero);
        step.addRequirement(new ItemGroup("id1", 2));
        step.addAcquires(new ItemGroup("CakeItem", 1));
        step.addAcquires(new ItemGroup("id2", 1));
        storyengine.Storyboard.Add(step);

        step = new StoryboardStep(storyengine.Storyboard.Count, "Door", Vector3.zero);
        step.addRequirement(new ItemGroup("id2", 5));
        step.addAcquires(new ItemGroup("id3", 1));
        storyengine.Storyboard.Add(step);

        step = new StoryboardStep(storyengine.Storyboard.Count, "Wolf", Vector3.zero);
        step.addRequirement(new ItemGroup("id3", 1));
        step.addAcquires(new ItemGroup("id4", 1));
        storyengine.Storyboard.Add(step);

        step = new StoryboardStep(storyengine.Storyboard.Count, "Door1", Vector3.zero);
        step.addRequirement(new ItemGroup("id4", 1));
        step.addAcquires(new ItemGroup("id5", 1));
        storyengine.Storyboard.Add(step);

        step = new StoryboardStep(storyengine.Storyboard.Count, "MHunter", Vector3.zero);
        step.addRequirement(new ItemGroup("id5", 1));
        step.addAcquires(new ItemGroup("id6", 1));
        storyengine.Storyboard.Add(step);

        step = new StoryboardStep(storyengine.Storyboard.Count, "Door1", Vector3.zero);
        step.addRequirement(new ItemGroup("id6", 1));
        step.addAcquires(new ItemGroup("id7", 1));
        storyengine.Storyboard.Add(step);

        step = new StoryboardStep(storyengine.Storyboard.Count, "FShopper", Vector3.zero);
        step.addRequirement(new ItemGroup("id7", 1));
        step.addAcquires(new ItemGroup("final", 1));
        storyengine.Storyboard.Add(step);

        storyengine.ProcessEntry("FAttendant");
        storyengine.ProcessEntry("Basket");
        storyengine.ProcessEntry("Bread");
        storyengine.ProcessEntry("Cheese");
        storyengine.ProcessEntry("Cake");
        storyengine.ProcessEntry("Cake");
        storyengine.ProcessEntry("Door");
        storyengine.ProcessEntry("Wolf");
        storyengine.ProcessEntry("Door1");
        storyengine.ProcessEntry("MHunter");
        storyengine.ProcessEntry("Door1");
        storyengine.ProcessEntry("FShopper");

        int finalAmount = -1;
        int testAmount = -1;
        int other = -1;
        foreach (ItemGroup group in storyengine.StoryItems)
        {
            switch(group.getItemName())
            {
                case "final":
                    finalAmount = group.getItemAmount();
                    break;
                case "id1":
                    testAmount = group.getItemAmount();
                    break;
                case "id7":
                    other = group.getItemAmount();
                    break;
                default: break;
            }
        }
        Assert.That(finalAmount, Is.EqualTo(1));
        Assert.That(testAmount, Is.EqualTo(1));
        Assert.That(other, Is.EqualTo(0));
    }
}
