using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    public GameObject enemyPrefab;

    void Start () {
        Debug.Log("Welcome to Dungeons in SPAAAACE!");
        SpawnEnemy();
    }

    void SpawnEnemy() {
        Instantiate(this.enemyPrefab, new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), 0), Quaternion.identity);
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
}
