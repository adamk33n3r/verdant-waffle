﻿using UnityEngine;
using System.Collections;

public class DefaultLaser : Ammo {

    protected float damage = 1f;
    
    protected override void Awake() {
        if (this.transform.parent.tag == "Enemy") {
            this.damage = 0.1f;
        }
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

    protected override void OnTriggerEnter2D(Collider2D collider) {
        // We collided with another bullet. Destroy each other.
        if (collider.gameObject.tag == "Bullet") {
            /*if ((this.transform.parent.tag == "Player" && collider.gameObject.transform.parent.tag == "Enemy") || (this.transform.parent.tag == "Enemy" && collider.gameObject.transform.parent.tag == "Player")) {
                Disable();
                collider.gameObject.SetActive(false);
            }*/

        // We hit a ship. Damage them.
        } else if ((this.transform.parent.tag == "Player" && collider.gameObject.tag == "Enemy") || (this.transform.parent.tag == "Enemy" && collider.gameObject.tag == "Player")) {
            collider.gameObject.SendMessage("Hit", -this.damage);
            Disable();
        }
    }
}
