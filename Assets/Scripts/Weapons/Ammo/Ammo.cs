using UnityEngine;
using System.Collections;

public class Ammo : BaseGameObject {
    protected float damage = 1f;
    protected float speed = 100f;
    public Vector2 currentVelocity;

    /* Properties */
    public float Speed { get { return this.speed; } }

    public virtual void Fire() {
        this.rigidBody2D.velocity = Vector2.zero;
        this.rigidBody2D.AddForce(this.transform.rotation * Vector2.up * this.speed);
    }

    protected override void Awake() {
        base.Awake();
        this.gameObject.layer = LayerMask.NameToLayer(this.transform.parent.parent.tag);
    }

    protected override void Update() {
        this.speed = this.gameController.ammoSpeed;
        this.currentVelocity = this.rigidBody2D.velocity;
    }

    protected virtual void OnEnable() {
        // Disable lasers after 2 seconds
        Invoke("Disable", 2f);
    }

    protected virtual void OnDisable() {
        // Cancel invoke if disabled (if hit something)
        CancelInvoke("Disable");
    }

    protected virtual void Disable() {
        // Set the laser to inactive to be allowed back in pool
        this.gameObject.SetActive(false);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collider) {
        Debug.Log(this.transform.parent.tag + "'s " + this.transform.tag + " hit " + collider.gameObject.tag);
    }
}
