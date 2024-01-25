using System;
using System.Collections;
using System.Collections.Generic;
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

    public ParticleSystem muzzleFlash;

    AudioSource audioSource;

    [SerializeField] private AudioClip pistolDraw;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip emptyClipSound;
    [SerializeField] private AudioClip reloadSound;

    float timeSinceLastShot;
    private bool isAiming;

    /*
    #region Bullet
    [Header("Bullet Parameters")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int bulletSpeed = 500;
    #endregion
    */

    private void Awake()
    {

        playerControls = new PlayerControls();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        audioSource.PlayOneShot(pistolDraw);
        
    }

    void ShowRayCast()
    {
        rayFromMuzzle = new(muzzle.transform.position, Camera.main.ViewportPointToRay(muzzleTarget).direction);

        bool hitTarget = Physics.Raycast(rayFromMuzzle, out RaycastHit hitInfo, weaponData.maxDistance);
        // Draw the ray using Debug.DrawRay
        Debug.DrawRay(rayFromMuzzle.origin, rayFromMuzzle.direction * (hitTarget ? hitInfo.distance : weaponData.maxDistance),
            hitTarget && hitInfo.collider.CompareTag("Target") ? Color.green : Color.red);

    }

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

    private bool CanShoot() => !weaponData.isReloading && timeSinceLastShot > 1f / (weaponData.fireRate / 60f);

    private void AimShoot()
    {
        if (CanShoot() && playerControls.Movement.Fire.triggered)
        {
            CommonFireLogic("ironshot_fire");
        }
    }

    private void OnAdsFire()
    {
        if (CanShoot() && playerControls.Movement.Fire.triggered)
        {
            CommonFireLogic("fire");
        }
    }

    private void CommonFireLogic(string animation)
    {
        if (weaponData.currentAmmo > 0)
        {
            animator.Play(animation);
            animator.SetBool("isEmpty", false);
            audioSource.PlayOneShot(shootSound);

            // muzzle flash particles
            muzzleFlash.Play();

            if (Physics.Raycast(rayFromMuzzle, out RaycastHit hitInfo, weaponData.maxDistance))
            {
                GameObject hitObject = hitInfo.collider.gameObject;
                IsTarget isTarget = hitObject.GetComponentInParent<IsTarget>();
                isTarget?.TakeDamage(weaponData.damage);
            }

            Debug.Log("Fired. Ammo left: " + ((int)weaponData.currentAmmo - 1));
            weaponData.currentAmmo--;
            timeSinceLastShot = 0;
        }
        else
        {
            animator.SetBool("isEmpty", true);
            audioSource.PlayOneShot(emptyClipSound);
            Debug.Log("Empty clip.");
        }
    }

    private void Update()
    {   
        timeSinceLastShot += Time.deltaTime;
        //DrawRayFromMuzzle();
        ShowRayCast();

        isAiming = Input.GetMouseButton(1);
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
