/*using NUnit.Framework;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

public class ExplodeSpyTest
{
    private GameObject bulletGO;
    private BulletSpy bullet;

    [SetUp]
    public void Setup()
    {
        bulletGO = new GameObject("Bullet");
        bullet = bulletGO.AddComponent<BulletSpy>();
        bullet.explosionRadius = 5f;

        // Stub enemy inside explosion radius
        GameObject enemy1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        enemy1.transform.position = bulletGO.transform.position + Vector3.forward * 1f;
        enemy1.tag = "Enemy";
        enemy1.AddComponent<Enemy>();

        // Stub enemy outside explosion radius
        GameObject enemy2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        enemy2.transform.position = bulletGO.transform.position + Vector3.forward * 20f;
        enemy2.tag = "Enemy";
        enemy2.AddComponent<Enemy>();
    }

    [TearDown]
    public void Teardown()
    {
        if (bulletGO != null)
            UnityEngine.Object.DestroyImmediate(bulletGO);

        // Clean only tagged enemies
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var obj in enemies)
        {
            UnityEngine.Object.DestroyImmediate(obj);
        }
    }

    [UnityTest]
    public IEnumerator Explode_DamagesOnlyEnemiesInRange()
    {
        *//* Spy Test + Stubbed Enemies *//*

        yield return null;

        // Add delay for Unity physics engine
        yield return new WaitForFixedUpdate();

        bullet.TestExplodeWrapper(); // Trigger explode logic
        yield return null;

        Assert.AreEqual(1, bullet.damageCalls, "Only 1 enemy within range should be damaged.");
    }

}
public class BulletSpy : Bullet
{
    public int damageCalls = 0;

    *//* Spy Test: override Damage to count calls *//*
    protected override void Damage(Transform enemy)
    {
        damageCalls++;
        base.Damage(enemy);
    }

    // Public method to expose protected Explode() for test
    public void TestExplodeWrapper()
    {
        Explode();
    }
}
*/