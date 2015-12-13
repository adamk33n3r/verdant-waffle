using UnityEngine;
using System.Collections.Generic;
using Weapon;

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
        AddWeapon(this.gameController.CreateObject(this.gameController.weaponPrefabs["DualWield"], new Dictionary<string, object> {
            { "ammoPrefab", this.gameController.ammoPrefabs["Laser"] },
            { "ship", this },
            { "x1", 0.3f },
            { "x2", -0.3f }
        }) as AbstractWeapon);
        AbstractWeapon missle = AddWeapon(this.gameController.CreateObject(this.gameController.weaponPrefabs["Projectile"], new Dictionary<string, object> {
            { "ammoPrefab", this.gameController.ammoPrefabs["Missle"] },
            { "ship", this }
        }) as AbstractWeapon);
        missle.transform.Translate(0, -0.3f, 0);
        foreach (var wep in this.weapons) {
            Debug.Log(wep);
        }
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
            FireWeapon();
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
