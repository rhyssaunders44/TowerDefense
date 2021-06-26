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
        //as soon as it spawns fire off in that direction
        //bullet speed is set in the inspector as 50
        Physics.IgnoreCollision(bulletCollider, sphere);
        bulletBody.AddForce(bulletBody.transform.forward * bulletSpeed, ForceMode.Force);
    }

    //since the mook destroys the projectile and reads information based on that
    //this has to deal with all other cases
    //physic layer collisions have been ignored between most layers such as the power grid and build layer
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);

        if (collision.collider.tag != "Enemy")
        {
            Destroy(this.gameObject);
        }
    }

}
