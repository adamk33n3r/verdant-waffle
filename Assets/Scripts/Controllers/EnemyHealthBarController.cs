using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarController : MonoBehaviour {
    private Image healthBar;
    private Text healthText;
    private EnemyController enemy;

    void Awake() {
        this.healthBar = GetComponentInChildren<Image>();
        this.healthText = GetComponentInChildren<Text>();
        this.enemy = GetComponentInParent<EnemyController>();
    }

    void Update() {
        this.transform.rotation = Quaternion.identity;
        this.healthBar.fillAmount = this.enemy.currentHealth / this.enemy.maxHealth;
        this.healthText.text = "Health: " + this.enemy.currentHealth;
    }
}
