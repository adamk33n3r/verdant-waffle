using UnityEngine;
using System.Collections.Generic;

public abstract class BaseGameObject : MonoBehaviour {

    protected GameController gameController;
    protected Color debugColor = Color.white;

    /* Utility Functions */

    /*
     * Call this after creating object.
     * Will be run after Awake but before Start.
     * Useful to pass params for init.
     */
    public virtual void Initialize(IDictionary<string, object> args = null) { }

    protected virtual void OnDrawGizmos() {
        Gizmos.color = this.debugColor;
        CircleCollider2D circleCollider = GetComponent<CircleCollider2D>();
        if (circleCollider) {
            Gizmos.matrix = this.transform.localToWorldMatrix;
            Gizmos.DrawWireSphere(circleCollider.offset, circleCollider.radius);
        }

        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider) {
            Gizmos.matrix = this.transform.localToWorldMatrix;
            Gizmos.DrawWireCube(boxCollider.offset, boxCollider.size);
        }
    }

    /* Unity Functions */

    protected virtual void Awake() {
        this.gameController = FindObjectOfType<GameController>();
    }

    protected virtual void Start() { }
    protected virtual void Update() { }
    protected virtual void FixedUpdate() { }
}
