using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealingScript : MonoBehaviour
{
    private Animator animator;
    [Header("Reference to Healing Data")]
    [SerializeField] private HealData data;

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
        data.availablePills = 5;
        playerControls = new PlayerControls();
        if(playerControls == null)
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
        playerStat.RestoreHealth(data.healAmount);
    }

    private void Update()
    {
        PillCountUI();
        if (playerControls.Movement.Heal.triggered && data.availablePills > 0)
        {   
            if (playerStat.GetHealth() < 100)
            {
                audioSource.PlayOneShot(healingAudio);
                fpsWeapons.SetActive(false);
                animator.Play("Heal");
                data.availablePills--;
            }
        }
    }

    public void ToggleWeaponHideUnHide()
    {
        fpsWeapons.SetActive(true);
    }

    private void PillCountUI()
    {
        pillCountUI.text = data.availablePills.ToString()+"x";
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
