using UnityEngine;
using System.Collections.Generic;

// TODO: Abstract class
public class Weapon : BaseGameObject {
    protected Ammo ammo;

    public override void Initialize(IDictionary<string, object> args = null) {
        this.ammo = args["ammo"] as Ammo;
    }
}
