using UnityEngine;

public class Autogun2 : MonoBehaviour
{
    public float fireRate = 10f;
    public float firingTimer = 0f;
    public float notFiringTimer = 0f;
    public float fireDuration = 5f;


    public bool isFiring = false;

    public GameObject laser;

    private void Update()
    {
        if (!isFiring && notFiringTimer >= fireRate)
        {
            isFiring = true;
            laser.SetActive(true);
            firingTimer = 0f;
        }

        if (isFiring)
        {
            firingTimer += Time.deltaTime;
            if (firingTimer >= fireDuration)
            {
                isFiring = false;
                laser.SetActive(false);
                notFiringTimer = 0f;
            }
        }
        else
        {
            notFiringTimer += Time.deltaTime;
        }
    }
}
