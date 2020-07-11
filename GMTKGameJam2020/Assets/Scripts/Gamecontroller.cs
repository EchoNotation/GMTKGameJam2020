using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    void Update()
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
        Instantiate(enemy, SpawnLocation(), Quaternion.identity);
        waveEnemyCount = enemiesActive.Count / ((enemiesActive.Count - 1) / waveEnemyCount);
        trickling = false;
    }
    private void SpawnNextWave()
    {
        waveEnemyCount = 0;
        for (int i = 0; i < 10; i++)
        {
            waveEnemyCount++;
            Instantiate(enemy, SpawnLocation(), Quaternion.identity);
        }

        waveNumber++;
        Debug.Log("Wave " + waveNumber + " Spawned");
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
        float magnitude = Random.Range(halfWidth, halfWidth * 6);

        return directionVector * magnitude;
    }

    public void RegisterEnemy(GameObject newEnemy)
    {
        enemiesActive.Add(newEnemy);
    }
    public void RemoveEnemy(GameObject deadEnemy)
    {
        enemiesActive.Remove(deadEnemy);
    }

    public void Pause()
    {
        
    }

    public void Play()
    {
    }

    public void SpawnEnemy(Enemies type, Vector3 position)
    {
        GameObject newEnemy = Instantiate(enemy, position, Quaternion.identity);
        newEnemy.GetComponent<Enemy>().enemyType = type;
    }
}
