using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public bool isActiveWeapon;

    [Header("Shoot")]
    bool isShooting, readyToShoot;
    bool allowReset = true;
    [SerializeField] float shootingDelay = 2;

    [Header("Burst")]
    public int bulletsPerBurst = 3;
    [SerializeField] int burstBulletsLeft;

    [Header("Spread")]
    [SerializeField] float spreadIntensity;
    [SerializeField] float hipSpreadIntensity;
    [SerializeField] float adsSpreadIntensity;

    [Header("Bullet")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform bulletSpawn;
    [SerializeField] float bulletVelocity;
    [SerializeField] float bulletPrefabLifeTime;

    [SerializeField] GameObject muzzleEffect;
    internal Animator animator;

    [Header("Loading")]
    [SerializeField] float reloadTime;
    public int magazineSize, bulletsLeft;
    [SerializeField] bool isReloading;

    public Vector3 spawnPos;
    public Vector3 spawnRotation;
    bool isADS;
    public enum WeaponModel
    {
        Pistol1911,
        AK74
    }
    public WeaponModel thisWeaponModel;
    public enum ShootingMode
    {
        Single,
        Burst,
        Auto
    }

    public ShootingMode currentShootingMode;

    void Awake()
    {
        animator = GetComponent<Animator>();
        readyToShoot = true;
        burstBulletsLeft = bulletsPerBurst;
        bulletsLeft = magazineSize;
        spreadIntensity = hipSpreadIntensity;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActiveWeapon)
        {
            GetComponent<BoxCollider>().enabled = false;
            if (Input.GetMouseButtonDown(1))
            {
                animator.SetTrigger("enterADS");
                isADS = true;
                HUDManager.Instance.middlePoint.SetActive(false);
                spreadIntensity = adsSpreadIntensity;
                
            }
            if(Input.GetMouseButtonUp(1))
            {
                animator.SetTrigger("exitADS");
                isADS = false;
                HUDManager.Instance.middlePoint.SetActive(true);
                spreadIntensity = hipSpreadIntensity;
            }

            GetComponent<Outline>().enabled = false;
            if (bulletsLeft == 0 && isShooting)
            {
                SoundManager.Instance.emptyMagSoundPistol.Play();
            }
            if (currentShootingMode == ShootingMode.Auto)
            {
                isShooting = Input.GetKey(KeyCode.Mouse0);
            }
            else if (currentShootingMode == ShootingMode.Single || currentShootingMode == ShootingMode.Burst)
            {
                isShooting = Input.GetKeyDown(KeyCode.Mouse0);
            }

            if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !isReloading && WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel) > 0)
            {
                Reload();
            }
            if (readyToShoot && !isShooting && !isReloading && bulletsLeft <= 0)
            {
                //Reload();
            }
            if (readyToShoot && isShooting && bulletsLeft > 0)
            {
                burstBulletsLeft = bulletsPerBurst;
                FireWeapon();
            }
        }
        else
        {
            GetComponent<BoxCollider>().enabled = true;
        }
    }
    void EnterADS()
    {
        animator.SetTrigger("enterADS");
        isADS = true;
        HUDManager.Instance.middlePoint.SetActive(false);
        spreadIntensity = adsSpreadIntensity;
    }
    void ExitADS()
    {
        animator.SetTrigger("exitADS");
        isADS = false;
        HUDManager.Instance.middlePoint.SetActive(true);
        spreadIntensity = hipSpreadIntensity;
    }
    private void FireWeapon()
    {
        bulletsLeft--;
        if (isADS)
        {
            animator.SetTrigger("RECOIL_ADS");
        }
        else
        {
            animator.SetTrigger("RECOIL");
        }
        SoundManager.Instance.ShootingSoundMode(thisWeaponModel);
        muzzleEffect.GetComponent<ParticleSystem>().Play();
        readyToShoot = false;
        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

        bullet.transform.forward = shootingDirection;
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime));

        if (allowReset)
        {
            Invoke(nameof(ResetShot), shootingDelay);
            allowReset = false;
        }
        if (currentShootingMode == ShootingMode.Burst && bulletsPerBurst > 1)
        {
            burstBulletsLeft--;
            Invoke(nameof(FireWeapon), shootingDelay);
        }
    }

    void Reload()
    {
        animator.SetTrigger("RELOAD");
        SoundManager.Instance.ReloadingSoundMode(thisWeaponModel);
        isReloading = true;
        Invoke(nameof(ReloadCompleted), reloadTime);
    }

    void ReloadCompleted()
    {
        if (WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel) > magazineSize)
        {
            bulletsLeft = magazineSize;
            WeaponManager.Instance.DecreaseTotalAmmo(bulletsLeft, thisWeaponModel);
        }
        else
        {
            bulletsLeft = WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel);
            WeaponManager.Instance.DecreaseTotalAmmo(bulletsLeft, thisWeaponModel);
        }
        isReloading = false;
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    private Vector3 CalculateDirectionAndSpread()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = targetPoint - bulletSpawn.position;
        float z = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        return direction + new Vector3(0, y, z);
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }

    
}
