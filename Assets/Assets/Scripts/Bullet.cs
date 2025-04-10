using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Bottle bottlePrefab;

    private void Awake()
    {
        bottlePrefab = GameObject.FindGameObjectWithTag("Bottle").GetComponent<Bottle>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            Debug.Log("Hit " + collision.gameObject.name + "!");
            Destroy(gameObject);
            CreateBulletImpactEffect(collision);
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Hit a wall!");
            Destroy(gameObject);
            CreateBulletImpactEffect(collision);
        }
        if (collision.gameObject.CompareTag("Bottle"))
        {
            bottlePrefab.Explode(collision.gameObject);
        }
    }

    private void CreateBulletImpactEffect(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];

        GameObject hole = Instantiate(GlobalReferences.Instance.bulletImpactEffectPrefab,
            contact.point,
            Quaternion.LookRotation(contact.normal)
            );

        hole.transform.SetParent(collision.gameObject.transform);
    }
}
