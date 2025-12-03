using UnityEngine;

public class Autogun : MonoBehaviour
{
    public float fireRate = 3f;
    public float bulletSpeed = 30f;
    private float nextFireTime = 0f;

    public GameObject bulletPrefab;

    void Update()
    {
        // Check if it's time to shoot again
        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        bullet.GetComponent<Rigidbody2D>().linearVelocity = transform.up * bulletSpeed;
    }
}
