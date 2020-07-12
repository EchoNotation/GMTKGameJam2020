using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gamecontroller : MonoBehaviour
{
    public static Gamecontroller instance = null;
    public int waveNumber;
    private ArrayList enemiesActive;
    private float halfWidth, waveSpawnThreshold, waveEnemyCount;
    private Camera mainCamera;
    public GameObject enemy, bombmer;
    private int[] waveEnemyMix;
    private bool trickling;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        waveSpawnThreshold = 0.25f;
        enemiesActive = new ArrayList();
        Reset();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (mainCamera == null)
        {
            if (GameObject.FindGameObjectWithTag("Player") != null)
            {
                mainCamera = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Camera>();
                halfWidth = mainCamera.orthographicSize * mainCamera.aspect;
            }
        }
        else
        {
            Debug.Log(enemiesActive.Count / waveEnemyCount);
            if (enemiesActive.Count / waveEnemyCount < waveSpawnThreshold)
            {
                SpawnNextWave();
            }
            if (!trickling)
            {
                StartCoroutine(SpawnTrickle());
            }
        }
    }

    public void Reset()
    {
        mainCamera = null;
        enemiesActive.Clear();
        waveNumber = 0;
        waveEnemyCount = 1;
        StopAllCoroutines();
        trickling = false;
    }
    private IEnumerator SpawnTrickle()
    {
        trickling = true;
        yield return new WaitForSeconds(Random.Range(100, 501) * 0.01f);
        SpawnEnemy(SpawnLocation());
        waveEnemyCount = enemiesActive.Count / ((enemiesActive.Count - 1) / waveEnemyCount);
        trickling = false;
    }

    private void SpawnNextWave()
    {
        waveEnemyCount = 0;
        for (int i = 0; i < 10; i++)
        {
            SpawnEnemy(SpawnLocation());
            waveEnemyCount++;
        }

        waveNumber++;
        GameObject.FindWithTag("Wave Counter").GetComponent<Text>().text = "Wave: " + waveNumber;
    }

    private Vector2 SpawnLocation()
    {
        //TODO: validate Coords on map, change distribution?
        float minRadius = 10f;
        float maxRadius = 25f;

        float xDirUnsigned = Random.Range(minRadius, maxRadius);
        float xDirSigned = Random.Range(0, 2) == 0 ? xDirUnsigned : -xDirUnsigned;

        float yDirUnsigned = Random.Range(minRadius, maxRadius);
        float yDirSigned = Random.Range(0, 2) == 0 ? yDirUnsigned : -yDirUnsigned;

        Vector2 directionVector = new Vector2(xDirSigned, yDirSigned);

        //directionVector = directionVector.normalized;
        //float magnitude = Random.Range(halfWidth, halfWidth * 4);

        //return directionVector * magnitude;
        return directionVector;
    }

    public void RegisterEnemy(GameObject newEnemy)
    {
        enemiesActive.Add(newEnemy);
    }
    public void RemoveEnemy(GameObject deadEnemy)
    {
        enemiesActive.Remove(deadEnemy);
    }
    public void SpawnEnemy(Vector3 position)
    {
        GameObject newEnemy = Instantiate(enemy, position, Quaternion.identity); ;
        int seed = Random.Range(0, 100);
        if (seed <= 55)
        {
            newEnemy.GetComponent<Enemy>().enemyType = Enemies.CHARGER;
        }
        else
        {
            newEnemy.GetComponent<Enemy>().enemyType = Enemies.GUNNER;
        }
    }
}
