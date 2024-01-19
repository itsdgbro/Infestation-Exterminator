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

    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip emptyClipSound;
    [SerializeField] private AudioClip reloadSound;

    float timeSinceLastShot;

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

    private void DrawRayFromMuzzle()
    {
        rayFromMuzzle = new(muzzle.transform.position, Camera.main.ViewportPointToRay(muzzleTarget).direction);
    }

    void ShowRayCast()
    {
        bool hitTarget = Physics.Raycast(rayFromMuzzle, out RaycastHit hitInfo, weaponData.maxDistance);
        // Draw the ray using Debug.DrawRay
        Debug.DrawRay(rayFromMuzzle.origin, rayFromMuzzle.direction * (hitTarget ? hitInfo.distance : weaponData.maxDistance),
            hitTarget && hitInfo.collider.CompareTag("Target") ? Color.green : Color.red);

    }

    private void ReloadWeapon()
    {
        if (playerControls.Movement.Reload.triggered)
            if (!weaponData.isReloading && this.gameObject.activeSelf && weaponData.currentAmmo!=weaponData.magazineSize)
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

    private void OnFire()
    {
        if (playerControls.Movement.Fire.triggered)
        {
            if (CanShoot())
            {
                if (weaponData.currentAmmo > 0)
                {
                    animator.SetBool("isEmpty", false);
                    audioSource.PlayOneShot(shootSound);
                    animator.Play("fire");

                    // muzzle flash particles
                    muzzleFlash.Play();

                    // Target Shoot 
                    if (Physics.Raycast(rayFromMuzzle, out RaycastHit hitInfo, weaponData.maxDistance))
                    {
                        // Get the GameObject that was hit
                        GameObject hitObject = hitInfo.collider.gameObject;

                        // Try to get the IsTarget component from the hitObject or its ancestors
                        IsTarget isTarget = hitObject.GetComponentInParent<IsTarget>();

                        // If the hitObject or any of its ancestors has the IsTarget component, apply damage
                        isTarget?.TakeDamage(weaponData.damage);

                        // bullet prefab
                        /*
                        // If the ray hits something, calculate the direction based on the hit point
                        Vector3 shootDirection = hitInfo.point - muzzle.position;
                        shootDirection.Normalize();

                        // Instantiate the bullet at the muzzle position
                        GameObject bullet = Instantiate(bulletPrefab, muzzle.position, Quaternion.identity);

                        // Rotate the bullet to face the direction it's moving
                        bullet.transform.rotation = Quaternion.LookRotation(shootDirection);

                        // Apply force to the bullet in the determined direction
                        bullet.GetComponent<Rigidbody>().AddForce(shootDirection * bulletSpeed, ForceMode.Impulse);
                        */

                    }
                    else
                    {
                        // bullet prefab
                        /*
                        // If the ray doesn't hit anything, shoot the bullet forward from the muzzle
                        GameObject bullet = Instantiate(bulletPrefab, muzzle.position, Quaternion.identity);

                        // Rotate the bullet to face the forward direction (transform.forward)
                        bullet.transform.rotation = Quaternion.LookRotation(transform.forward);

                        bullet.GetComponent<Rigidbody>().AddForce(transform.forward * bulletSpeed, ForceMode.Impulse);
                        */

                    }

                    Debug.Log("Fired. Ammo left: " + weaponData.currentAmmo);
                    weaponData.currentAmmo--;
                    timeSinceLastShot = 0;

                }
                else
                {
                    audioSource.PlayOneShot(emptyClipSound);
                    animator.SetBool("isEmpty", true);
                    Debug.Log("No Ammo Left");
                }

            }
        }
    }


    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        DrawRayFromMuzzle();
        ShowRayCast();
        OnFire();
        ReloadWeapon();
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
