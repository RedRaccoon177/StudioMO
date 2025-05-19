using UnityEngine;
using System.Collections.Generic;

public class BulletSpawnerManager : MonoBehaviour
{
    public Dictionary<int, NormalBulletSpawner> normalSpawners = new();
    public Dictionary<int, GuidedBulletSpawner> guidedSpawners = new();

    public void SpawnNormal(int side, int amount)
    {
        if (!normalSpawners.ContainsKey(side))
        {
            Debug.LogWarning($"[SpawnNormal] side {side} 스포너 없음 - 발사 실패");
            return;
        }

        Debug.Log($"[SpawnNormal] side {side}, amount {amount}");

        var spawner = normalSpawners[side];
        for (int i = 0; i < amount; i++)
        {
            spawner.FireBullet();
        }
    }

    public void SpawnGuided(int side, int amount)
    {
        if (!guidedSpawners.ContainsKey(side)) return;
        var spawner = guidedSpawners[side];
        for (int i = 0; i < amount; i++)
        {
            spawner.FireBullet();
        }
    }
}
