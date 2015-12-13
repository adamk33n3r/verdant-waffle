using UnityEngine;
using System.Collections;

public class Missile : Ammo {
    private void BlastOff() {
        this.rigidBody2D.AddForce(this.transform.rotation * Vector2.up * this.speed);
    }

    public override void Fire() {
        this.rigidBody2D.velocity = Vector2.zero;
        this.rigidBody2D.AddForce(this.transform.rotation * Vector2.up * 50f);
    }

    /* Unity Functions */

    protected override void Awake() {
        base.Awake();
        this.damage = 50f;
        this.speed = 100f;
        this.gameObject.layer = LayerMask.NameToLayer(this.transform.parent.parent.tag + "Missile");
    }

    protected override void OnEnable() {
        base.OnEnable();
        Invoke("BlastOff", 0.5f);
    }

    protected override void OnTriggerEnter2D(Collider2D collider) {
        // We collided with another bullet. Destroy each other.
        if (collider.gameObject.tag == "Bullet") {
            if ((this.transform.parent.tag == "Player" && collider.gameObject.transform.parent.tag == "Enemy") || (this.transform.parent.tag == "Enemy" && collider.gameObject.transform.parent.tag == "Player")) {
                Disable();
                collider.gameObject.SetActive(false);
            }

        // We hit a ship. Damage them.
        } else if ((this.transform.parent.tag == "Player" && collider.gameObject.tag == "Enemy") || (this.transform.parent.tag == "Enemy" && collider.gameObject.tag == "Player")) {
            collider.gameObject.SendMessage("Hit", -this.damage);
            Disable();
        }
    }
}
