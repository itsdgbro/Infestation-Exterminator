using System.Collections;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    private PlayerControls playerControls;
    private Animator animator;

    [Header("References")]
    [SerializeField] private WeaponData weaponData;

    #region Muzzle
    [Header("Muzzle Parameters")]
    public Transform muzzle;
    private Vector3 muzzleTarget = new(0.5f, 0.5f, 0f);
    private Ray rayFromMuzzle;
    #endregion

    AudioSource audioSource;

    [Header("Aduio Clips")]
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip emptyClipSound;
    [SerializeField] private AudioClip reloadSound;

    private float timeSinceLastShot;
    [HideInInspector]public bool isAiming;
    public Camera fpsCam;

    private void Awake()
    {

        playerControls = new PlayerControls();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // show ray from muzzle
    void ShowRayCast()
    {
        rayFromMuzzle = new(muzzle.transform.position, Camera.main.ViewportPointToRay(muzzleTarget).direction);

        bool hitTarget = Physics.Raycast(rayFromMuzzle, out RaycastHit hitInfo, weaponData.maxDistance);

        Debug.DrawRay(rayFromMuzzle.origin, rayFromMuzzle.direction * (hitTarget ? hitInfo.distance : weaponData.maxDistance),
            hitTarget && hitInfo.collider.CompareTag("Target") ? Color.green : Color.red);

    }

    /*    
       private void OnDrawGizmos()
        {
            RaycastHit hit;
            Vector3 rayOrigin = fpsCam.transform.position;
            Vector3 rayDirection = fpsCam.transform.forward;
            if (Physics.Raycast(rayOrigin, rayDirection, out hit, weaponData.maxDistance))
            {
                // Draw the ray using Gizmos
                Gizmos.color = Color.green;
                Gizmos.DrawLine(rayOrigin, hit.point);

                Debug.Log("Hit Object Name: " + hit.collider.gameObject.name);

            }
            else
            {
                // Draw the ray if it doesn't hit anything
                Gizmos.color = Color.green;
                Gizmos.DrawLine(rayOrigin, rayOrigin + rayDirection * 100f); // Adjust the length as needed
            }
        }*/

    private void ReloadWeapon()
    {
        if (playerControls.Movement.Reload.triggered)
            if (!weaponData.isReloading && this.gameObject.activeSelf && weaponData.currentAmmo != weaponData.magazineSize)
                StartCoroutine(ReloadTime());
    }

    private IEnumerator ReloadTime()
    {
        weaponData.isReloading = true;
        animator.Play("reload");
        animator.SetBool("isEmpty", false);
        audioSource.PlayOneShot(reloadSound);
        Debug.Log("RELOADING");

        yield return new WaitForSeconds(weaponData.reloadTime);

        weaponData.currentAmmo = weaponData.magazineSize;
        weaponData.isReloading = false;
    }

    private void GunIsEmpty()
    {
        animator.SetBool("isEmpty", true);
        audioSource.PlayOneShot(emptyClipSound);
    }

    private bool CanShoot() => !weaponData.isReloading && weaponData.currentAmmo > 0 && timeSinceLastShot > 1f / (weaponData.fireRate / 60f);

    private void AimShoot()
    {
        if (CanShoot() && weaponData.isAutomatic && playerControls.Movement.Fire.ReadValue<float>() > 0.1f)
        {
            CommonFireLogic("ironshot_fire");
        }
        else if (CanShoot() && !weaponData.isAutomatic && playerControls.Movement.Fire.triggered)
        {
            CommonFireLogic("ironshot_fire");
        }
        else if (weaponData.currentAmmo == 0 && playerControls.Movement.Fire.triggered)
        {
            GunIsEmpty();
        }
    }


    private void OnAdsFire()
    {
        if (CanShoot() && weaponData.isAutomatic && playerControls.Movement.Fire.ReadValue<float>() > 0.1f)
        {
            CommonFireLogic("fire");
        }
        else if (CanShoot() && !weaponData.isAutomatic && playerControls.Movement.Fire.triggered)
        {
            CommonFireLogic("fire");
        }
        else if (weaponData.currentAmmo == 0 && playerControls.Movement.Fire.triggered)
        {
            GunIsEmpty();
        }
    }

    private void CommonFireLogic(string animation)
    {
        animator.Play(animation);
        animator.SetBool("isEmpty", false);
        audioSource.PlayOneShot(shootSound);

        // shooting from camera to crosshair for accuracy
        Vector3 rayOrigin = fpsCam.transform.position;
        Vector3 rayDirection = fpsCam.transform.forward;

        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hitInfo, weaponData.maxDistance))
        {
            GameObject hitObject = hitInfo.collider.gameObject;
            IsTarget isTarget = hitObject.GetComponentInParent<IsTarget>();
            isTarget?.TakeDamage(weaponData.damage);
        }

        Debug.Log("Fired. Ammo left: " + ((int)weaponData.currentAmmo - 1));
        weaponData.currentAmmo--;
        timeSinceLastShot = 0;
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        //DrawRayFromMuzzle();
        ShowRayCast();

        isAiming = playerControls.Movement.Aim.ReadValue<float>() > 0.1f;
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
            ReloadWeapon();
        }
    }

    #region Enable/Disable
    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
        weaponData.isReloading = false;
    }
    #endregion
}
