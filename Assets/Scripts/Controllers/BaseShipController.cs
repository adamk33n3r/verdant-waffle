using UnityEngine;
using System.Collections.Generic;
using Weapon;

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
            return this.activeWeapon.isFiring ? this.firingAcceleration : this.acceleration;
        }
    }

    private float MaxSpeed {
        get {
            return this.activeWeapon.isFiring ? this.firingMaxSpeed : this.maxSpeed;
        }
    }

    private float RotSpeed {
        get {
            return this.activeWeapon.isFiring ? this.firingRotSpeed : this.rotSpeed;
        }
    }

    // Weapons
    protected List<AbstractWeapon> weapons;
    protected AbstractWeapon activeWeapon;

    // Color hit
    float currentColorLerpPerc = 0f;
    Color initialColor;
    protected Color damagedColor;
    bool hit = false;

    public float maxHealth;
    public float currentHealth;

    protected SpriteRenderer highlightRenderer;

    /* Pseudo Constructor */
    public override void Initialize(IDictionary<string, object> args) {
        this.currentHealth = (float)args["startingHealth"];
        this.maxHealth = (float)args["maxHealth"];
    }

    public AbstractWeapon AddWeapon(AbstractWeapon weapon) {
        weapon.transform.parent = this.transform;
        weapon.transform.position = this.transform.position;
        weapon.transform.rotation = this.transform.rotation;
        this.weapons.Add(weapon);
        if (this.activeWeapon == null) {
            this.activeWeapon = weapon;
        }
        return weapon;
    }

    public AbstractWeapon SwitchWeapon(int idx) {
        this.activeWeapon = this.weapons[idx];
        return this.activeWeapon;
    }

    /* Unity Functions */

    protected override void Awake() {
        base.Awake();
        this.debugColor = Color.yellow;
        this.weapons = new List<AbstractWeapon>(1);
    }

    protected override void Start() {
        this.rigidBody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        
        // Get the transform of the objects holding the sprites
        this.highlightRenderer = this.transform.Find("Sprites/ShipHighlights").GetComponent<SpriteRenderer>();

        this.initialColor = this.highlightRenderer.color;
        this.damagedColor = Color.white;
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
        this.GetComponent<Rigidbody2D>().AddForce(force * this.Acceleration);
        if (this.GetComponent<Rigidbody2D>().velocity.magnitude > this.MaxSpeed) {
            this.GetComponent<Rigidbody2D>().velocity = this.GetComponent<Rigidbody2D>().velocity.normalized * this.MaxSpeed;
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
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(new Vector3(0, 0, angle - 90)), Time.deltaTime * this.RotSpeed);
    }

    public virtual void FireWeapon() {
        this.activeWeapon.Fire(1, 0);
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
