using UnityEngine;
using System.Collections;

namespace Weapon {
    namespace Ammo {
        public class Missile : AbstractAmmo {
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
            }

            protected override void OnEnable() {
                base.OnEnable();
                Invoke("BlastOff", 0.5f);
            }

            protected override void OnTriggerEnter2D(Collider2D collider) {
                // We collided with another bullet. Destroy each other.
                if (collider.gameObject.tag == "Bullet") {
                    //Disable();
                    collider.gameObject.SetActive(false);
                } else {
                    collider.gameObject.SendMessage("Hit", -this.damage);
                    Disable();
                }
            }
        }
    }
}
