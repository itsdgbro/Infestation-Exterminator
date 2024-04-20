using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerStat : MonoBehaviour, IDataPersistence
{
    [Header("References")]
    [SerializeField] private PlayerData playerData;     // playerData 
    [SerializeField] private Image staminaBar;      // stamina UI
    [Range(0f, 100f)]
    private readonly float maxHealth = 100f;

    private float lerpTimer;
    private readonly float chipSpeed = 2f;

    [Header("Image Reference")]     // health UI
    public Image frontHealthBar;
    public Image backHealthBar;

    [Header("Blood Overlay")]
    [SerializeField] private GameObject bloodOverlay;   // bloodUI
    [SerializeField] private float fadeDuration = 3f;

    [Header("Main Camera")]
    [SerializeField] private Camera mainCamera;
    public Vector3 deathCameraPosition = new(-0.39f, -1.35f, 0f); // Ending position of the camera
    public float transitionDuration = 0.75f;

    [Header("Damage Audio")]
    private AudioSource audioSource;
    [SerializeField] private AudioClip damageAudio;

    [Header("Game Manager")]
    [SerializeField] private GameManager gameManager;

    public float GetHealth() => this.playerData.playerHealth;
    public void SetHealth(float value) => this.playerData.playerHealth = value;

    private CharacterController characterController;
    private PlayerControls playerControls;


    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerControls = new PlayerControls();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {

        // check state player is dead or alive
        this.playerData.isDead = playerData.playerHealth <= 0;

    }

    public bool IsDead()
    {
        return this.playerData.isDead;
    }

    public void ReceiveDamage(float damage)
    {
        this.playerData.playerHealth -= damage;
        //Debug.Log(playerData.playerHealth);
        if (this.playerData.playerHealth <= 0)
        {
            // player dead
            this.playerData.playerHealth = 0;
            DeathCam();
        }
        else
        {
            // receive hit
            if(damageAudio == null)
            {
                Debug.LogError("NUL");
            }
            else
            {
                audioSource.PlayOneShot(damageAudio);

            }
            StartCoroutine(BloodOverlayEffect());
        }
        lerpTimer = 0;
    }

    private IEnumerator BloodOverlayEffect()
    {
        if (bloodOverlay.activeInHierarchy == false)
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

        // keeping playerData.playerHealth fraction between 0 and 1
        float hFraction = playerData.playerHealth / maxHealth;

        if (fillB > hFraction)
        {
            frontHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }
        if (fillF < hFraction)
        {
            backHealthBar.color = Color.green;
            backHealthBar.fillAmount = hFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillF, backHealthBar.fillAmount, percentComplete);
        }
    }

    public void RestoreHealth(float healAmount)
    {
        if (playerData.playerHealth < maxHealth)
        {
            playerData.playerHealth += healAmount;
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

    // player dead
    public void DeadUIDisplay()
    {
        if (gameManager != null && IsDead())
        {
            playerControls.Disable();
            gameManager.ShowDeadUI();
        }
    }

    public void DeathCam()
    {
        StartCoroutine(TransitionCamera());
    }

    private IEnumerator TransitionCamera()
    {
        Vector3 initialCameraOffset = mainCamera.transform.localPosition; // Store the initial camera local position
        Vector3 targetOffset = deathCameraPosition; // Target offset relative to the parent

        float elapsedTime = 0f; // Elapsed time since the start of the transition

        while (elapsedTime < transitionDuration)
        {
            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            // Calculate the interpolation factor based on the elapsed time
            float t = Mathf.Clamp01(elapsedTime / transitionDuration);

            // Interpolate the camera local position from initial to target position
            mainCamera.transform.localPosition = Vector3.Lerp(initialCameraOffset, targetOffset, t);

            yield return null; // Wait for the next frame
        }

        // Ensure the camera reaches the exact target position at the end of the transition
        mainCamera.transform.localPosition = targetOffset;
    }

    private void Update()
    {
        playerData.stamina = staminaBar.fillAmount;
        this.playerData.isDead = playerData.playerHealth <= 0;
        HealthUIUpdate();
        IncreaseStamina();
    }



    public void SaveData(GameData data)
    {
        data.player.health = this.playerData.playerHealth;
        data.sceneName = SceneManager.GetActiveScene().name;

        data.player.position = this.transform.position;

        data.player.rotation = this.transform.rotation;

        data.player.direction = this.transform.forward;
    }

    public void LoadData(GameData data)
    {
        this.playerData.playerHealth = data.player.health;

        characterController.enabled = false;
        this.transform.SetPositionAndRotation(data.player.position, data.player.rotation);
        characterController.enabled = true;

        this.transform.forward = data.player.direction;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
}
