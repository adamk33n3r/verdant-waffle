using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
    public int ammoCount = 3;
    public int ammoSpeed = 100;
    public int ammoSpread = 60;
    public int fireRate = 10;
    public int currentWeapon = 1;
    public PlayerController player;
    public GameObject bulletPoolObj;

    public Texture2D cursorTex;

    public IDictionary<string, GameObject> shipPrefabs;
    public IDictionary<string, GameObject> weaponPrefabs;
    public IDictionary<string, GameObject> ammoPrefabs;

    public BaseGameObject CreateObject(GameObject prefab, IDictionary<string, object> args = null) {
        return CreateObject(prefab, Vector3.zero, Quaternion.identity, args);
    }

    public BaseGameObject CreateObject(GameObject prefab, Vector3 pos, Quaternion rot, IDictionary<string, object> args = null) {
        // Instantiate and get attached script
        BaseGameObject obj = (Instantiate(prefab, pos, rot) as GameObject).GetComponent<BaseGameObject>();

        // If it has a script, call Initialize with args
        if (obj) {
            obj.Initialize(args);
        } else {
            Debug.LogError("Object does not have a script attached");
        }
        return obj;
    }

    private GameObject[] LoadPrefabs(string dir) {
        return Resources.LoadAll<GameObject>("Prefabs/" + dir);
    }

    void Awake() {
        // This should be initialized to 1...idk
        this.currentWeapon = 1;
        this.bulletPoolObj = GameObject.Find("BulletPool");

        //Cursor.SetCursor(this.cursorTex, Vector2.zero, CursorMode.ForceSoftware);
        Debug.Log("Loading prefabs");
        Debug.Log("Ships");
        GameObject[] shipPrefabs = LoadPrefabs("Ships");
        this.shipPrefabs = new Dictionary<string, GameObject>(shipPrefabs.Length);
        foreach (GameObject p in shipPrefabs) {
            Debug.Log("-" + p.name);
            this.shipPrefabs[p.name] = p;
        }
        Debug.Log("Weapons");
        GameObject[] weaponPrefabs = LoadPrefabs("Weapons");
        this.weaponPrefabs = new Dictionary<string, GameObject>(weaponPrefabs.Length);
        foreach (GameObject p in weaponPrefabs) {
            Debug.Log("-" + p.name);
            this.weaponPrefabs[p.name] = p;
        }
        Debug.Log("Ammo");
        GameObject[] ammoPrefabs = LoadPrefabs("Ammo");
        this.ammoPrefabs = new Dictionary<string, GameObject>(ammoPrefabs.Length);
        foreach (GameObject p in ammoPrefabs) {
            Debug.Log("-" + p.name);
            this.ammoPrefabs[p.name] = p;
        }
    }

    void Start () {
        Debug.Log("Welcome to Dungeons in SPAAAACE!");
        SpawnPlayer();
        //SpawnEnemy();
    }

    void SpawnPlayer() {
        this.player = CreatePlayer();
    }

    void SpawnEnemy() {
        CreateEnemy(Random.Range(-10, 10), Random.Range(-10, 10), 100, 100);
    }

    void Update () {
        // Close if escape is pushed
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();

        // Spawn enemy on space
        } else if (Input.GetKeyDown(KeyCode.Space)) {
            SpawnEnemy();
        } else if (Input.GetKeyDown(KeyCode.Tab)) {
            if (this.currentWeapon == 1) {
                this.player.SwitchWeapon(1);
                this.currentWeapon = 2;
            } else {
                this.player.SwitchWeapon(0);
                this.currentWeapon = 1;
            }
        } else if (Input.GetKeyDown(KeyCode.PageUp)) {
            this.ammoCount++;
        } else if (Input.GetKeyDown(KeyCode.PageDown)) {
            this.ammoCount--;
        } else if (Input.GetKey(KeyCode.Home)) {
            this.ammoSpread++;
        } else if (Input.GetKey(KeyCode.End)) {
            this.ammoSpread--;
        } else if (Input.GetKey(KeyCode.Plus) || Input.GetKey(KeyCode.KeypadPlus)) {
            this.ammoSpeed++;
        } else if (Input.GetKey(KeyCode.Minus) || Input.GetKey(KeyCode.KeypadMinus)) {
            this.ammoSpeed--;
        } else if (Input.GetKey(KeyCode.RightShift)) {
            if (Input.GetKeyDown(KeyCode.Period)) {
                this.fireRate++;
            } else if (Input.GetKeyDown(KeyCode.Comma)) {
                this.fireRate--;
            }
        }
    }

    /* Utility Functions */

    private PlayerController CreatePlayer() {
        PlayerController playerController = CreateShip(this.shipPrefabs["Player"], 0, 0, 100, 100).GetComponent<PlayerController>();
        return playerController;
    }

    private EnemyController CreateEnemy(float x, float y, float startingHealth, float maxHealth) {
        EnemyController enemyController = CreateShip(this.shipPrefabs["Enemy"], x, y, startingHealth, maxHealth).GetComponent<EnemyController>();
        return enemyController;
    }

    private BaseGameObject CreateShip(GameObject prefab, float x, float y, float startingHealth, float maxHealth) {
        return CreateObject(prefab, new Vector3(x, y, 0), Quaternion.identity, new Dictionary<string, object> {
            { "startingHealth", startingHealth },
            { "maxHealth", maxHealth },
        }) as BaseGameObject;
    }
}
