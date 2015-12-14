using UnityEngine;
using System.Collections;
using Weapon.Ammo;

namespace Weapon {
    public class Beam : AbstractWeapon {
        AbstractAmmo beam;
        
        protected override void Start() {
            this.ammoPoolSize = 1;
            base.Start();
            this.beam = this.ammoPool[0];
            this.beam.transform.parent = this.ship.transform;
            this.beam.transform.position = new Vector3(0, this.beam.transform.localScale.y / 2 + this.ship.transform.localScale.y / 2, 0);
        }

        public override bool Fire(int count, float angle) {
            this.beam.gameObject.SetActive(true);
            this.beam.Fire();
            return true;
        }
    }
}
