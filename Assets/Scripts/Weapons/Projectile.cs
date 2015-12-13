using UnityEngine;
using System.Collections.Generic;
using Weapon.Ammo;

namespace Weapon {
    public class Projectile : AbstractWeapon {
        public override bool Fire(int count, float angle) {
            if (Time.time > this.nextFire) {
                List<AbstractAmmo> unusedAmmoList = new List<AbstractAmmo>(count);
                for (int i = 0; i < this.ammoPool.Count; i++) {
                    AbstractAmmo newLaser = this.ammoPool[i];
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
                    AbstractAmmo unusedAmmo = unusedAmmoList[i];
                    GameObject unusedAmmoObj = unusedAmmo.gameObject;
                    // Set position and rotation to be the same as the ship
                    unusedAmmoObj.transform.position = this.transform.position;
                    unusedAmmoObj.transform.rotation = this.transform.rotation;
                    unusedAmmoObj.transform.Rotate(new Vector3(0, 0, angles[i]));

                    // Enable the laser and fire it
                    unusedAmmoObj.SetActive(true);
                    unusedAmmo.Fire();
                }
                return true;
            }
            return false;
        }
    }
}
