using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

// Dummy Waypoints class for testing Enemy behavior.
// Remove or replace if you already have a Waypoints script in your project.
public static class Waypoints
{
    public static Transform[] points;
}

public class MyGameTests
{
    [UnityTest]
    public IEnumerator Bullet_DestroysItselfWhenTargetIsNull()
    {
        GameObject bulletGO = new GameObject("Bullet");
        Bullet bullet = bulletGO.AddComponent<Bullet>();

        GameObject dummyImpact = new GameObject("ImpactEffect");
        bullet.impactEffect = dummyImpact;

        yield return null;

        yield return null;
        Assert.IsTrue(bulletGO == null, "Bullet should be destroyed when target is null");

        // Clean up
        Object.Destroy(dummyImpact);
    }


    [UnityTest]
    public IEnumerator BuildManager_SetsSingletonInstance()
    {
        GameObject buildManagerGO = new GameObject("BuildManager");
        BuildManager buildManager = buildManagerGO.AddComponent<BuildManager>();

        yield return null;

        Assert.IsNotNull(BuildManager.instance, "BuildManager.instance should be set");

        Object.Destroy(buildManagerGO);
    }

    [UnityTest]
    public IEnumerator Bullet_HitsEnemyAndDestroysIt()
    {
        GameObject enemyGO = new GameObject("Enemy");
        enemyGO.transform.position = Vector3.zero;

        // Create a bullet GameObject.
        GameObject bulletGO = new GameObject("Bullet");
        Bullet bullet = bulletGO.AddComponent<Bullet>();
        bullet.speed = 70f;

        GameObject dummyImpact = new GameObject("ImpactEffect");
        bullet.impactEffect = dummyImpact;

        bulletGO.transform.position = enemyGO.transform.position;

        bullet.Seek(enemyGO.transform);

        yield return null;
        yield return null;

        Assert.IsTrue(enemyGO == null, "Enemy should be destroyed when bullet reaches it.");
        Assert.IsTrue(bulletGO == null, "Bullet should be destroyed after hitting the enemy.");

        if (dummyImpact != null)
        {
            Object.Destroy(dummyImpact);
        }
    }

}
