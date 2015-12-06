using UnityEngine;
using System.Collections.Generic;

public class BaseShipController : MonoBehaviour {
    public GameObject laserPrefab;

    public float acceleration = 20f;
    public float maxSpeed = 5f;
    public float rotSpeed = 5f;
    public float laserSpeed = 500f;
    public float fireRate = 0.1f;
    public int laserPoolSize = 20;

    protected Transform spriteTransform;
    protected SpriteRenderer highlightRenderer;
    protected new Rigidbody2D rigidbody2D;
    protected float nextFire = 0f;

    private List<GameObject> laserPool;

    protected virtual void Start() {
        this.rigidbody2D = GetComponent<Rigidbody2D>();
        
        // Get the transform of the objects holding the sprites
        this.spriteTransform = this.transform.Find("Sprites").transform;
        this.highlightRenderer = this.transform.Find("Sprites/ShipHighlights").GetComponent<SpriteRenderer>();

        // Initialize the laser pool
        this.laserPool = new List<GameObject>(this.laserPoolSize);
        for (int i = 0; i < this.laserPoolSize; i++) {
            GameObject laser = Instantiate(this.laserPrefab);
            laser.transform.parent = this.transform;
            this.laserPool.Add(laser);
        }
    }

    protected void MoveTranslate(Vector2 dir) {
        // Move the ship in direction
        this.transform.Translate(dir);
    }

    protected void MoveForce(Vector2 force) {
        this.rigidbody2D.AddForce(force);
        if (this.rigidbody2D.velocity.magnitude > this.maxSpeed) {
            this.rigidbody2D.velocity = this.rigidbody2D.velocity.normalized * this.maxSpeed;
        }
    }

    protected void MoveToward(Vector3 position, float speed) {
        Vector3 heading = position - this.transform.position;
        MoveForce(heading.normalized * speed);
    }

    protected void RotateTo(Vector3 position) {
        // Use screen positions to calculate the heading
        Vector3 heading = position - this.transform.position;

        // Use arctangent to get the angle between ship and mouse
        float angle = Mathf.Atan2(heading.y, heading.x) * Mathf.Rad2Deg;

        // Set the ships rotation to face the mouse
        this.spriteTransform.rotation = Quaternion.Lerp(this.spriteTransform.rotation, Quaternion.Euler(new Vector3(0, 0, angle - 90)), this.rotSpeed);
    }

    protected void ShootLaser() {
        if (Time.time > this.nextFire) {
            for (int i = 0; i < this.laserPool.Count; i++) {
                GameObject newLaser = this.laserPool[i];

                // Fire laser if one exists
                if (!newLaser.activeInHierarchy) {
                    this.nextFire = Time.time + this.fireRate;

                    // Set position and rotation to be the same as the ship
                    newLaser.transform.position = this.transform.position;
                    newLaser.transform.rotation = this.spriteTransform.rotation;

                    // Enable the laser
                    newLaser.SetActive(true);

                    // Get the rigid body and add velocity
                    Rigidbody2D rigidBody = newLaser.GetComponent<Rigidbody2D>();
                    rigidBody.AddForce(newLaser.transform.rotation * Vector2.up * this.laserSpeed);
                    break;
                }
            }
        }
    }
}
