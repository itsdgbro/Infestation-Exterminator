using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class HealingScript : MonoBehaviour, IDataPersistence
{
    private Animator animator;
    [Header("Reference to Healing Data")]
    [SerializeField] private HealData healingData;

    [Header("Parent references")]
    private PlayerStat playerStat;
    [SerializeField] private GameObject fpsWeapons;

    [Header("Healing UI")]
    [SerializeField] private TextMeshProUGUI pillCountUI;

    [Header("Healing Audio")]
    private AudioSource audioSource;
    [SerializeField] private AudioClip healingAudio;

    private void Awake()
    {

        animator = GetComponent<Animator>();
        playerStat = GetComponentInParent<PlayerStat>();
        if (playerStat == null)
            Debug.LogWarning("PlayerStat not found");
        audioSource = GetComponent<AudioSource>();

        PillCountUI();
    }

    private void Start()
    {
        // action subscribe
        PlayerInputHandler.Instance.HealAction.started += PerfromHeal;
    }

    // Trigger from animation event
    public void Heal()
    {
        playerStat.RestoreHealth(healingData.healAmount);
    }

    // Trigger for heal animation
    private void PerfromHeal(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (healingData.availablePills > 0)
            {
                if (playerStat.GetHealth() < 100)
                {
                    audioSource.PlayOneShot(healingAudio);
                    fpsWeapons.SetActive(false);
                    animator.Play("Heal");
                    healingData.availablePills--;
                    PillCountUI();
                }
            }
        }
    }

    // During healing hide the weapon
    public void ToggleWeaponHideUnHide()
    {
        fpsWeapons.SetActive(true);
    }

    // Disaply no.of pills in UI 
    private void PillCountUI()
    {
        pillCountUI.text = healingData.availablePills.ToString() + "x";
    }

    public void LoadData(GameData data)
    {
        healingData.availablePills = data.player.healingPillsLeft;
    }

    public void SaveData(GameData data)
    {
        data.player.healingPillsLeft = healingData.availablePills;
    }

    private void OnDisable()
    {
        // action unsubscribe
        PlayerInputHandler.Instance.HealAction.started -= PerfromHeal;
    }
}
