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
        // Create a Bullet game object
        GameObject bulletGO = new GameObject("Bullet");
        Bullet bullet = bulletGO.AddComponent<Bullet>();

        // Create a dummy impact effect
        GameObject dummyImpact = new GameObject("ImpactEffect");
        bullet.impactEffect = dummyImpact;

        // Don't call Seek, so target remains null
        yield return null; // Let Bullet.Update() run once

        // After Update, the bullet should schedule itself for destruction
        yield return null; // Unity destroys objects at end of frame
        Assert.IsTrue(bulletGO == null, "Bullet should be destroyed when target is null");

        // Clean up
        Object.Destroy(dummyImpact);
    }


    [UnityTest]
    public IEnumerator BuildManager_SetsSingletonInstance()
    {
        // Create a BuildManager
        GameObject buildManagerGO = new GameObject("BuildManager");
        BuildManager buildManager = buildManagerGO.AddComponent<BuildManager>();

        yield return null; // Allow Awake() and Start() to run

        Assert.IsNotNull(BuildManager.instance, "BuildManager.instance should be set");

        // Clean up
        Object.Destroy(buildManagerGO);
    }

    [UnityTest]
    public IEnumerator Bullet_HitsEnemyAndDestroysIt()
    {
        // Create an enemy GameObject.
        GameObject enemyGO = new GameObject("Enemy");
        // Set enemy position (for clarity, we put it at the origin).
        enemyGO.transform.position = Vector3.zero;

        // Create a bullet GameObject.
        GameObject bulletGO = new GameObject("Bullet");
        Bullet bullet = bulletGO.AddComponent<Bullet>();
        bullet.speed = 70f; // Default speed; adjust if needed.

        // Create a dummy impact effect to assign to the bullet.
        GameObject dummyImpact = new GameObject("ImpactEffect");
        bullet.impactEffect = dummyImpact;

        // Position the bullet exactly at the enemy's location.
        // This ensures that the distance between them is zero.
        bulletGO.transform.position = enemyGO.transform.position;

        // Set the bullet to seek the enemy.
        bullet.Seek(enemyGO.transform);

        // Wait a couple of frames to allow Update() to run and process the hit.
        yield return null;
        yield return null;

        // At this point, the bullet should have called HitTarget() and destroyed both itself and the enemy.
        Assert.IsTrue(enemyGO == null, "Enemy should be destroyed when bullet reaches it.");
        Assert.IsTrue(bulletGO == null, "Bullet should be destroyed after hitting the enemy.");

        // Cleanup: Destroy the dummy impact effect if it still exists.
        if (dummyImpact != null)
        {
            Object.Destroy(dummyImpact);
        }
    }

}
