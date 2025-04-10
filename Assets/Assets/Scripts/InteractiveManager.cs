using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractiveManager : MonoBehaviour
{
    public static InteractiveManager Instance { get; set; }
    Weapon hoveredWeapon = null;
    AmmoBox hoveredAmmoBox = null;
    Throwable hoveredThrowable = null;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            GameObject objectHitByRay = hit.transform.gameObject;
            if (objectHitByRay.GetComponent<Weapon>() && objectHitByRay.GetComponent<Weapon>().isActiveWeapon == false)
            {
                hoveredWeapon = objectHitByRay.GetComponent<Weapon>();
                hoveredWeapon.GetComponent<Outline>().enabled = true;
                if (Input.GetKeyDown(KeyCode.F))
                {
                    WeaponManager.Instance.PickupWeapon(objectHitByRay.gameObject);
                }
            }
            else
            {
                if (hoveredWeapon)
                {
                    hoveredWeapon.GetComponent<Outline>().enabled = false;
                }
            }

            //AmmoBox
            if (objectHitByRay.GetComponent<AmmoBox>())
            {
                hoveredAmmoBox = objectHitByRay.GetComponent<AmmoBox>();
                hoveredAmmoBox.GetComponent<Outline>().enabled = true;
                if (Input.GetKeyDown(KeyCode.F))
                {
                    WeaponManager.Instance.PickupAmmo(hoveredAmmoBox);
                }
            }
            else
            {
                if (hoveredAmmoBox)
                {
                    hoveredAmmoBox.GetComponent<Outline>().enabled = false;
                }
            }

            //Throwable
            if (objectHitByRay.GetComponent<Throwable>())
            {
                hoveredThrowable = objectHitByRay.GetComponent<Throwable>();
                hoveredThrowable.GetComponent<Outline>().enabled = true;
                if (Input.GetKeyDown(KeyCode.F))
                {
                    WeaponManager.Instance.PickupThrowable(hoveredThrowable);
                    Destroy(objectHitByRay);
                }
            }
            else
            {
                if (hoveredThrowable)
                {
                    hoveredThrowable.GetComponent<Outline>().enabled = false;
                }
            }
        }
    }
}
