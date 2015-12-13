using UnityEngine;
using System.Collections.Generic;
using Weapon;

public class EnemyController : BaseShipController {

    private Transform player;
    private float currentRotLerpPerc;

    /* Pseudo Constructor */

    public override void Initialize(IDictionary<string, object> args) {
        base.Initialize(args);
        this.acceleration = 5f;
        this.firingAcceleration = 5f;
        this.maxSpeed = 1f;
        this.firingMaxSpeed = 1f;
        this.rotSpeed = 5f;
        this.firingRotSpeed = 5f;

        /*this.laserSpeed = 500f;
        this.fireRate = 10f;
        this.laserPoolSize = 20;*/
    }

    protected override void Start() {
        base.Start();
        // Get the players transform
        this.player = GameObject.FindGameObjectWithTag(Tags.Player).transform;
        AddWeapon(this.gameController.CreateObject(this.gameController.weaponPrefabs["Projectile"], new Dictionary<string, object> {
            { "ammoPrefab", this.gameController.ammoPrefabs["Laser"] },
            { "ship", this }
        }) as AbstractWeapon);
    }

    protected override void Update() {
        base.Update();
        RotateTo(this.player.position);
        FireWeapon();
    }

    protected override void FixedUpdate() {
        float distance = (player.position - this.transform.position).magnitude;
        if (distance > 1) {
            MoveToward(player.position);
        }
    }

    public override void FireWeapon() {
        this.activeWeapon.Fire(1, 0);
    }
}
