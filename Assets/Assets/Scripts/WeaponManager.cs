using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Weapon;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; set; }
    public List<GameObject> weaponSlots;

    public GameObject activeWeaponSlot;

    [Header("Ammo")]
    public int totalPistolAmmo = 0;
    public int totalRifleAmmo = 0;

    [Header("Throwables")]
    public int grenades;
    public float throwForce = 10;
    public float forceMultiplier;
    public float forceMultiplierLimit = 2;
    public GameObject grenadePrefab;
    public GameObject throwableSpawn;
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

    private void Start()
    {
        activeWeaponSlot = weaponSlots[0];
    }

    private void Update()
    {
        foreach (GameObject weaponSlot in weaponSlots)
        {
            if (weaponSlot == activeWeaponSlot)
            {
                weaponSlot.SetActive(true);
            }
            else
            {
                weaponSlot.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchActiveSlot(0);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchActiveSlot(1);
        }

        if (Input.GetKey(KeyCode.G))
        {
            forceMultiplier += Time.deltaTime;
            if (forceMultiplier > forceMultiplierLimit)
            {
                forceMultiplier = forceMultiplierLimit;
            }
        }
        if(Input.GetKeyUp(KeyCode.G))
        {
            if (grenades > 0)
            {
                ThrowLethal();
            }
            forceMultiplier = 0;
        }
    }

    

    public void PickupWeapon(GameObject pickedUpWeapon)
    {
        AddWeaponIntoActiveSlot(pickedUpWeapon);
    }

    void AddWeaponIntoActiveSlot(GameObject pickedUpWeapon)
    {
        DropCurrentWeapon(pickedUpWeapon);
        pickedUpWeapon.transform.SetParent(activeWeaponSlot.transform, false);

        Weapon weapon = pickedUpWeapon.GetComponent<Weapon>();
        pickedUpWeapon.transform.localPosition = new Vector3(weapon.spawnPos.x, weapon.spawnPos.y, weapon.spawnPos.z);
        pickedUpWeapon.transform.localRotation = Quaternion.Euler(weapon.spawnRotation.x, weapon.spawnRotation.y, weapon.spawnRotation.z);

        weapon.isActiveWeapon = true;
        weapon.animator.enabled = true;
    }

    void DropCurrentWeapon(GameObject pickedUpWeapon)
    {
        if (activeWeaponSlot.transform.childCount > 0)
        {
            var weaponToDrop = activeWeaponSlot.transform.GetChild(0).gameObject;
            
            weaponToDrop.GetComponent<Weapon>().isActiveWeapon = false;
            weaponToDrop.GetComponent<Weapon>().animator.enabled = false;

            weaponToDrop.transform.SetParent(pickedUpWeapon.transform.parent);
            weaponToDrop.transform.localPosition = pickedUpWeapon.transform.localPosition;
            weaponToDrop.transform.localRotation = pickedUpWeapon.transform.localRotation;
        }
    }

    public void SwitchActiveSlot(int slotNumber)
    {
        if (activeWeaponSlot.transform.childCount > 0)
        {
            Weapon currentWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            currentWeapon.isActiveWeapon = false;
        }
        activeWeaponSlot = weaponSlots[slotNumber];

        if (activeWeaponSlot.transform.childCount > 0)
        {
            Weapon newWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            newWeapon.isActiveWeapon = true;
        }
    }

    internal void PickupAmmo(AmmoBox ammo)
    {
        switch (ammo.ammoType)
        {
            case AmmoBox.AmmoType.PistolAmmo:
                totalPistolAmmo += ammo.ammoAmount;
                break;

            case AmmoBox.AmmoType.RifleAmmo:
                totalRifleAmmo += ammo.ammoAmount;
                break;

        }
    }
    public int CheckAmmoLeftFor(WeaponModel thisWeaponModel)
    {
        switch (thisWeaponModel)
        {
            case WeaponModel.Pistol1911:
                return totalPistolAmmo;

            case WeaponModel.AK74:
                return totalRifleAmmo;

            default:
                return 0;
        }
    }
    internal void DecreaseTotalAmmo(int bulletsToDecrease, WeaponModel thisWeaponModel)
    {
        switch (thisWeaponModel)
        {
            case Weapon.WeaponModel.AK74:
                totalRifleAmmo -= bulletsToDecrease;
                break;

            case Weapon.WeaponModel.Pistol1911:
                totalPistolAmmo -= bulletsToDecrease;
                break;
        }
    }

    #region || - - - Throwables - - - ||
    internal void PickupThrowable(Throwable throwable)
    {
        switch (throwable.throwableType)
        {
            case Throwable.ThrowableType.Grenade:
                PickupGrenade();
                break;
        }
    }

    private void PickupGrenade()
    {
        grenades += 1;
        HUDManager.Instance.UpdateThrowables(Throwable.ThrowableType.Grenade);
    }

    private void ThrowLethal()
    {
        GameObject lethalPrefab = grenadePrefab;
        GameObject throwable = Instantiate(lethalPrefab, throwableSpawn.transform.position, Camera.main.transform.rotation);
        Rigidbody rb = throwable.GetComponent<Rigidbody>();

        rb.AddForce(Camera.main.transform.forward * (throwForce * forceMultiplier), ForceMode.Impulse);
        throwable.GetComponent<Throwable>().hasBeenThrown = true;

        grenades -= 1;
        HUDManager.Instance.UpdateThrowables(Throwable.ThrowableType.Grenade);
    }
    #endregion
}