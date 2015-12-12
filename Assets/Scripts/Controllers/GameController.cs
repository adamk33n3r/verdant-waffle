using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject laserPrefab;

    public BaseGameObject CreateObject(GameObject prefab, IDictionary<string, object> args = null) {
        return CreateObject(prefab, Vector3.zero, Quaternion.identity, args);
    }

    public BaseGameObject CreateObject(GameObject prefab, Vector3 pos, Quaternion rot, IDictionary<string, object> args = null) {
        BaseGameObject obj = (Instantiate(prefab, pos, rot) as GameObject).GetComponent<BaseGameObject>();
        if (obj) {
            obj.Initialize(args);
        }
        return obj;
    }

    void Awake() { }

    void Start () {
        Debug.Log("Welcome to Dungeons in SPAAAACE!");
        SpawnPlayer();
        SpawnEnemy();
    }

    void SpawnPlayer() {
        CreatePlayer();
    }

    void SpawnEnemy() {
        CreateEnemy(Random.Range(-10, 10), Random.Range(-10, 10), 10, 100);
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

    private PlayerController CreatePlayer() {
        PlayerController playerController = CreateShip(this.playerPrefab, 0, 0, this.laserPrefab, 100, 100).GetComponent<PlayerController>();
        return playerController;
    }

    private EnemyController CreateEnemy(float x, float y, float startingHealth, float maxHealth) {
        EnemyController enemyController = CreateShip(this.enemyPrefab, x, y, this.laserPrefab, startingHealth, maxHealth).GetComponent<EnemyController>();
        return enemyController;
    }

    private BaseGameObject CreateShip(GameObject prefab, float x, float y, GameObject laserPrefab, float startingHealth, float maxHealth) {
        return CreateObject(prefab, new Vector3(x, y, 0), Quaternion.identity, new Dictionary<string, object> {
            { "startingHealth", startingHealth },
            { "laserPrefab", laserPrefab },
            { "maxHealth", maxHealth },
        }) as BaseGameObject;
    }
}
