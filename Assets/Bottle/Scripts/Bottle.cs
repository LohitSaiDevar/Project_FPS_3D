using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : MonoBehaviour
{
    [SerializeField] GameObject brokenBottlePrefab;
    
    void Update() // just for testing
    {
        
    }
    
    public void Explode(GameObject gameObject)
    {
        GameObject brokenBottle = Instantiate(brokenBottlePrefab, gameObject.transform.position, Quaternion.identity);
        brokenBottle.GetComponent<BrokenBottle>().RandomVelocities();
        Destroy(gameObject);
    }
}
