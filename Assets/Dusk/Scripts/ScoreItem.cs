using System;
using UnityEngine;

public class ScoreItem : MonoBehaviour, IItem
{
    public int score = 5;
    public static event Action<int> OnScoreItemCollected;
    public GameObject afterVFX;

    public void Collect()
    {
        OnScoreItemCollected.Invoke(score);

        if (afterVFX != null)
        {
            Instantiate(afterVFX, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
