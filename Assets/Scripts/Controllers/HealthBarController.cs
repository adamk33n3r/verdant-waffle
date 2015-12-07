using UnityEngine;
using System.Collections;

public class HealthBarController : MonoBehaviour {

    private RectTransform health;
    private RectTransform rectTransform;

    private Vector3 startingPos;

    private void Start() {
        this.rectTransform = this.transform.GetComponent<RectTransform>();
        this.health = this.transform.GetChild(0).GetComponent<RectTransform>();
        this.startingPos = this.health.localPosition;
    }

    public void SetHealthBar(float amt) {
        //Debug.Log(amt);
        this.health.localPosition = new Vector3(this.startingPos.x, this.startingPos.y - (100 - amt), this.startingPos.z);
        //Debug.Log(this.transform.position);
        //this.health.position = new Vector3(0, 50 + amt, 0);
    }
}
