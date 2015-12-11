using UnityEngine;
using System.Collections;

public class TriLaser : DefaultLaser {
    protected override void OnTriggerEnter2D(Collider2D collider) {
        //Debug.Log(this.transform.parent.tag + "'s " + this.transform.tag + " hit " + collider.gameObject.tag);
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
