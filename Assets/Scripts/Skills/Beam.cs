using UnityEngine;
using System.Collections;

namespace Skill {
    public class Beam : AbstractSkill {
        protected override bool Activate() {
            Debug.Log("Beam activated!");
            this.gameController.player.SwitchWeapon(2);
            this.gameController.player.FireWeapon();
            this.gameController.player.SwitchWeapon(0);
            return true;
        }
    }
}
