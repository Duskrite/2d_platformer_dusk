using UnityEngine;
using System.Collections.Generic;

public class EnemyLoot : MonoBehaviour
{
    public List<LootItem> lootItems;

    public void SpawnLoot()
    {
        foreach (LootItem item in lootItems)
        {
            if (Random.Range(0f, 100f) <= item.dropChance)
            {
                GameObject droppedLoot = Instantiate(item.itemPrefab, transform.position, Quaternion.identity);
                break;
            }
        }
    }

    private void OnDestroy()
    {
        SpawnLoot();
    }
}
