using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamecontroller : MonoBehaviour
{
    private int waveNumber, waveEnemyCount;
    private ArrayList enemiesActive;
    private float halfWidth, waveSpawnThreshold;
    public Camera mainCamera;
    public GameObject Charger, Gunner, Bombmer;
    private int[] waveEnemyMix;
    private bool gamePaused;
    
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        mainCamera = null;
        enemiesActive = new ArrayList();
        waveSpawnThreshold = 0.25f;
        waveEnemyMix = new int[] {3, 0, 0};
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (mainCamera == null) {
            mainCamera = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Camera>();
            if (mainCamera != null) {
                halfWidth = mainCamera.orthographicSize * mainCamera.aspect;
            }
        }
        else if (!gamePaused) {
            Debug.Log("GameC update");
            Debug.Log(enemiesActive.Count / waveEnemyCount < waveSpawnThreshold);
            if (enemiesActive.Count / waveEnemyCount < waveSpawnThreshold) {
                SpawNextWave();
            }
        }
    }

    public void Reset() {
        waveNumber = 0;
    }

    private void SpawNextWave() {
        for (int i = 0; i < 10; i++) {
            Instantiate(Gunner, SpawnLocation(), Quaternion.identity);
            waveEnemyCount++;
        }

        waveNumber++;
    }

    private Vector2 SpawnLocation() {
        //TODO: validate Coords on map, change distribution?

        Vector2 directionVector = Vector2.zero;
        while (directionVector == Vector2.zero) {
            directionVector = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        }

        directionVector = directionVector.normalized;
        float magnitude = Random.Range(halfWidth, halfWidth * 6);

        return directionVector * magnitude;
    }

    public void RegisterEnemy(GameObject newEnemy) {
        Debug.Log("Enemy Registered");
        enemiesActive.Add(newEnemy);
    }
    public void RemoveEnemy(GameObject deadEnemy) {
        enemiesActive.Remove(deadEnemy);
    }

    public void Pause() {
        gamePaused = true;
    }

    public void Play() {
        Debug.Log("Playing");
        gamePaused = false;
    }
}
