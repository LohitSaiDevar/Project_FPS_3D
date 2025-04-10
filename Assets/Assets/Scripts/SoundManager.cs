using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Weapon;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }

    public AudioSource shootingChannel;
    public AudioClip shootingSoundPistol1911;
    public AudioClip shootingSoundAK74;

    public AudioSource reloadingSoundPistol;
    public AudioSource reloadingSoundAK74;
    public AudioSource emptyMagSoundPistol;

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

    public void ShootingSoundMode(WeaponModel type)
    {
        switch (type)
        {
            case WeaponModel.Pistol1911:
                shootingChannel.PlayOneShot(shootingSoundPistol1911);
                break;

            case WeaponModel.AK74:
                shootingChannel.PlayOneShot(shootingSoundAK74);
                break;
        }
    }

    public void ReloadingSoundMode(WeaponModel type)
    {
        switch (type)
        {
            case WeaponModel.Pistol1911:
                reloadingSoundPistol.Play();
                break;

            case WeaponModel.AK74:
                reloadingSoundAK74.Play();
                break;
        }
    }
}
