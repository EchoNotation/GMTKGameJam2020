using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gamecontroller : MonoBehaviour
{
    public static Gamecontroller instance = null;
    public int waveNumber;
    private float halfWidth, waveSpawnThreshold, waveEnemyCount;
    private bool tricklActive, waveSpawnActive;
    private ArrayList enemiesActive;
    private Camera mainCamera;
    public GameObject enemy, bombmer;

    public Vector2 powerUpSpawnXBounds = Vector2.zero;
    public Vector2 powerUpSpawnYBounds = Vector2.zero;

    public GameObject[] powerups;

    public int startNumPowerups = 15;

    private bool spawnedPowerups = false;

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
            if (enemiesActive.Count / waveEnemyCount <= waveSpawnThreshold & !waveSpawnActive)
            {
                StartCoroutine(SpawnNextWave());
                SpawnPowerup();
                if (!spawnedPowerups)
                {
                    for (int i = 0; i < startNumPowerups; i++)
                    {
                        SpawnPowerup();
                    }
                    spawnedPowerups = true;
                }
            }
            // if (!tricklActive & !waveSpawnActive)
            // {
            //     StartCoroutine(SpawnTrickle());
            // }
        }
    }

    public void Reset()
    {
        mainCamera = null;
        waveNumber = 0;
        waveEnemyCount = 1;
        StopAllCoroutines();
        tricklActive = false;
        waveSpawnActive = false;
        spawnedPowerups = false;
    }
    private IEnumerator SpawnTrickle()
    {
        tricklActive = true;
        yield return new WaitForSeconds(Random.Range(250, 601) * 0.01f);
        SpawnEnemy(SpawnLocation());
        tricklActive = false;
    }
    private IEnumerator SpawnNextWave()
    {
        waveSpawnActive = true;
        waveEnemyCount = 0;
        waveNumber++;
        GameObject.FindWithTag("Wave Counter").GetComponent<Text>().text = "Wave: " + waveNumber;
        for (int i = 0; i < Mathf.Pow(2, waveNumber); i++)
        {
            if (i % 5 == 0)
            {
                SpawnBomber();
            }
            SpawnEnemy(SpawnLocation());
            waveEnemyCount++;
            yield return new WaitForSeconds(Random.Range(50, 101) * 0.01f);
        }
        waveSpawnActive = false;
    }
    public void SpawnPowerup()
    {
        float x = 0f;
        float y = 0f;
        bool valid = false;
        while (!valid)
        {
            x = Random.Range(powerUpSpawnXBounds.x, powerUpSpawnXBounds.y);
            y = Random.Range(powerUpSpawnYBounds.x, powerUpSpawnYBounds.y);

            //Debug.Log("testing point " + x + " " + y);

            valid = Physics2D.OverlapBox(new Vector2(x, y), new Vector2(1f, 1f), 0f) == null;
        }

        int i = Random.Range(0, powerups.Length);

        Instantiate(powerups[i], new Vector3(x, y, 0), Quaternion.identity);
    }
    private Vector2 SpawnLocation()
    {
        //TODO: validate Coords on map, change distribution?

        Vector2 directionVector = Vector2.zero;
        while (directionVector == Vector2.zero)
        {
            directionVector = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        }

        directionVector = directionVector.normalized;
        float magnitude = Random.Range(halfWidth * 1.5f, halfWidth * 4);

        return directionVector * magnitude + (Vector2)GameObject.FindGameObjectWithTag("Player").transform.position;
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
    public void SpawnBomber()
    {
        //set to whatever
        float radius = 15f;
        Vector3 location = Random.onUnitSphere * radius;
        location.z = 0;

        Instantiate(bombmer, location, Quaternion.identity);
    }
}
