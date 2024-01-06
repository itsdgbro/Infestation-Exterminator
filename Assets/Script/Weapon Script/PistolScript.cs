using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolScript : MonoBehaviour
{
    private PlayerControls playerControls;

    #region Gun_Parameters
    [SerializeField] private float damage = 10f;
    [SerializeField] private float range = 50f;
    [SerializeField] private int ammo = 7;
    #endregion

    #region RayCast Muzzle
    public Transform muzzle;
    public float rayLength = 10f;
    public Color rayColor = Color.red;
    #endregion


    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void CastRayMuzzle()
    {
        Ray ray = new Ray(muzzle.transform.position, transform.forward);
        bool hitTarget = Physics.Raycast(ray, out RaycastHit hitInfo, rayLength);

        Debug.DrawRay(ray.origin, ray.direction * (hitTarget ? hitInfo.distance : rayLength), hitTarget && hitInfo.collider.CompareTag("Target") ? Color.green : rayColor);

        if (hitTarget)
        {
            if (hitInfo.collider.CompareTag("Target"))
            {
                Debug.Log("Raycast hit a Target: " + hitInfo.collider.gameObject.name);
                Target target = hitInfo.collider.gameObject.GetComponent<Target>();
                target?.TakeDamage(damage);
            }
            else
            {
                Debug.Log("Raycast hit: " + hitInfo.collider.gameObject.name);
            }
        }
    }


    private void OnFire()
    {
        if (playerControls.Movement.Semi_Fire.triggered)
        {
            if (ammo > 0)
            {

                CastRayMuzzle();
                Debug.Log("Fired. Ammo left: " + ammo);
                ammo--;
            }
            else
            {
                Debug.Log("No Ammo Left");
            }
        }
    }


    private void Update()
    {
        OnFire();
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
