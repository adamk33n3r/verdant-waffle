using UnityEngine;
using System.Collections.Generic;

public class PlayerController : BaseShipController {

    private HealthBarController healthBar;

    /* Pseudo Constructor */

    public override void Initialize(GameObject laserPrefab, float currentHealth, float maxHealth) {
        base.Initialize(laserPrefab, currentHealth, maxHealth);
        this.acceleration = 20f;
        this.firingAcceleration = 10f;
        this.maxSpeed = 5f;
        this.firingMaxSpeed = 3f;
        this.rotSpeed = 5f;
        this.firingRotSpeed = 3f;
        this.laserSpeed = 500f;
        this.fireRate = 20f;
        this.laserPoolSize = 120;
    }

    protected void Awake() {
        this.healthBar = GameObject.FindGameObjectWithTag(Tags.Health).GetComponent<HealthBarController>();
    }

    protected override void Start() {
        base.Start();
    }

    void FixedUpdate() {
        // Get mouse position
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // Rotate the ship
        RotateTo(mousePos);

        // Move the ship
        Move();
    }

    protected override void Update() {
        base.Update();
        // Firin' mah lazor
        if (Input.GetButton("Fire1")) {
            ShootLaser();
        } else {
            this.firing = false;
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
    
    /* Temporary until weapons are implemented to handle this */
    protected virtual void ShootLaser() {
        if (Time.time > this.nextFire) {
            this.firing = true;
            List<GameObject> newLasers = new List<GameObject>(3);
            for (int i = 0; i < this.laserPool.Count; i++) {
                GameObject newLaser = this.laserPool[i];
                // Fire laser if one exists
                if (!newLaser.activeInHierarchy) {
                    newLasers.Add(newLaser);
                }
                if (newLasers.Count == 3) {
                    break;
                }
            }

            if (newLasers.Count > 1) {
                this.nextFire = Time.time + 1 / this.fireRate;
            }

            for (int i = 0; i < newLasers.Count; i++) {
                GameObject newLaser = newLasers[i];
                // Set position and rotation to be the same as the ship
                newLaser.transform.position = this.transform.position;
                newLaser.transform.rotation = this.spriteTransform.rotation;

                if (i == 0) {
                    newLaser.transform.Rotate(new Vector3(0, 0, -20));
                } else if (i == 2) {
                    newLaser.transform.Rotate(new Vector3(0, 0, 20));
                }

                // Enable the laser
                newLaser.SetActive(true);

                // Get the rigid body and add velocity
                Rigidbody2D rigidBody = newLaser.GetComponent<Rigidbody2D>();
                rigidBody.AddForce(newLaser.transform.rotation * Vector2.up * this.laserSpeed);
            }
        }
    }
}
