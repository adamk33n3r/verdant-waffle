using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

    public float rotSpeed = 5f;

    private Transform spriteTransform;
    private Transform player;

	void Start () {
        // Get the transform of the objects holding the sprites
        this.spriteTransform = this.transform.Find("Sprites").transform;
        
        // Get the players transform
        this.player = GameObject.Find("Player").transform;
        Debug.Log("I am awake!");
	}
	
	void Update () {
        Rotate();
	}

    void Rotate() {
        // Calculate the diff
        Vector3 diff = this.transform.position - player.position;

        // Use arctangent to get the angle between ship and player
        float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

        // Set the ships rotation to face the player
        this.spriteTransform.rotation = Quaternion.Lerp(this.spriteTransform.rotation, Quaternion.Euler(new Vector3(0, 0, angle + 90)), Time.deltaTime * this.rotSpeed);
    }
}
