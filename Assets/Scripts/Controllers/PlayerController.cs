using UnityEngine;
using System.Collections.Generic;

public class PlayerController : BaseShipController {

    public HealthBarController healthBar;

    protected override void Start() {
        base.Start();
    }

    void FixedUpdate() {
        // Get mouse position
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // Rotate the ship
        RotateTo(mousePos);

        // Move the ship
        Move();
    }

    protected override void Update() {
        base.Update();
        // Firin' mah lazor
        if (Input.GetButton("Fire1")) {
            ShootLaser();
        }
    }

    void Move() {
        // Get movement values
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        MoveForce(new Vector2(horizontal, vertical) * this.acceleration);
    }

    protected override void UpdateHealth(float amt) {
        base.UpdateHealth(amt);
        this.healthBar.SetHealthBar(this.currentHealth);
    }

    protected override void Kill() {
        this.gameObject.SetActive(false);
    }

}
