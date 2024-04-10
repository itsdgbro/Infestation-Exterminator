using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealingScript : MonoBehaviour, IDataPersistence
{
    private Animator animator;
    [Header("Reference to Healing Data")]
    [SerializeField] private HealData healingData;

    [Header("Parent references")]
    private PlayerStat playerStat;
    [SerializeField] private GameObject fpsWeapons;
    private PlayerControls playerControls;

    [Header("Healing UI")]
    [SerializeField] private TextMeshProUGUI pillCountUI;

    [Header("Healing Audio")]
    private AudioSource audioSource;
    [SerializeField] private AudioClip healingAudio;

    private void Awake()
    {
        playerControls = new PlayerControls();
        if (playerControls == null)
        {
            Debug.LogWarning("PLayer controls");
        }
        animator = GetComponent<Animator>();
        playerStat = GetComponentInParent<PlayerStat>();
        if (playerStat == null)
            Debug.LogWarning("PlayerStat not found");
        audioSource = GetComponent<AudioSource>();
    }

    // Trigger from animation event
    public void Heal()
    {
        playerStat.RestoreHealth(healingData.healAmount);
    }

    private void Update()
    {
        PillCountUI();
        if (playerControls.Movement.Heal.triggered && healingData.availablePills > 0)
        {
            if (playerStat.GetHealth() < 100)
            {
                audioSource.PlayOneShot(healingAudio);
                fpsWeapons.SetActive(false);
                animator.Play("Heal");
                healingData.availablePills--;
            }
        }
    }

    public void ToggleWeaponHideUnHide()
    {
        fpsWeapons.SetActive(true);
    }

    private void PillCountUI()
    {
        pillCountUI.text = healingData.availablePills.ToString() + "x";
    }

    public void LoadData(GameData data)
    {
        Debug.Log("Hea");
        healingData.availablePills = data.player.healingPillsLeft;
    }

    public void SaveData(GameData data)
    {
        data.player.healingPillsLeft = healingData.availablePills;
    }

    #region Enable/Disable
    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    #endregion
}
