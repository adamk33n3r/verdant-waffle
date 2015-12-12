using UnityEngine;
using System.Collections.Generic;

public class PlayerController : BaseShipController {

    private HealthBarController healthBar;

    /* Pseudo Constructor */

    public override void Initialize(IDictionary<string, object> args) {
        base.Initialize(args);
        this.acceleration = 20f;
        this.firingAcceleration = 10f;
        this.maxSpeed = 5f;
        this.firingMaxSpeed = 3f;
        this.rotSpeed = 5f;
        this.firingRotSpeed = 3f;
        /*this.laserSpeed = 350f;
        this.fireRate = 15f;
        this.laserPoolSize = 120;*/
    }

    /* Unity Functions */

    protected override void Awake() {
        base.Awake();
        this.healthBar = GameObject.FindGameObjectWithTag(Tags.Health).GetComponent<HealthBarController>();
    }

    protected override void Start() {
        base.Start();
        AddWeapon(this.gameController.CreateObject(this.gameController.weaponPrefabs["TriShot"], new Dictionary<string, object> {
            { "ammoPrefab", this.gameController.laserPrefab },
            { "ship", this }
        }) as Weapon);
    }

    protected override void FixedUpdate() {
        // Get mouse position
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // Rotate the ship
        RotateTo(mousePos);

        // Move the ship
        //Debug.Log(this.activeWeapon.isFiring);
        Move();
    }

    protected override void Update() {
        base.Update();
        // Firin' mah lazor
        if (Input.GetButton("Fire1")) {
            ShootLaser();
        } else {
            //this.firing = false;
        }
    }

    void Move() {
        // Get movement values
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        MoveForce(new Vector2(horizontal, vertical));
    }

    protected override void UpdateHealth(float amt) {
        base.UpdateHealth(amt);
        this.healthBar.SetHealthBar(this.currentHealth);
    }

    protected override void Kill() {
        this.gameObject.SetActive(false);
    }
    
}
