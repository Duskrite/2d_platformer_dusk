using System;
using UnityEngine;

public class SpeedBoostItem : MonoBehaviour, IItem
{
    public float speedBoostMultiplier = 1.5f;
    public float speedBoostDuration = 4f;
    public static event Action<float, float> OnSpeedBoostItemCollected;
    public GameObject afterVFX;

    public void Collect()
    {
        OnSpeedBoostItemCollected.Invoke(speedBoostMultiplier, speedBoostDuration);

        if (afterVFX != null)
        {
            Instantiate(afterVFX, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
