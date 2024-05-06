using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponScript : MonoBehaviour, IDataPersistence
{
    // private PlayerControls playerControls;
    private PlayerInputHandler playerControls;
    // Input Action
    private InputAction inputAction;

    private Animator animator;

    [Header("References")]
    [SerializeField] private WeaponData weaponData;

    #region Muzzle
    [Header("Muzzle Parameters")]
    public Transform muzzle;
    private Vector3 muzzleTarget = new(0.5f, 0.5f, 0f);
    private Ray rayFromMuzzle;
    #endregion
    [SerializeField] private new ParticleSystem particleSystem;
    [SerializeField] private GameObject bulletHolePrefab;

    public AudioSource audioSource1;
    public AudioSource audioSource2;

    [Header("Aduio Clips")]
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip emptyClipSound;
    [SerializeField] private AudioClip reloadSound;
    [SerializeField] private AudioClip drawSound;

    [Header("Game Manager Reference")]
    [SerializeField] private GameManager gameManager;


    private float timeSinceLastShot;
    [HideInInspector] public bool isAiming;
    public Camera fpsCam;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        // Subscribe action
        // inputAction.
    }

    private void Start()
    {
        playerControls = PlayerInputHandler.Instance;
        // reload-action subscribe 
        PlayerInputHandler.Instance.ReloadAction.started += ReloadWeapon;
    }
    // show ray from muzzle
    void ShowRayCast()
    {
        rayFromMuzzle = new(muzzle.transform.position, Camera.main.ViewportPointToRay(muzzleTarget).direction);

        bool hitTarget = Physics.Raycast(rayFromMuzzle, out RaycastHit hitInfo, weaponData.maxDistance);

        Debug.DrawRay(rayFromMuzzle.origin, rayFromMuzzle.direction * (hitTarget ? hitInfo.distance : weaponData.maxDistance),
            hitTarget && hitInfo.collider.CompareTag("Target") ? Color.green : Color.red);

    }

    private void ReloadWeapon(InputAction.CallbackContext context)
    {
        if (context.started && weaponData.ammoLeft > 0)
            if (!weaponData.isReloading && this.gameObject.activeSelf && weaponData.currentAmmo != weaponData.magazineSize)
                StartCoroutine(ReloadTime());
    }

    private IEnumerator ReloadTime()
    {

        weaponData.isReloading = true;
        animator.Play("reload");
        animator.SetBool("isEmpty", false);
        audioSource1.PlayOneShot(reloadSound);

        yield return new WaitForSeconds(weaponData.reloadTime);
        // ammo system

        int totalAmmo = weaponData.currentAmmo + weaponData.ammoLeft;

        if (totalAmmo <= weaponData.magazineSize)
        {
            weaponData.currentAmmo = totalAmmo;
            weaponData.ammoLeft = 0;
        }
        else
        {
            weaponData.currentAmmo = weaponData.magazineSize;
            weaponData.ammoLeft = totalAmmo - weaponData.magazineSize;
        }
        weaponData.isReloading = false;
    }

    private void GunIsEmpty()
    {
        animator.SetBool("isEmpty", true);

        audioSource2.Play();
    }

    private bool CanShoot() => !weaponData.isReloading && weaponData.currentAmmo > 0 && timeSinceLastShot > 1f / (weaponData.fireRate / 60f) && !gameManager.GetIsGamePaused();


    private void AimShoot()
    {
        if (CanShoot() && weaponData.isAutomatic && playerControls.FireAutoTriggered > 0.1f)
        {
            CommonFireLogic("ironshot_fire");
            PlayParticleEffect();
        }
        else if (CanShoot() && !weaponData.isAutomatic && playerControls.FireTriggered)
        {
            CommonFireLogic("ironshot_fire");
            PlayParticleEffect();
        }
        else if (weaponData.currentAmmo == 0 && playerControls.FireTriggered)
        {
            GunIsEmpty();
        }
    }


    private void OnAdsFire()
    {
        if (CanShoot() && weaponData.isAutomatic && playerControls.FireAutoTriggered > 0.1f)
        {
            CommonFireLogic("fire");
            PlayParticleEffect();
        }
        else if (CanShoot() && !weaponData.isAutomatic && playerControls.FireTriggered)
        {
            CommonFireLogic("fire");
            PlayParticleEffect();
        }
        else if (weaponData.currentAmmo == 0 && playerControls.FireTriggered)
        {

            GunIsEmpty();
        }
    }

    private void CommonFireLogic(string animation)
    {
        animator.Play(animation);
        animator.SetBool("isEmpty", false);
        audioSource1.PlayOneShot(shootSound);

        // shooting from camera to crosshair for accuracy
        Vector3 rayOrigin = fpsCam.transform.position;
        Vector3 rayDirection = fpsCam.transform.forward;

        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hitInfo, weaponData.maxDistance))
        {
            GameObject hitObject = hitInfo.collider.gameObject;
            ISTarget isTarget = hitObject.GetComponentInParent<ISTarget>();
            isTarget?.TakeDamage(weaponData.damage);
            BulletHoleEffect(hitInfo);
        }

        weaponData.currentAmmo--;
        timeSinceLastShot = 0;
    }

    // Method to be called from the animation event
    public void PlayParticleEffect()
    {
        if (particleSystem != null)
        {
            particleSystem.Play();
        }
    }

    private void BulletHoleEffect(RaycastHit hit)
    {
        if (hit.collider.CompareTag("Environment"))
        {
            GameObject impact = Instantiate(bulletHolePrefab, hit.point, Quaternion.identity);
            impact.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);

            impact.transform.Translate(hit.normal * 0.02f, Space.World);

            Destroy(impact, 2f);
        }

    }

    public void PlayDrawSound()
    {
        audioSource1.PlayOneShot(drawSound);
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        //DrawRayFromMuzzle();
        ShowRayCast();


        isAiming = playerControls.AimTriggered > 0.1f;
        animator.SetBool("isAiming", isAiming);
        if (Time.timeScale > 0)
        {
            if (isAiming)
            {
                AimShoot();
            }
            else
            {
                OnAdsFire();
            }
        }
    }


    public void LoadData(GameData data)
    {
        if (this.gameObject.name == "AR")
        {
            weaponData.ammoLeft = data.weapon.ar.totalAmmo;
            weaponData.currentAmmo = data.weapon.ar.currentAmmo;

            Debug.Log("AR " + data.weapon.ar.currentAmmo);
        }
        else if (this.gameObject.name == "Pistol")
        {
            weaponData.ammoLeft = data.weapon.pistol.totalAmmo;
            weaponData.currentAmmo = data.weapon.pistol.currentAmmo;
        }

    }


    public void SaveData(GameData data)
    {
        if (this.gameObject.name == "AR")
        {
            data.weapon.ar.totalAmmo = weaponData.ammoLeft;
            data.weapon.ar.currentAmmo = weaponData.currentAmmo;
        }
        else if (this.gameObject.name == "Pistol")
        {
            data.weapon.pistol.totalAmmo = weaponData.ammoLeft;
            data.weapon.pistol.currentAmmo = weaponData.currentAmmo;
        }
    }

    private void OnDisable()
    {
        // reload-action unsubscribe 
        PlayerInputHandler.Instance.ReloadAction.started -= ReloadWeapon;
    }
}
