using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private GameObject[] Enemies = new GameObject[4];
    [SerializeField] private Transform Spawner;
    [SerializeField] public static List<GameObject> Wave;
    [SerializeField] public static bool inWave;
    [SerializeField] private int waveNumber;

    private void Start()
    {
        Wave = new List<GameObject>();
        waveNumber = 0;
    }

    void Update()
    {
        if(Wave.Count > 0)
        {
            inWave = true;
            
        }

        if(Wave.Count <= 0)
        {
            inWave = false;
            UiManager.changing = true;
        }

    }

    //augmentable for spawning different enemies
    public void SpawnWave(int spawnInt, GameObject enemySpawned)
    {
        Wave.Add(Instantiate(enemySpawned, Spawner.transform.position, Quaternion.identity));
        spawnInt--;
        if (spawnInt > 0)
            SpawnWave(spawnInt, enemySpawned);
    }

    public void WaveProgresser()
    {
        int mookCount = 1 + 2 * waveNumber;
        waveNumber++;
        //picks a random amount of enemies to spawn
        //can be more complex by factoring in the enemy spawn so that swarms or tough enemies spawn in different amounts 
        //by changing mookCount to take this into account, or making the random.range function also pick a different "mookCount" that would adjust spawn rates accordingly.
        SpawnWave(mookCount, Enemies[Random.Range(0, Enemies.Length)]);
    }
}
