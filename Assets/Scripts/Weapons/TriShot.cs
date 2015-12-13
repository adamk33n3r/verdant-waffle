using UnityEngine;
using System.Collections.Generic;

public class TriShot : Weapon {
    protected override void Awake() {
        this.ammoPoolSize = 120;
        base.Awake();
    }
}
