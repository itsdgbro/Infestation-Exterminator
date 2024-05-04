using TMPro;
using UnityEngine;

public class HealingScript : MonoBehaviour, IDataPersistence
{
    // private PlayerControls playerControls;
    private PlayerInputHandler playerControls;


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
    }

    private void Start()
    {
        playerControls = PlayerInputHandler.Instance;
        if (playerControls == null)
        {
            Debug.LogWarning("PLayer controls");
        }
    }

    // Trigger from animation event
    public void Heal()
    {
        playerStat.RestoreHealth(healingData.healAmount);
    }

    private void Update()
    {
        PillCountUI();
        if (playerControls.HealTriggered && healingData.availablePills > 0)
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
        healingData.availablePills = data.player.healingPillsLeft;
    }

    public void SaveData(GameData data)
    {
        data.player.healingPillsLeft = healingData.availablePills;
    }
}
