using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class WaveSpawnerTests
{

    // Helper to safely destroy objects.
    private static void SafeDestroy(Object obj)
    {
        if (Application.isPlaying)
            Object.Destroy(obj);
        else
            Object.DestroyImmediate(obj);
    }

    // ----- Spy Test -----
    // Forces a wave spawn and verifies that the number of spawned enemy clones equals the private wave index.
    [UnityTest]
    public IEnumerator WaveSpawner_SpawnsCorrectEnemies_SpyTest()
    {
        // Create a dummy enemy prefab.
        var enemyPrefab = new GameObject("EnemyPrefab");

        // Create a spawn point.
        var spawnPoint = new GameObject("SpawnPoint").transform;

        // Create a dummy UI Text.
        var waveText = new GameObject("WaveText").AddComponent<Text>();

        // Create WaveSpawner and set required properties.
        var spawnerGO = new GameObject("WaveSpawner");
        var spawner = spawnerGO.AddComponent<WaveSpawner>();
        spawner.enemyPrefab = enemyPrefab.transform;
        spawner.spawnPoint = spawnPoint;
        spawner.waveCountdownText = waveText;
        spawner.timeBetweenWaves = 5f;

        // Force a wave spawn by setting the private 'countdown' field to 0.
        typeof(WaveSpawner)
            .GetField("countdown", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(spawner, 0f);

        yield return new WaitForSeconds(1f); // Wait for the coroutine to run.

        // Retrieve private waveIndex.
        int waveIndex = (int)typeof(WaveSpawner)
            .GetField("waveIndex", BindingFlags.NonPublic | BindingFlags.Instance)
            .GetValue(spawner);

        // Count spawned enemy clones using the new API.
        int enemyCount = 0;
        foreach (var go in Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None))
        {
            if (go.name.Contains("EnemyPrefab"))
                enemyCount++;
        }
        Assert.AreEqual(waveIndex, enemyCount, "Spawned enemy count does not match wave index");

        // Cleanup.
        SafeDestroy(enemyPrefab);
        SafeDestroy(spawnPoint.gameObject);
        SafeDestroy(waveText.gameObject);
        SafeDestroy(spawnerGO);
        yield return null;
    }
}
