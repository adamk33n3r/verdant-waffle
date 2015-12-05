using UnityEngine;
using System.Collections;

public class DefaultLaser : MonoBehaviour {

    void OnEnable() {
        // Disable lasers after 2 seconds
        Invoke("Disable", 2f);
    }

    void OnDisable() {
        // Cancel invoke if disabled
        CancelInvoke();
    }

    void Disable() {
        // Set the laser to inactive to be allowed back in pool
        this.gameObject.SetActive(false);
    }
}
