using UnityEngine;
using System.Collections.Generic;

public class BaseShipController : BaseGameObject {
    // Movement
    public float acceleration = 20f;
    public float firingAcceleration = 10f;
    public float maxSpeed = 5f;
    public float firingMaxSpeed = 1f;
    public float rotSpeed = 5f;
    public float firingRotSpeed = 1f;

    private float Acceleration {
        get {
            return this.firing ? this.firingAcceleration : this.acceleration;
        }
    }

    private float MaxSpeed {
        get {
            return this.firing ? this.firingMaxSpeed : this.maxSpeed;
        }
    }

    private float RotSpeed {
        get {
            return this.firing ? this.firingRotSpeed : this.rotSpeed;
        }
    }

    // Weapons
    protected List<Weapon> weapons;

    // Laser
    // TODO: Refactor
    protected GameObject laserPrefab;
    protected float laserSpeed = 500f;
    protected float fireRate = 10f;
    protected int laserPoolSize = 20;
    protected float nextFire = 0f;
    protected bool firing;

    // Color hit
    float currentColorLerpPerc = 0f;
    Color initialColor;
    protected Color damagedColor;
    bool hit = false;

    protected float maxHealth;
    protected float currentHealth;

    protected Transform spriteTransform;
    protected SpriteRenderer highlightRenderer;
    protected new Rigidbody2D rigidbody2D;

    protected List<GameObject> laserPool;

    /* Pseudo Constrcutor */

    public override void Initialize(IDictionary<string, object> args) {
        this.laserPrefab = args["laserPrefab"] as GameObject;
        this.currentHealth = (float)args["startingHealth"];
        this.maxHealth = (float)args["maxHealth"];
    }

    /* Unity Functions */

    protected override void Awake() {
        base.Awake();
        this.debugColor = Color.yellow;
        this.weapons = new List<Weapon>(1);
    }

    protected override void Start() {
        this.rigidbody2D = GetComponent<Rigidbody2D>();
        this.rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        
        // Get the transform of the objects holding the sprites
        this.spriteTransform = this.transform.Find("Sprites").transform;
        this.highlightRenderer = this.transform.Find("Sprites/ShipHighlights").GetComponent<SpriteRenderer>();

        this.initialColor = this.highlightRenderer.color;
        this.damagedColor = Color.white;

        // Initialize the laser pool
        this.laserPool = new List<GameObject>(this.laserPoolSize);
        for (int i = 0; i < this.laserPoolSize; i++) {
            GameObject laser = Instantiate(this.laserPrefab);
            laser.transform.parent = this.transform;
            laser.layer = LayerMask.NameToLayer(this.tag);
            this.laserPool.Add(laser);
        }
    }

    protected override void Update() {
        if (this.hit) {
            ResetColor();
        }
    }

    /* Custom Functions */

    protected void MoveTranslate(Vector2 dir) {
        // Move the ship in direction
        this.transform.Translate(dir);
    }

    protected void MoveForce(Vector2 force) {
        this.rigidbody2D.AddForce(force * this.Acceleration);
        if (this.rigidbody2D.velocity.magnitude > this.MaxSpeed) {
            this.rigidbody2D.velocity = this.rigidbody2D.velocity.normalized * this.MaxSpeed;
        }
    }

    protected void MoveToward(Vector3 position) {
        Vector3 heading = position - this.transform.position;
        MoveForce(heading.normalized * this.Acceleration);
    }

    protected virtual void RotateTo(Vector3 position) {
        // Calculate the heading
        Vector3 heading = position - this.transform.position;

        // Use arctangent to get the angle between ship and position
        float angle = Mathf.Atan2(heading.y, heading.x) * Mathf.Rad2Deg;

        // Set the ships rotation to face the position
        this.spriteTransform.rotation = Quaternion.Lerp(this.spriteTransform.rotation, Quaternion.Euler(new Vector3(0, 0, angle - 90)), Time.deltaTime * this.RotSpeed);
    }

    protected virtual void ShootLaser() {
        if (Time.time > this.nextFire) {
            this.firing = true;
            for (int i = 0; i < this.laserPool.Count; i++) {
                GameObject newLaser = this.laserPool[i];
                // Fire laser if one exists
                if (!newLaser.activeInHierarchy) {
                    this.nextFire = Time.time + 1/this.fireRate;

                    // Set position and rotation to be the same as the ship
                    newLaser.transform.position = this.transform.position;
                    newLaser.transform.rotation = this.spriteTransform.rotation;

                    // Enable the laser
                    newLaser.SetActive(true);

                    // Get the rigid body and add velocity
                    Rigidbody2D rigidBody = newLaser.GetComponent<Rigidbody2D>();
                    rigidBody.AddForce(newLaser.transform.rotation * Vector2.up * this.laserSpeed);
                    break;
                }
            }
        }
    }

    protected virtual void UpdateHealth(float amt) {
        this.currentHealth += amt;
        if (this.currentHealth < 0) {
            this.currentHealth = 0;
        } else if (this.currentHealth > this.maxHealth) {
            this.currentHealth = this.maxHealth;
        }
    }

    protected virtual void Kill() {
        Destroy(this.gameObject);
    }

    // Messages

    void Hit(float dmg) {
        this.highlightRenderer.color = this.damagedColor;
        this.currentColorLerpPerc = 0f;
        this.hit = true;
        UpdateHealth(dmg);
        if (this.currentHealth <= 0) {
            Kill();
        }
    }

    void ResetColor() {
        this.currentColorLerpPerc += Time.deltaTime / 0.25f;
        if (this.currentColorLerpPerc > 1f) {
            this.currentColorLerpPerc = 1f;
        }
        this.highlightRenderer.color = Color.Lerp(this.damagedColor, this.initialColor, this.currentColorLerpPerc);
        if (this.highlightRenderer.color == this.initialColor) {
            this.highlightRenderer.color = this.initialColor;
            this.currentColorLerpPerc = 0f;
            this.hit = false;
        }
    }
}
