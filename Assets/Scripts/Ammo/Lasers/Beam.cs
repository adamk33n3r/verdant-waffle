using UnityEngine;
using System.Collections;

namespace Weapon {
    namespace Ammo {
        public class Beam : AbstractAmmo {
            protected override void Awake() {
                base.Awake();
                this.damage = 20f;
            }

            protected override void Update() {}
            protected override void OnEnable() {}
            protected override void OnDisable() {}

            public override void Fire() {
                //this.rigidBody2D.velocity = Vector2.zero;
                //this.rigidBody2D.AddForce(this.transform.rotation * Vector2.up * this.speed);
                Debug.Log("beam is fired");
                Invoke("Disable", 2f);
            }

            protected override void OnTriggerEnter2D(Collider2D collider) {
                base.OnTriggerEnter2D(collider);
                if (collider.gameObject.tag == "Bullet") {
                } else {
                // If we hit a ship. Damage them and return to pool.
                //if ((this.transform.tag == "Player" && collider.gameObject.tag == "Enemy") || (this.transform.parent.tag == "Enemy" && collider.gameObject.tag == "Player")) {
                    collider.gameObject.SendMessage("Hit", -this.damage);
                    //Disable();
                }
            }
        }
    }
}
