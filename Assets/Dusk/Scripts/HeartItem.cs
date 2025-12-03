using System;
using UnityEngine;

public class HeartItem : MonoBehaviour, IItem
{
    public int healthAmount = 1;
    public static event Action<int> OnHeartItemCollected;
    public GameObject afterVFX;

    public void Collect()
    {
        OnHeartItemCollected.Invoke(healthAmount);

        if (afterVFX != null)
        {
            Instantiate(afterVFX, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
