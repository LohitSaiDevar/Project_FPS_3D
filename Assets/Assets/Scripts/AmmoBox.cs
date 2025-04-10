using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    public int ammoAmount = 200;
    public enum AmmoType 
    {
        RifleAmmo,
        PistolAmmo
    }
    public AmmoType ammoType;
}
