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
        // Create a ray starting from the object's position and facing forward
        Ray ray = new Ray(muzzle.transform.position, transform.forward);

        // Perform the raycast
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, rayLength))
        {
            Debug.DrawRay(ray.origin, ray.direction * hitInfo.distance, Color.green);
            Debug.Log("Raycast hit: " + hitInfo.collider.gameObject.name);
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction * rayLength, rayColor);
        }
    }

    private void OnFire()
    {
        if (playerControls.Movement.Semi_Fire.triggered)
        {
            if (ammo > 0)
            {
                Debug.Log("Fired. Ammo left: " + ammo);
                // Perform shooting logic here

                // Decrease ammo after firing
                ammo--;
            }
            else
            {
                Debug.Log("No Ammo Left");
                // Handle out-of-ammo situation
            }
        }
    }


    private void Update()
    {
        CastRayMuzzle();
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
