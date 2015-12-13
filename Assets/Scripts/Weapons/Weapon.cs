using UnityEngine;
using System.Collections.Generic;

public abstract class Weapon : BaseGameObject {

    protected BaseShipController ship;

    // Ammo
    protected List<Ammo> ammoPool;
    protected GameObject ammoPrefab;
    protected int ammoPoolSize = 120;
    protected float fireRate = 10f;
    protected float nextFire = 0f;
    protected bool firing;
    public bool isFiring { get { return this.firing; } }


    public override void Initialize(IDictionary<string, object> args = null) {
        this.ammoPrefab = args["ammoPrefab"] as GameObject;
        this.ship = args["ship"] as BaseShipController;
    }

    protected override void Awake() {
        base.Awake();
        this.ammoPool = new List<Ammo>(this.ammoPoolSize);
    }

    protected override void Start() {
        // Initialize the laser pool
        for (int i = 0; i < this.ammoPoolSize; i++) {
            Ammo ammo = this.gameController.CreateObject(this.ammoPrefab) as Ammo;
            ammo.transform.parent = this.transform;
            this.ammoPool.Add(ammo);
        }
    }

    protected override void Update() {
        base.Update();
        this.fireRate = this.gameController.fireRate;
    }

    private float[] GetSpread(int count, float angle = 5) {
        if (angle == 0 || count == 1) {
            return new float[] { 0 };
        }
        float fov = angle * (count - 1);
        float div = fov / 2;
        float[] angles = new float[count];
        int idx = 0;
        for (float i = -div; i <= div; i += angle) {
            angles[idx] = i;
            idx++;
        }
        return angles;
    }

    public bool Fire() {
        return Fire(1, 0);
    }

    public virtual bool Fire(int count, float angle) {
        if (Time.time > this.nextFire) {
            List<Ammo> unusedAmmoList = new List<Ammo>(count);
            for (int i = 0; i < this.ammoPool.Count; i++) {
                Ammo newLaser = this.ammoPool[i];
                // Fire laser if one exists
                if (!newLaser.gameObject.activeInHierarchy) {
                    unusedAmmoList.Add(newLaser);
                }
                if (unusedAmmoList.Count == count) {
                    break;
                }
            }

            if (unusedAmmoList.Count == 0) {
                return false;
            }

            this.firing = true;
            CancelInvoke("SetNotFire");
            Invoke("SetNotFire", 1 / this.fireRate + 0.05f);
            this.nextFire = Time.time + 1 / this.fireRate;
            float[] angles = GetSpread(count, angle);
            for (int i = 0; i < unusedAmmoList.Count; i++) {
                Ammo unusedAmmo = unusedAmmoList[i];
                GameObject unusedAmmoObj = unusedAmmo.gameObject;
                // Set position and rotation to be the same as the ship
                unusedAmmoObj.transform.position = this.transform.position;
                unusedAmmoObj.transform.rotation = this.ship.spriteTransform.rotation;
                unusedAmmoObj.transform.Rotate(new Vector3(0, 0, angles[i]));

                // Enable the laser and fire it
                unusedAmmoObj.SetActive(true);
                unusedAmmo.Fire();
            }
            return true;
        }
        return false;
    }

    private void SetNotFire() {
        this.firing = false;
    }
}
