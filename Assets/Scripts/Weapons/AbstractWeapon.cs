using UnityEngine;
using System.Collections.Generic;
using Weapon.Ammo;

namespace Weapon {
    public abstract class AbstractWeapon : BaseGameObject {

        protected BaseShipController ship;

        // Ammo
        protected List<AbstractAmmo> ammoPool;
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
            this.ammoPool = new List<AbstractAmmo>(this.ammoPoolSize);
        }

        protected override void Start() {
            // Initialize the laser pool
            int ammoLayer = LayerMask.NameToLayer(LayerMask.LayerToName(this.ship.gameObject.layer) + "Missile");
            for (int i = 0; i < this.ammoPoolSize; i++) {
                AbstractAmmo ammo = this.gameController.CreateObject(this.ammoPrefab, new Dictionary<string, object> {
                    { "layer", ammoLayer }
                }) as AbstractAmmo;
                ammo.transform.parent = this.gameController.bulletPoolObj.transform;
                this.ammoPool.Add(ammo);
            }
        }

        protected override void Update() {
            base.Update();
            this.fireRate = this.gameController.fireRate;
        }

        protected float[] GetSpread(int count, float angle = 5) {
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

        public abstract bool Fire(int count, float angle);

        private void SetNotFire() {
            this.firing = false;
        }
    }
}
