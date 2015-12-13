using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SettingsDisplay : BaseGameObject {
    private Text text;
    protected override void Awake() {
        base.Awake();
        this.text = GetComponent<Text>();
    }
    protected override void Update() {
        this.text.text = string.Format(@"Ammo Count: {0}
Ammo Spread: {1}
Ammo Speed: {2}
Fire Rate (bps): {3}
Current Weapon: {4}", this.gameController.ammoCount, this.gameController.ammoSpread, this.gameController.ammoSpeed, this.gameController.fireRate, this.gameController.currentWeapon);
    }
}
