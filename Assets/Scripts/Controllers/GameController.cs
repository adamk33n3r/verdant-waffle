using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject laserPrefab;

    private static GameController _instance;
    public static GameController instance { get { return GameController._instance; } }

    void Awake() {
        GameController._instance = this;
    }

    void Start () {
        Debug.Log("Welcome to Dungeons in SPAAAACE!");
        SpawnPlayer();
        SpawnEnemy();
    }

    void SpawnPlayer() {
        GameController.CreatePlayer();
    }

    void SpawnEnemy() {
        GameController.CreateEnemy(Random.Range(-10, 10), Random.Range(-10, 10), 10, 100);
    }

    void Update () {
        // Close if escape is pushed
        if (Input.GetKeyDown("escape")) {
            Application.Quit();

        // Spawn enemy on space
        } else if (Input.GetKeyDown("space")) {
            SpawnEnemy();
        }
    }

    /* Utility Functions */

    public static PlayerController CreatePlayer() {
        PlayerController playerController = CreateEntity(GameController.instance.playerPrefab, 0, 0).GetComponent<PlayerController>();
        playerController.Initialize(GameController.instance.laserPrefab, 100, 100);
        return playerController;
    }

    public static EnemyController CreateEnemy(float x, float y, float startingHealth, float maxHealth) {
        EnemyController enemyController = CreateEntity(GameController.instance.enemyPrefab, x, y).GetComponent<EnemyController>();
        enemyController.Initialize(GameController.instance.laserPrefab, startingHealth, maxHealth);
        return enemyController;
    }

    public static GameObject CreateEntity(GameObject prefab, float x, float y) {
        return Instantiate(prefab, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
    }
}
