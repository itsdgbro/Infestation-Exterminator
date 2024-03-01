using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStat : MonoBehaviour
{
    
    [SerializeField] private PlayerData playerData;
    private float maxHealth = 100f;

    private float health;
    private float lerpTimer;
    private float chipSpeed = 2f;

    [Header("Image Reference")]
    public Image frontHealthBar;
    public Image backHealthBar;


    private void Start()
    {
        health = maxHealth;
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

    private void Update()
    {
        Debug.Log("Player Health " +  health);
        HealthUIUpdate();
        if(Input.GetKeyDown(KeyCode.X) )
        {
            RestoreHealth(10);
        }
    }
}
