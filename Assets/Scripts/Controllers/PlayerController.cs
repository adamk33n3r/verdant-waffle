using UnityEngine;
using System.Collections.Generic;

public class PlayerController : BaseShipController {

    Color initialColor;

    protected override void Start() {
        base.Start();
        this.initialColor = this.highlightRenderer.color;
    }

    void FixedUpdate() {
        // Get mouse position
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // Rotate the ship
        RotateTo(mousePos);

        // Move the ship
        Move();
    }
    void Update() {
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

    // Messages

    void Hit() {
        this.highlightRenderer.color = Color.red;
        CancelInvoke("ResetColor");
        InvokeRepeating("ResetColor", 0f, .1f);
    }

    void ResetColor() {
        this.highlightRenderer.color = Color.Lerp(this.highlightRenderer.color, this.initialColor, .1f);
        if ((this.highlightRenderer.color - this.initialColor).maxColorComponent < .1) {
            this.highlightRenderer.color = this.initialColor;
            CancelInvoke("ResetColor");
        }
    }
}
