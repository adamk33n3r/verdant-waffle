using UnityEngine;
using System.Collections.Generic;

public class TriShot : Weapon {
    protected override void Awake() {
        this.ammoPoolSize = 120;
        base.Awake();
    }

    public override bool Fire(float angle, float speed) {
        if (Time.time > this.nextFire) {
            List<GameObject> newLasers = new List<GameObject>(3);
            for (int i = 0; i < this.ammoPool.Count; i++) {
                GameObject newLaser = this.ammoPool[i].gameObject;
                // Fire laser if one exists
                if (!newLaser.activeInHierarchy) {
                    newLasers.Add(newLaser);
                }
                if (newLasers.Count == 3) {
                    break;
                }
            }

            if (newLasers.Count > 1) {
                this.firing = true;
                CancelInvoke("SetNotFire");
                Invoke("SetNotFire", 1 / this.fireRate + 0.05f);
                this.nextFire = Time.time + 1 / this.fireRate;
            } else {
                return false;
            }

            for (int i = 0; i < newLasers.Count; i++) {
                GameObject newLaser = newLasers[i];
                // Set position and rotation to be the same as the ship
                newLaser.transform.position = this.transform.position;
                newLaser.transform.rotation = this.ship.spriteTransform.rotation;

                if (i == 0) {
                    newLaser.transform.Rotate(new Vector3(0, 0, -5));
                } else if (i == 2) {
                    newLaser.transform.Rotate(new Vector3(0, 0, 5));
                }

                // Enable the laser
                newLaser.SetActive(true);

                // Get the rigid body and add velocity
                Rigidbody2D rigidBody = newLaser.GetComponent<Rigidbody2D>();
                rigidBody.AddForce(newLaser.transform.rotation * Vector2.up * this.ammoSpeed);
            }
            return true;
        }
        return false;
    }
}
