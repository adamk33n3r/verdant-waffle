using UnityEngine;
using System.Collections;

public class Ammo : BaseGameObject {
    protected virtual void OnTriggerEnter2D(Collider2D collider) {
        Debug.Log(this.transform.parent.tag + "'s " + this.transform.tag + " hit " + collider.gameObject.tag);
    }
}
