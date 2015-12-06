using UnityEngine;
using System.Collections;

public class EnemyController : BaseShipController {

    private Transform player;

    protected override void Start() {
        base.Start();
        // Get the players transform
        this.player = GameObject.Find("Player").transform;
        Debug.Log("I am awake!");
    }

    void Update() {
        RotateTo(player.position);
        ShootLaser();
    }

    void FixedUpdate() {
        float distance = (player.position - this.transform.position).magnitude;
        if (distance > 1) {
            MoveToward(player.position, this.acceleration);
        }
    }
}
