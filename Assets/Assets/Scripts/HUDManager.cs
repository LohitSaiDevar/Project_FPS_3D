using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; set; }

    [Header("Ammo")]
    public TextMeshProUGUI magazineAmmoUI;
    public TextMeshProUGUI totalAmmoUI;
    public Image ammoTypeUI;

    [Header("Weapon")]
    public Image activeWeaponUI;
    public Image inActiveWeaponUI;

    [Header("Throwables")]
    public Image lethalUI;
    public TextMeshProUGUI lethalAmountUI;

    public Image tacticalUI;
    public TextMeshProUGUI tacticalAmountUI;

    public Sprite emptySlot;
    public GameObject middlePoint;
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

    // Update is called once per frame
    void Update()
    {
        Weapon activeWeapon = WeaponManager.Instance.activeWeaponSlot.GetComponentInChildren<Weapon>();
        Weapon inActiveWeapon = GetInActiveWeaponSlot().GetComponentInChildren<Weapon>();
        if (activeWeapon)
        {
            magazineAmmoUI.text = "" + activeWeapon.bulletsLeft/activeWeapon.bulletsPerBurst;
            totalAmmoUI.text = "" + WeaponManager.Instance.CheckAmmoLeftFor(activeWeapon.thisWeaponModel);

            Weapon.WeaponModel model = activeWeapon.thisWeaponModel;
            ammoTypeUI.sprite = GetAmmoSprite(model);
            activeWeaponUI.sprite = GetWeaponSprite(model);

            if (inActiveWeapon)
            {
                inActiveWeaponUI.sprite = GetWeaponSprite(inActiveWeapon.thisWeaponModel);
            }
        }
        else
        {
            magazineAmmoUI.text = "";
            totalAmmoUI.text = "";

            ammoTypeUI.sprite = emptySlot;
            activeWeaponUI.sprite = emptySlot;
            inActiveWeaponUI.sprite = emptySlot;
        }
    }

    private Sprite GetWeaponSprite(Weapon.WeaponModel model)
    {
        switch(model) 
        {
            case Weapon.WeaponModel.Pistol1911:
                GameObject weaponSpriteObject = Resources.Load<GameObject>("Pistol1911_Weapon");
                if (weaponSpriteObject != null)
                {
                    return weaponSpriteObject.GetComponent<SpriteRenderer>().sprite;
                }
                return null;

            case Weapon.WeaponModel.AK74:
                GameObject weaponSpriteObject1 = Resources.Load<GameObject>("AK74_Weapon");
                if (weaponSpriteObject1 != null)
                {
                    return weaponSpriteObject1.GetComponent<SpriteRenderer>().sprite;
                }
                return null;

            default:
                return null;
        }
    }

    private Sprite GetAmmoSprite(Weapon.WeaponModel model)
    {
        switch (model)
        {
            case Weapon.WeaponModel.Pistol1911:
                GameObject weaponSpriteObject = Resources.Load<GameObject>("Pistol_Ammo");
                if (weaponSpriteObject != null)
                {
                    return weaponSpriteObject.GetComponent<SpriteRenderer>().sprite;
                }
                return null;

            case Weapon.WeaponModel.AK74:
                GameObject weaponSpriteObject1 = Resources.Load<GameObject>("Rifle_Ammo");
                if (weaponSpriteObject1 != null)
                {
                    return weaponSpriteObject1.GetComponent<SpriteRenderer>().sprite;
                }
                return null;

            default:
                return null;
        }
    }

    public GameObject GetInActiveWeaponSlot()
    {
        foreach (GameObject weaponSlot in WeaponManager.Instance.weaponSlots)
        {
            if (weaponSlot != WeaponManager.Instance.activeWeaponSlot)
            {
                return weaponSlot;
            }
        }
        return null;
    }

    internal void UpdateThrowables(Throwable.ThrowableType throwable)
    {
        switch (throwable)
        {
            case Throwable.ThrowableType.Grenade:
                lethalAmountUI.text = "" + WeaponManager.Instance.grenades;
                lethalUI.sprite = Resources.Load<GameObject>("Grenade").GetComponent<SpriteRenderer>().sprite;
                break;
        }
    }
}
