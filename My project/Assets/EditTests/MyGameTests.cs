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
    public IEnumerator Enemy_ReachesFinalWaypointAndDestroysItself()
    {
        // Create dummy waypoints
        GameObject wp1 = new GameObject("WP1");
        GameObject wp2 = new GameObject("WP2");
        wp1.transform.position = Vector3.zero;
        wp2.transform.position = new Vector3(5f, 0f, 0f);
        Waypoints.points = new Transform[] { wp1.transform, wp2.transform };

        // Create an Enemy at the first waypoint
        GameObject enemyGO = new GameObject("Enemy");
        Enemy enemy = enemyGO.AddComponent<Enemy>();
        enemyGO.transform.position = wp1.transform.position;

        yield return null; // Let Enemy.Start() run

        // Move the enemy to the final waypoint instantly
        enemyGO.transform.position = wp2.transform.position;
        yield return null; // Let Enemy.Update() run

        // The enemy should be destroyed after reaching final waypoint
        yield return null;
        Assert.IsTrue(enemyGO == null, "Enemy should be destroyed upon reaching the final waypoint");

        // Clean up
        Object.Destroy(wp1);
        Object.Destroy(wp2);
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
    public IEnumerator WaveSpawner_SpawnsEnemyWaves()
    {
        // Create a WaveSpawner GameObject
        GameObject spawnerGO = new GameObject("WaveSpawner");
        WaveSpawner spawner = spawnerGO.AddComponent<WaveSpawner>();

        // Create a dummy enemy prefab
        GameObject enemyPrefabGO = new GameObject("EnemyPrefab");
        // Optionally, add the Enemy script if you want
        enemyPrefabGO.AddComponent<Enemy>();
        // Ensure the prefab has the "Enemy" tag so we can find it
        enemyPrefabGO.tag = "Enemy";

        // Assign the dummy prefab to the spawner
        spawner.enemyPrefab = enemyPrefabGO.transform;

        // Create a spawn point
        GameObject spawnPointGO = new GameObject("SpawnPoint");
        spawnPointGO.transform.position = Vector3.zero;
        spawner.spawnPoint = spawnPointGO.transform;

        // Create a dummy Text for the wave countdown
        GameObject textGO = new GameObject("WaveCountdownText");
        Text countdownText = textGO.AddComponent<Text>();
        spawner.waveCountdownText = countdownText;

        // Force immediate wave spawn
        spawner.timeBetweenWaves = 0f;
        yield return null; // Let Update run once, so it starts the coroutine

        // Wait enough time for the SpawnWave coroutine to run
        yield return new WaitForSeconds(1f);

        // Verify at least one enemy was spawned (by tag)
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Assert.IsTrue(enemies.Length > 0, "WaveSpawner should have spawned at least one enemy");

        // Clean up
        Object.Destroy(spawnerGO);
        Object.Destroy(enemyPrefabGO);
        Object.Destroy(spawnPointGO);
        Object.Destroy(textGO);
    }
}
