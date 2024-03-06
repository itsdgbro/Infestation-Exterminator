using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStat : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerData playerData;
    [SerializeField] private Image staminaBar;
    [Range(0f, 100f)]
    private readonly float maxHealth = 100f;

    private float health;
    private float lerpTimer;
    private float chipSpeed = 2f;

    [Header("Image Reference")]
    public Image frontHealthBar;
    public Image backHealthBar;


    public float GetHealth() => health;
    public void SetHealth(float value) => health = value;

    private void Start()
    {
        health = 20;
    }

    public void ReceiveDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            // player die
            Destroy(this.gameObject);
        }
        lerpTimer = 0;
    }

    public void HealthUIUpdate()
    {
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;

        // keeping health fraction between 0 and 1
        float hFraction = health / maxHealth;

        if(fillB > hFraction)
        {
            frontHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }
        if(fillF < hFraction)
        {
            backHealthBar.color = Color.green;
            backHealthBar.fillAmount = hFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer/ chipSpeed;
            percentComplete = percentComplete * percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillF, backHealthBar.fillAmount, percentComplete);
        }
    }

    public void RestoreHealth(float healAmount)
    {
        if(health < maxHealth)
        {
            health += healAmount;
        }
        lerpTimer = 0f;
    }

    // stamina system
    public void DecreaseStamina()
    {
        if (playerData.stamina > 0)
        {
            staminaBar.fillAmount -= Time.deltaTime / (1.5f * playerData.staminaChangeRate);
            // Ensure fillAmount doesn't go below 0
            staminaBar.fillAmount = Mathf.Max(staminaBar.fillAmount, 0f);
        }
    }

    public void DecreaseStamina(float value)
    {
        if (playerData.stamina > 0)
        {
            staminaBar.fillAmount -= Time.deltaTime * value;
            // Ensure fillAmount doesn't go below 0
            staminaBar.fillAmount = Mathf.Max(staminaBar.fillAmount, 0f);
        }
    }

    private void IncreaseStamina()
    {
        if (staminaBar.fillAmount < 1)
        {
            staminaBar.fillAmount += Time.deltaTime / (4 * playerData.staminaChangeRate);
        }
    }

    public bool CanSprint()
    {
        return playerData.stamina > 0.01;
    }

    private void Update()
    {
        playerData.stamina = staminaBar.fillAmount;
        Debug.Log("Player Health " + playerData.stamina);
        HealthUIUpdate();
        IncreaseStamina();
    }
}
