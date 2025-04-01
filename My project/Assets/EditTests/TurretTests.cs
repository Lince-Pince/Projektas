using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TurretTests
{

    // Helper to safely destroy objects whether in play mode or edit mode.
    private static void SafeDestroy(Object obj)
    {
        if (Application.isPlaying)
            Object.Destroy(obj);
        else
            Object.DestroyImmediate(obj);
    }

    // ----- Spy Test -----
    // Verifies that when Turret.Shoot is invoked, the spawned Bullet’s internal 'target' field is set correctly.
    [UnityTest]
    public IEnumerator Turret_Shoot_SpyTest()
    {
        // Create a dummy enemy.
        var enemyGO = new GameObject("Enemy");
        enemyGO.tag = "Enemy";

        // Set up turret with required child objects.
        var turretGO = new GameObject("Turret");
        var turret = turretGO.AddComponent<Turret>();
        turret.partToRotate = new GameObject("PartToRotate").transform;
        turret.firePoint = new GameObject("FirePoint").transform;

        // Create a bullet prefab.
        var bulletPrefab = new GameObject("BulletPrefab");
        bulletPrefab.AddComponent<Bullet>();
        turret.bulletPrefab = bulletPrefab;

        // Manually set turret's private 'target' field to enemy.
        typeof(Turret)
            .GetField("target", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(turret, enemyGO.transform);

        // Invoke Shoot (using SendMessage to bypass access restrictions).
        turret.SendMessage("Shoot", SendMessageOptions.DontRequireReceiver);
        yield return null; // Wait one frame for the bullet to be instantiated.

        // Find the spawned bullet using the new API.
        Bullet spawnedBullet = null;
        foreach (var b in Object.FindObjectsByType<Bullet>(FindObjectsSortMode.None))
        {
            if (b.gameObject.name.Contains("BulletPrefab"))
                spawnedBullet = b;
        }
        Assert.IsNotNull(spawnedBullet, "Bullet was not spawned");

        // Check bullet's private 'target' field.
        var bulletTarget = (Transform)typeof(Bullet)
            .GetField("target", BindingFlags.NonPublic | BindingFlags.Instance)
            .GetValue(spawnedBullet);
        Assert.AreEqual(enemyGO.transform, bulletTarget, "Bullet's target is not set correctly");

        // Cleanup.
        SafeDestroy(enemyGO);
        SafeDestroy(turretGO);
        SafeDestroy(bulletPrefab);
        foreach (var b in Object.FindObjectsByType<Bullet>(FindObjectsSortMode.None))
        {
            SafeDestroy(b.gameObject);
        }
    }

    // ----- Mock Test -----
    // Creates two enemy GameObjects and verifies that Turret.UpdateTarget picks the nearest enemy.
    [UnityTest]
    public IEnumerator Turret_UpdateTarget_MockTest()
    {
        // Create two enemy GameObjects at different distances.
        var enemy1 = new GameObject("Enemy1");
        enemy1.tag = "Enemy";
        enemy1.transform.position = new Vector3(5, 0, 0);

        var enemy2 = new GameObject("Enemy2");
        enemy2.tag = "Enemy";
        enemy2.transform.position = new Vector3(10, 0, 0);

        // Create turret.
        var turretGO = new GameObject("Turret");
        var turret = turretGO.AddComponent<Turret>();
        turretGO.transform.position = Vector3.zero;
        turret.range = 20f;

        // Invoke private UpdateTarget via reflection.
        typeof(Turret)
            .GetMethod("UpdateTarget", BindingFlags.NonPublic | BindingFlags.Instance)
            .Invoke(turret, null);

        // Retrieve private 'target' field.
        var selectedTarget = (Transform)typeof(Turret)
            .GetField("target", BindingFlags.NonPublic | BindingFlags.Instance)
            .GetValue(turret);
        Assert.IsNotNull(selectedTarget, "No target selected");
        Assert.AreEqual(enemy1.transform, selectedTarget, "Nearest enemy was not selected");

        // Cleanup.
        SafeDestroy(enemy1);
        SafeDestroy(enemy2);
        SafeDestroy(turretGO);
        yield return null;
    }
}
