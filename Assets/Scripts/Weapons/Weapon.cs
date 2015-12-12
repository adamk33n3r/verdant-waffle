using UnityEngine;
using System.Collections.Generic;

public abstract class Weapon : BaseGameObject {

    protected BaseShipController ship;

    // Ammo
    protected List<Ammo> ammoPool;
    protected GameObject ammoPrefab;
    protected int ammoPoolSize = 20;
    protected float ammoSpeed = 500f;
    protected float fireRate = 10f;
    protected float nextFire = 0f;
    protected bool firing;
    public bool isFiring { get { return this.firing; } }


    public override void Initialize(IDictionary<string, object> args = null) {
        this.ammoPrefab = args["ammoPrefab"] as GameObject;
        this.ship = args["ship"] as BaseShipController;
    }

    protected override void Awake() {
        base.Awake();
        this.ammoPool = new List<Ammo>(this.ammoPoolSize);
    }

    protected override void Start() {
        // Initialize the laser pool
        for (int i = 0; i < this.ammoPoolSize; i++) {
            Ammo ammo = this.gameController.CreateObject(this.ammoPrefab) as Ammo;
            ammo.transform.parent = this.transform;
            ammo.gameObject.layer = LayerMask.NameToLayer(this.transform.parent.tag);
            this.ammoPool.Add(ammo);
        }
    }

    public virtual bool Fire(float angle, float speed) {
        if (Time.time > this.nextFire) {
            for (int i = 0; i < this.ammoPool.Count; i++) {
                GameObject newAmmo = this.ammoPool[i].gameObject;
                // Fire laser if one exists
                if (!newAmmo.activeInHierarchy) {
                    this.firing = true;
                    CancelInvoke("SetNotFire");
                    Invoke("SetNotFire", 1 / this.fireRate + 0.05f);
                    this.nextFire = Time.time + 1/this.fireRate;

                    // Set position and rotation to be the same as the weapon
                    newAmmo.transform.position = this.transform.position;
                    newAmmo.transform.rotation = this.ship.spriteTransform.rotation;

                    // Enable the laser
                    newAmmo.SetActive(true);

                    // Get the rigid body and add velocity
                    Rigidbody2D rigidBody = newAmmo.GetComponent<Rigidbody2D>();
                    rigidBody.AddForce(newAmmo.transform.rotation * Vector2.up * this.ammoSpeed);
                    return true;
                }
            }
        }
        return false;
    }

    private void SetNotFire() {
        this.firing = false;
    }
}
