using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] Wave[] waves;
    public Transform[] spawnPoints;
    public Transform bossSpawnPoint;
    public Animator animator;
    public Animator transition;
    public TMP_Text waveName;
    public TMP_Text enemiesLeft;
    private Wave currentWave;
    private int currentWaveNumber;
    private float nextSpawnTime;
    public bool canSpawn = false;
    private bool canAnimate = false;
    [SerializeField] AudioSource inGameMusic, bossMusic;
    public Tilemap objectTilemap, objectMapTilemap;

    public List<UnityEngine.Rendering.Universal.Light2D> lightsToTurnOff;


    private void Start()
    {
        PlayInGameMusic();
    }

    private void PlayInGameMusic()
    {
        inGameMusic.Play();
        bossMusic.Stop();
    }

    private void Update()
    {
        currentWave = waves[currentWaveNumber];
        SpawnWave();
        GameObject[] totalEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (totalEnemies.Length == 0)
        {
            if (currentWaveNumber + 1 != waves.Length)
            {
                if (canAnimate)
                {
                    waveName.text = waves[currentWaveNumber + 1].waveName;
                    animator.SetTrigger("WaveComplete");
                    canAnimate = false;
                }
            }
            else
            {
                StartCoroutine(TransitionToGameWon());
            }
        }
    }

    IEnumerator TransitionToGameWon()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("GameWon");
    }
    void SpawnWave()
    {
        if (canSpawn && nextSpawnTime < Time.time && currentWave.noOfEnemies > 0)
        {
            Transform spawnPoint = (currentWaveNumber == waves.Length - 1) ? bossSpawnPoint : GetRandomSpawnPoint();
            GameObject randomEnemy = currentWave.typeOfEnemies[Random.Range(0, currentWave.typeOfEnemies.Length)];
            Instantiate(randomEnemy, spawnPoint.position, Quaternion.identity);
            currentWave.noOfEnemies--;
            nextSpawnTime = Time.time + currentWave.spawnInterval;
            if (currentWave.noOfEnemies == 0)
            {
                canSpawn = false;
                canAnimate = true;

                if (currentWaveNumber == waves.Length - 1)
                {
                    PlayBossMusic();
                    objectTilemap.gameObject.SetActive(false);
                    objectMapTilemap.gameObject.SetActive(false);
                    foreach (var light in lightsToTurnOff)
                    {
                        light.enabled = false;
                    }
                }
            }
        }
    }
    
    void SpawnNextWave()
    {
        currentWaveNumber++;
        canSpawn = true;
    }


    void PlayBossMusic()
    {
        inGameMusic.Stop();
        bossMusic.Play();
    }

    Transform GetRandomSpawnPoint()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Length)];
    }
}
