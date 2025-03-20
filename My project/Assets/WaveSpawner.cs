using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    public Transform enemyPrefab;

    public Transform spawnPoint;
    //galima bus sunkint lygi ir skirtingus enemy

    public float timeBetweenWaves = 5f;
    private float countdown = 3f;

    public Text waveCountdownText;
    private int waveIndex = 0;



    void Update()
    {
        if(countdown <= 0)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;

        }
        countdown -= Time.deltaTime;
        waveCountdownText.text = Mathf.Round(countdown).ToString();
    }

    IEnumerator SpawnWave()
    {
        waveIndex++;
        Debug.Log("Wave Inc");
        for (int i = 0; i < waveIndex; i++)
        {

            SpawnEnemy();
            yield return new WaitForSeconds(0.5f);
        }
        
    }
    
    void SpawnEnemy()
    {
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }

}
