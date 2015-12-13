using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Timers;

namespace Skill {
    public abstract class AbstractSkill : BaseGameObject {
        protected Image image;
        protected Text cooldownText;
        private bool active;
        private float cooldownTimer;
        private float cooldownUntil;

        protected abstract bool Activate();

        public bool Activate(float cooldown) {
            if (!this.active) {
                return false;
            }
            if (cooldown > 0) {
                Cooldown(cooldown);
            }
            return Activate();
        }

        protected void Cooldown(float seconds) {
            this.active = false;
            this.image.color = Color.grey;
            this.cooldownTimer = 0;
            this.cooldownUntil = seconds;
        }

        protected override void Awake() {
            base.Awake();
            this.active = true;
            this.image = this.transform.Find("Image").GetComponent<Image>();
            this.cooldownText = this.transform.Find("CooldownText").GetComponent<Text>();
        }

        protected override void Update() {
            base.Update();
            if (!this.active) {
                this.cooldownTimer += Time.deltaTime;
                if (this.cooldownTimer <= this.cooldownUntil) {
                    float perc = this.cooldownTimer / this.cooldownUntil;
                    this.image.color = new Color(perc, perc, perc);
                    this.cooldownText.text = (this.cooldownUntil - this.cooldownTimer).ToString("F1");
                    //this.cooldownText.color = new Color(1 - perc, 1 - perc, 1 - perc);
                } else {
                    this.active = true;
                    this.cooldownText.text = "";
                }
            }
        }

        private void OnMouseUpAsButton() {
            Activate();
        }
    }
}
