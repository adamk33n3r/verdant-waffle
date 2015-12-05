using UnityEngine;
using System.Collections;

public class DefaultLaser : MonoBehaviour {

    void Start() {

    }

    void OnEnable() {
        Invoke("Disable", 2f);
    }

    void Update() {

    }

    void OnDisable() {
        CancelInvoke();
    }

    void Disable() {
        this.gameObject.SetActive(false);
    }
}
