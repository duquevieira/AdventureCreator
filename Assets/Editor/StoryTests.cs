using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using MoreMountains.InventoryEngine;

public class StoryTests
{
    [Test]
    public void FakeInventoryTests()
    {
        var gameObject = new GameObject();
        FakeInventory inventory = gameObject.AddComponent<FakeInventory>();

        inventory.Items.Add(new ItemGroup("item1", 1));
        Assert.That(inventory.GetQuantity("item1"), Is.EqualTo(1));
        inventory.Items.Add(new ItemGroup("item2", 5));
        Assert.That(inventory.GetQuantity("item2"), Is.EqualTo(5));
        inventory.Items.Add(new ItemGroup("item1", 10));
        Assert.That(inventory.GetQuantity("item1"), Is.EqualTo(11));

        inventory.RemoveItemByID("item1", 2);
        Assert.That(inventory.GetQuantity("item1"), Is.EqualTo(9));
        inventory.RemoveItemByID("item2", 6);
        Assert.That(inventory.GetQuantity("item2"), Is.EqualTo(0));
    }

    [Test]
    public void StoryboardTests()
    {
        Storyboard storyboard = new Storyboard();
        StoryboardStep step0 = new StoryboardStep(storyboard.getStorySteps().Count, "collider");
        step0.addAcquires(new ItemGroup("acquires", 1));
        step0.addRequirement(new ItemGroup("requires", 2));
        storyboard.addStep(step0);


        StoryboardStep step1 = new StoryboardStep(storyboard.getStorySteps().Count, "collider1");
        step1.addAcquires(new ItemGroup("acquires1", 2));
        step1.addRequirement(new ItemGroup("requires1", 3));
        storyboard.addStep(step1);

        Assert.That(storyboard.getStorySteps()[0].getId(), Is.EqualTo(0));
        Assert.That(storyboard.getStorySteps()[0].getColliderName(), Is.EqualTo("collider"));
        Assert.That(storyboard.getStorySteps()[0].getAcquired()[0].getItemName(), Is.EqualTo("acquires"));
        Assert.That(storyboard.getStorySteps()[0].getAcquired()[0].getItemAmount(), Is.EqualTo(1));
        Assert.That(storyboard.getStorySteps()[0].getRequirements()[0].getItemName(), Is.EqualTo("requires"));
        Assert.That(storyboard.getStorySteps()[0].getRequirements()[0].getItemAmount(), Is.EqualTo(2));

        Assert.That(storyboard.getStorySteps()[1].getId(), Is.EqualTo(1));
        Assert.That(storyboard.getStorySteps()[1].getColliderName(), Is.EqualTo("collider1"));
        Assert.That(storyboard.getStorySteps()[1].getAcquired()[0].getItemName(), Is.EqualTo("acquires1"));
        Assert.That(storyboard.getStorySteps()[1].getAcquired()[0].getItemAmount(), Is.EqualTo(2));
        Assert.That(storyboard.getStorySteps()[1].getRequirements()[0].getItemName(), Is.EqualTo("requires1"));
        Assert.That(storyboard.getStorySteps()[1].getRequirements()[0].getItemAmount(), Is.EqualTo(3));
    }

    [Test]
    public void StoryEngineTests()
    {
        var gameObject = new GameObject();
        var storyengine = gameObject.AddComponent<StoryEngineScript>();
        storyengine.Storyboard = new Storyboard();
        storyengine.StoryItems = new List<ItemGroup>();
        gameObject = new GameObject();
        storyengine.Inventory = gameObject.AddComponent<FakeInventory>();
        
        StoryboardStep step = new StoryboardStep(storyengine.Storyboard.getStorySteps().Count, "FAttendant");
        step.addAcquires(new ItemGroup("id0", 1));
        storyengine.Storyboard.addStep(step);

        step = new StoryboardStep(storyengine.Storyboard.getStorySteps().Count, "Basket");
        step.addRequirement(new ItemGroup("id0", 1));
        step.addAcquires(new ItemGroup("id1", 5));
        storyengine.Storyboard.addStep(step);

        step = new StoryboardStep(storyengine.Storyboard.getStorySteps().Count, "Bread");
        step.addRequirement(new ItemGroup("id1", 1));
        step.addAcquires(new ItemGroup("BreadItem", 1));
        step.addAcquires(new ItemGroup("id2", 1));
        storyengine.Storyboard.addStep(step);

        step = new StoryboardStep(storyengine.Storyboard.getStorySteps().Count, "Cheese");
        step.addRequirement(new ItemGroup("id1", 1));
        step.addAcquires(new ItemGroup("CheeseItem", 1));
        step.addAcquires(new ItemGroup("id2", 1));
        storyengine.Storyboard.addStep(step);

        step = new StoryboardStep(storyengine.Storyboard.getStorySteps().Count, "Cake");
        step.addRequirement(new ItemGroup("id1", 2));
        step.addAcquires(new ItemGroup("CakeItem", 1));
        step.addAcquires(new ItemGroup("id2", 1));
        storyengine.Storyboard.addStep(step);

        step = new StoryboardStep(storyengine.Storyboard.getStorySteps().Count, "Door");
        step.addRequirement(new ItemGroup("id2", 5));
        step.addAcquires(new ItemGroup("id3", 1));
        storyengine.Storyboard.addStep(step);

        step = new StoryboardStep(storyengine.Storyboard.getStorySteps().Count, "Wolf");
        step.addRequirement(new ItemGroup("id3", 1));
        step.addAcquires(new ItemGroup("id4", 1));
        storyengine.Storyboard.addStep(step);

        step = new StoryboardStep(storyengine.Storyboard.getStorySteps().Count, "Door1");
        step.addRequirement(new ItemGroup("id4", 1));
        step.addAcquires(new ItemGroup("id5", 1));
        storyengine.Storyboard.addStep(step);

        step = new StoryboardStep(storyengine.Storyboard.getStorySteps().Count, "MHunter");
        step.addRequirement(new ItemGroup("id5", 1));
        step.addAcquires(new ItemGroup("id6", 1));
        storyengine.Storyboard.addStep(step);

        step = new StoryboardStep(storyengine.Storyboard.getStorySteps().Count, "Door1");
        step.addRequirement(new ItemGroup("id6", 1));
        step.addAcquires(new ItemGroup("id7", 1));
        storyengine.Storyboard.addStep(step);

        step = new StoryboardStep(storyengine.Storyboard.getStorySteps().Count, "FShopper");
        step.addRequirement(new ItemGroup("id7", 1));
        step.addAcquires(new ItemGroup("final", 1));
        storyengine.Storyboard.addStep(step);

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
