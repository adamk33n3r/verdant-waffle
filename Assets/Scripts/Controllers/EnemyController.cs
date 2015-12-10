using UnityEngine;
using System.Collections;

public class EnemyController : BaseShipController {

    private Transform player;
    private float currentRotLerpPerc;

    /* Pseudo Constructor */

    public override void Initialize(GameObject laserPrefab, float currentHealth, float maxHealth) {
        base.Initialize(laserPrefab, currentHealth, maxHealth);
        this.acceleration = 5f;
        this.firingAcceleration = 5f;
        this.maxSpeed = 1f;
        this.firingMaxSpeed = 1f;
        this.rotSpeed = 5f;
        this.firingRotSpeed = 5f;
        this.laserSpeed = 500f;
        this.fireRate = 10f;
        this.laserPoolSize = 20;
    }

    protected override void Start() {
        base.Start();
        // Get the players transform
        this.player = GameObject.FindGameObjectWithTag(Tags.Player).transform;
        Debug.Log("I am awake!");
    }

    protected override void Update() {
        base.Update();
        RotateTo(this.player.position);
        ShootLaser();
    }

    void FixedUpdate() {
        float distance = (player.position - this.transform.position).magnitude;
        if (distance > 1) {
            MoveToward(player.position);
        }
    }
}
