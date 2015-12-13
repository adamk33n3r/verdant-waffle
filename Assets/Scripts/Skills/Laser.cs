using UnityEngine;
using System.Collections;

namespace Skill {
    public class Laser : AbstractSkill {
        protected override bool Activate() {
            Debug.Log("Laser activated!");
            return true;
        }
    }
}
