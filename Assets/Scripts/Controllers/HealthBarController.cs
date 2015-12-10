using UnityEngine;
using System.Collections;

public class HealthBarController : MonoBehaviour {

    private RectTransform health;

    private Vector3 startingPos;

    private void Start() {
        this.health = this.transform.GetChild(0).GetComponent<RectTransform>();
        this.startingPos = this.health.localPosition;
    }

    public void SetHealthBar(float amt) {
        this.health.localPosition = new Vector3(this.startingPos.x, this.startingPos.y - (100 - amt), this.startingPos.z);
    }
}
