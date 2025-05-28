using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{

    public static int EnemiesAlive;
    void Start()
    {
        EnemiesAlive = 0;
    }
    public Wave[] waves;
    public Transform spawnPoint;

    public float timeBetweenWaves = 5f;

    // Make countdown accessible to tests
     internal float countdown = 2f;
    private float totalTime = 0f;     // skaičiuoja bendrą laiką


    public Text waveCountdownText;
    private int waveIndex = 0;

    public GameManager gameManager;

    // Make Update() callable from tests
    void Update()
    {
        totalTime += Time.deltaTime;
        waveCountdownText.text = string.Format("{0:00.00}", totalTime);

        if (EnemiesAlive > 0)
        {
            return;
        }

        if (waveIndex == waves.Length)
        {
            gameManager.WinLevel();
            this.enabled = false;
            return;
        }

        if (countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
            return;
        }

        countdown -= Time.deltaTime;
        countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);
    }


    public IEnumerator SpawnWave()
    {
        
        PlayerStats.Rounds++;

        Wave wave = waves[waveIndex];

        EnemiesAlive = wave.count;
        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemy(wave.enemy);
            yield return new WaitForSeconds(1f / wave.rate);
        }
        waveIndex++;

        
    }

    void SpawnEnemy(GameObject enemy)
    {
        Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
    }
}
