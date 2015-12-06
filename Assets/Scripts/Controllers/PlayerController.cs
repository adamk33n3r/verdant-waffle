using UnityEngine;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {
    public GameObject laserPrefab;

    public float speed = 5f;
    public float rotSpeed = 5f;
    public float laserSpeed = 500f;
    public float fireRate = 0.1f;
    public int laserPoolSize = 20;

    private float nextFire = 0f;

    private Transform spriteTransform;

    private List<GameObject> laserPool;

    void Start() {
        // Get the transform of the objects holding the sprites
        this.spriteTransform = this.transform.Find("Sprites").transform;

        // Initialize the laser pool
        this.laserPool = new List<GameObject>(this.laserPoolSize);
        for (int i = 0; i < this.laserPoolSize; i++) {
            this.laserPool.Add(Instantiate(this.laserPrefab));
        }
    }

    void Update() {
        // Move the ship
        Move();

        // Rotate the ship
        Rotate();

        // Firin' mah lazor
        if (Input.GetButton("Fire1") && Time.time > this.nextFire) {
            this.nextFire = Time.time + this.fireRate;
            ShootLaser();
        }
    }

    void Move() {
        // Get movement values
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Move the ship according to direction and speed
        this.transform.Translate(new Vector2(horizontal, vertical) * Time.deltaTime * this.speed);
    }

    void Rotate() {
        // Get mouse position
        Vector2 mousePos = Input.mousePosition;

        // Convert world position of ship to screen point
        Vector3 screenPos = Camera.main.WorldToScreenPoint(this.transform.position);

        // Use screen positions to calculate the diff
        float relX = mousePos.x - screenPos.x;
        float relY = mousePos.y - screenPos.y;

        // Use arctangent to get the angle between ship and mouse
        float angle = Mathf.Atan2(relY, relX) * Mathf.Rad2Deg;

        // Set the ships rotation to face the mouse
        this.spriteTransform.rotation = Quaternion.Lerp(this.spriteTransform.rotation, Quaternion.Euler(new Vector3(0, 0, angle - 90)), Time.deltaTime * this.rotSpeed);
    }

    void ShootLaser() {
        for (int i = 0; i < this.laserPool.Count; i++) {
            GameObject newLaser = this.laserPool[i];

            // Fire laser if one exists
            if (!newLaser.activeInHierarchy) {
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
