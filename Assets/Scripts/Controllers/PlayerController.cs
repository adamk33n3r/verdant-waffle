using UnityEngine;
using System.Collections.Generic;

public class PlayerController : BaseShipController {

    float currentLerpPerc = 0f;
    Color initialColor;
    Color damagedColor;
    bool hit = false;

    protected override void Start() {
        base.Start();
        this.initialColor = this.highlightRenderer.color;
        this.damagedColor = new Color(244/255f, 30/255f, 30/255f);
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
        if (this.hit) {
            ResetColor();
        }

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
        this.highlightRenderer.color = this.damagedColor;
        this.currentLerpPerc = 0f;
        this.hit = true;
    }

    void ResetColor() {
        this.currentLerpPerc += Time.deltaTime / 0.25f;
        if (this.currentLerpPerc > 1f) {
            this.currentLerpPerc = 1f;
        }
        this.highlightRenderer.color = Color.Lerp(this.damagedColor, this.initialColor, this.currentLerpPerc);
        if (this.highlightRenderer.color == this.initialColor) {
            this.highlightRenderer.color = this.initialColor;
            this.currentLerpPerc = 0f;
            this.hit = false;
            ;
        }
    }
}
