using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class BulletPatternExecutor : MonoBehaviour
{
    public BulletPatternLoader loader;                // CSV에서 불러온 데이터
    public BulletSpawnerManager spawnerManager;       // 탄막 매니저
    
    [Header("분당 BPM")]public float bpm = 110f;

    private float beatInterval;                       // 한 beat 당 시간
    private float timer;
    private int currentBeatIndex = 1;

    void Start()
    {
        beatInterval = 60f / bpm;
        Debug.Log($"[Executor] beatInterval = {beatInterval}");
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= beatInterval)
        {
            timer -= beatInterval;

            // 현재 beat에 해당하는 데이터만 실행
            var matches = loader.patternData
                .Where(d => d.beatIndex == currentBeatIndex)
                .ToList();

            foreach (var data in matches)
            {
                ExecuteBeat(data, currentBeatIndex);
            }

            currentBeatIndex++;
        }
    }

    void ExecuteBeat(BulletSpawnData data, int beatIndex)
    {
        if (data.generateA)
        {
            foreach (int side in ParseSides(data.aGenerateSide))
            {
                spawnerManager.SpawnNormal(side, data.aGenerateAmount);
            }
        }

        if (data.generateB)
        {
            foreach (int side in ParseSides(data.bGenerateSide))
            {
                spawnerManager.SpawnGuided(side, data.bGenerateAmount);
            }
        }
    }

    int[] ParseSides(string raw)
    {
        if (string.IsNullOrWhiteSpace(raw)) return new int[0];

        List<int> result = new();

        if (raw.Contains("1")) result.Add(1);  // 아래
        if (raw.Contains("2")) result.Add(2);  // 왼쪽
        if (raw.Contains("3")) result.Add(3);  // 오른쪽
        if (raw.Contains("4")) result.Add(4);  // 위쪽

        return result.ToArray();
    }

}
