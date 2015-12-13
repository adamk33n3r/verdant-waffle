using UnityEngine;
using System.Collections;

namespace Weapon {
    namespace Ammo {
        public class DefaultLaser : AbstractAmmo {
            protected override void Awake() {
                base.Awake();
                if (LayerMask.LayerToName(this.gameObject.layer) == "Enemy") {
                    this.damage = 0.1f;
                }
            }

            protected override void OnTriggerEnter2D(Collider2D collider) {
                if (collider.gameObject.tag == "Bullet") {
                } else {
                // If we hit a ship. Damage them and return to pool.
                //if ((this.transform.tag == "Player" && collider.gameObject.tag == "Enemy") || (this.transform.parent.tag == "Enemy" && collider.gameObject.tag == "Player")) {
                    collider.gameObject.SendMessage("Hit", -this.damage);
                    Disable();
                }
            }
        }
    }
}
