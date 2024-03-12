using System;
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

    [Header("Blood Overlay")]
    [SerializeField] private GameObject bloodOverlay;
    [SerializeField] private float fadeDuration = 3f;

    public float GetHealth() => health;
    public void SetHealth(float value) => health = value;

    private void Start()
    {
        health = 100;
    }

    public void ReceiveDamage(float damage)
    {
        health -= damage;
        //Debug.Log(health);
        if (health <= 0)
        {
            // player dead
            health = 0;

        }
        else
        {
            // receive hit
            StartCoroutine(BloodOverlayEffect());
        }
        lerpTimer = 0;
    }

    private IEnumerator BloodOverlayEffect()
    {
        if(bloodOverlay.activeInHierarchy == false)
        {
            bloodOverlay.SetActive(true);
        }

        // Get the Image component for fading.
        var fadeImage = bloodOverlay.GetComponentInChildren<Image>();

        // Set up initial values.
        float timer = 0f;
        Color startColor = new(0.6f, 0f, 0f, 0.6f);
        Color endColor = new(1f, 1f, 1f, 0); // Final color (black with alpha 0)
        // Loop to interpolate color over time.
        while (timer < fadeDuration)
        {
            fadeImage.color = Color.Lerp(startColor, endColor, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        if (bloodOverlay.activeInHierarchy)
        {
            bloodOverlay.SetActive(false);
        }
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
        HealthUIUpdate();
        IncreaseStamina();
    }
}
