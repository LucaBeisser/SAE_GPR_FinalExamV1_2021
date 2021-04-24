using NUnit.Framework;
using UnityEngine;

public class LootTests
{
    [Test]
    public void EmptyLootDescriptionTest()
    {
        var lootDescription = ScriptableObject.CreateInstance<LootDescription>();

        lootDescription.SetDrops();
        for (int i = 0; i < 100; i++)
        {
            var drop = lootDescription.SelectDropRandomly();
            Assert.AreEqual(null, drop);
        }

        Object.DestroyImmediate(lootDescription);
    }

    [Test]
    public void CertainDropLootDescriptionTest()
    {
        var lootDescription = ScriptableObject.CreateInstance<LootDescription>();
        var testDrop = ScriptableObject.CreateInstance<Drop>();
        testDrop.DropName = "TestDrop";

        lootDescription.SetDrops(new DropProbabilityPair[] {
            new DropProbabilityPair() { Drop = testDrop, Probability = 1 } });

        for (int i = 0; i < 100; i++)
        {
            var drop = lootDescription.SelectDropRandomly();
            Assert.AreEqual(testDrop, drop);
        }

        Object.DestroyImmediate(lootDescription);
        Object.DestroyImmediate(testDrop);
    }

    [Test]
    public void AlwaysDropSomethingTest()
    {
        var lootDescription = ScriptableObject.CreateInstance<LootDescription>();

        var testDrop1 = ScriptableObject.CreateInstance<Drop>();
        testDrop1.DropName = "TestDrop1";

        var testDrop2 = ScriptableObject.CreateInstance<Drop>();
        testDrop2.DropName = "TestDrop2";

        lootDescription.SetDrops(new DropProbabilityPair[] {
            new DropProbabilityPair() { Drop = testDrop1, Probability =  0.5f },
            new DropProbabilityPair() { Drop = testDrop2, Probability =  0.5f } });


        for (int i = 0; i < 100; i++)
        {
            var drop = lootDescription.SelectDropRandomly();
            Assert.AreNotEqual(null, drop);
        }

        Object.DestroyImmediate(lootDescription);
        Object.DestroyImmediate(testDrop1);
        Object.DestroyImmediate(testDrop2);
    }

    [Test]
    public void OrderDoesNotMatterTest()
    {
        var ab = ScriptableObject.CreateInstance<LootDescription>();
        var ba = ScriptableObject.CreateInstance<LootDescription>();

        var testDrop1 = ScriptableObject.CreateInstance<Drop>();
        testDrop1.DropName = "TestDrop1";

        var testDrop2 = ScriptableObject.CreateInstance<Drop>();
        testDrop2.DropName = "TestDrop2";

        ab.SetDrops(new DropProbabilityPair[] {
            new DropProbabilityPair() { Drop = testDrop1, Probability =  0.2f },
            new DropProbabilityPair() { Drop = testDrop2, Probability =  0.8f } });


        ba.SetDrops(new DropProbabilityPair[] {
            new DropProbabilityPair() { Drop = testDrop2, Probability =  0.8f },
            new DropProbabilityPair() { Drop = testDrop1, Probability =  0.2f } });


        //Dont know how to test it


        Object.DestroyImmediate(ab);
        Object.DestroyImmediate(ba);
        Object.DestroyImmediate(testDrop1);
        Object.DestroyImmediate(testDrop2);
    }
}
