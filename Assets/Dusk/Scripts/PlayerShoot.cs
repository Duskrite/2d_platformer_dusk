using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerShoot : MonoBehaviour
{
    public float bulletSpeed = 50f;
    public GameObject bulletPrefab;

    public void Shoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // Get mouse position in world coordinates
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Calculate direction from player to mouse position
            Vector3 shootDirection = (mousePosition - transform.position).normalized;

            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            // bullet.GetComponent<Bullet>.damage = 1;

            bullet.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(shootDirection.x, shootDirection.y) * bulletSpeed;
        }
    }
}