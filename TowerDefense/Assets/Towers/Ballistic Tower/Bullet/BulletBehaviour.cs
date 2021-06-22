using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    [SerializeField] private Rigidbody bulletBody;
    [SerializeField] private Collider bulletCollider;
    [SerializeField] public static GameObject Bullet;
    [SerializeField] private int bulletSpeed;
    [SerializeField] private Collider sphere;

    void Start()
    {
        Physics.IgnoreCollision(bulletCollider, sphere);
        bulletBody.AddForce(bulletBody.transform.forward * bulletSpeed, ForceMode.Force);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.collider.tag);
        if (collision.collider.tag != "Enemy")
        {
            Destroy(this.gameObject);
        }
    }

}
