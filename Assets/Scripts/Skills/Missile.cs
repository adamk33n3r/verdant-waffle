using UnityEngine;
using System.Collections;

namespace Skill {
    public class Missile : AbstractSkill {
        protected override bool Activate() {
            Debug.Log("Missle activated!");
            this.gameController.player.SwitchWeapon(1);
            this.gameController.player.FireWeapon();
            this.gameController.player.SwitchWeapon(0);
            return true;
        }
    }
}