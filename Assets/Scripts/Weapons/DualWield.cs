using UnityEngine;
using System.Collections.Generic;

namespace Weapon {
    public class DualWield : Projectile {
        private Projectile weapon1;
        private Projectile weapon2;

        private float x1;
        private float x2;

        public override void Initialize(IDictionary<string, object> args = null) {
            base.Initialize(args);
            this.x1 = (float)args["x1"];
            this.x2 = (float)args["x2"];
        }

        public override bool Fire(int count, float angle) {
            bool weapon1Fired = this.weapon1.Fire(count, angle);
            bool weapon2Fired = this.weapon2.Fire(count, angle);
            return weapon1Fired || weapon2Fired;
        }

        protected override void Start() {
            base.Start();
            this.weapon1 = this.ship.AddWeapon(this.gameController.CreateObject(this.gameController.weaponPrefabs["Projectile"], new Dictionary<string, object> {
                { "ammoPrefab", this.ammoPrefab },
                { "ship", this.ship }
            }) as AbstractWeapon) as Projectile;
            this.weapon1.transform.parent = this.transform;
            this.weapon1.transform.Translate(this.x1, 0, 0);
            this.weapon2 = this.ship.AddWeapon(this.gameController.CreateObject(this.gameController.weaponPrefabs["Projectile"], new Dictionary<string, object> {
                { "ammoPrefab", this.ammoPrefab },
                { "ship", this.ship }
            }) as AbstractWeapon) as Projectile;
            this.weapon2.transform.parent = this.transform;
            this.weapon2.transform.Translate(this.x2, 0, 0);
        }
    }
}
