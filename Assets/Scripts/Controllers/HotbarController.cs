using UnityEngine;
using System.Collections.Generic;
using Skill;

public class HotbarController : BaseGameObject {
    private AbstractSkill[] skills;
    //private AbstractSkill activeSkill;

    protected override void Awake() {
        base.Awake();
        this.skills = GetComponentsInChildren<AbstractSkill>();
    }

    protected override void Update() {
        base.Update();
        if (Input.GetKeyUp(KeyCode.Alpha1)) {
            this.skills[0].Activate(0);
        } else if (Input.GetKeyUp(KeyCode.Alpha2)) {
            this.skills[1].Activate(6);
        }
    }
}
