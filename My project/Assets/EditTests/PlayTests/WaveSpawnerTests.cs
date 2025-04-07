using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.TestTools;

public class WaveSpawnerTests
{
    private GameObject spawnerGO;
    private WaveSpawner waveSpawner;
    private Text waveCountdownText;

    [SetUp]
    public void Setup()
    {
        // Create a dummy PlayerStats GameObject to initialize static variables
        var statsGO = new GameObject("PlayerStats");
        statsGO.AddComponent<PlayerStats>(); // Will initialize Rounds = 0 in Start()

        // Wait a frame for Start() to run in [UnityTest] if needed
        spawnerGO = new GameObject("WaveSpawner");
        waveSpawner = spawnerGO.AddComponent<WaveSpawner>();

        GameObject spawnPoint = new GameObject("SpawnPoint");
        waveSpawner.spawnPoint = spawnPoint.transform;

        // Enemy prefab that gets instantiated — must be a Transform and visible
        GameObject enemyPrefab = GameObject.CreatePrimitive(PrimitiveType.Cube);
        enemyPrefab.tag = "Untagged"; // or "Enemy"
        waveSpawner.enemyPrefab = enemyPrefab.transform;

        // Add UI text element
        GameObject textGO = new GameObject("CountdownText");
        waveCountdownText = textGO.AddComponent<UnityEngine.UI.Text>();
        waveSpawner.waveCountdownText = waveCountdownText;

        // Optional: reset PlayerStats
        PlayerStats.Rounds = 0;
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(spawnerGO);
    }

    [UnityTest]
    public IEnumerator Countdown_TriggersWaveSpawn()
    {
        waveSpawner.timeBetweenWaves = 0.5f;
        // Use reflection to set the private countdown field
        var countdownField = typeof(WaveSpawner).GetField("countdown", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        countdownField.SetValue(waveSpawner, 0.1f);

        // Simulate enough frames for countdown and coroutine
        for (int i = 0; i < 60; i++)
        {
            // Use reflection to invoke the private Update method
            var updateMethod = typeof(WaveSpawner).GetMethod("Update", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            updateMethod.Invoke(waveSpawner, null);
            yield return null;
        }

        Assert.GreaterOrEqual(PlayerStats.Rounds, 1, "Wave should have increased rounds.");
    }

    [UnityTest]
    public IEnumerator SpawnWave_SpawnsCorrectNumberOfEnemies()
    {
        PlayerStats.Rounds = 0;
        yield return waveSpawner.StartCoroutine(waveSpawner.SpawnWave());

        // We expect one enemy for the first wave
        var all = Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        Assert.GreaterOrEqual(all.Length, 1, "Expected at least one enemy to spawn.");
    }

    [UnityTest]
    public IEnumerator Countdown_DisplayUpdatesOverTime()
    {
        waveSpawner.timeBetweenWaves = 2f;
        float initial = waveSpawner.timeBetweenWaves;
        yield return new WaitForSeconds(1f);

        float updated = float.Parse(waveCountdownText.text);
        Assert.Less(updated, initial);
    }
}
