using UnityEngine;
using System.Collections;
using System.Linq;

public class BulletPatternExecutor : MonoBehaviour
{
    public BulletPatternLoader loader;                // CSV에서 불러온 데이터
    public BulletSpawnerManager spawnerManager;       // 탄막 매니저
    public float bpm = 110f;                          // beat per minute

    private float beatInterval;                       // 한 beat 당 시간
    private float timer;
    private int currentBeatIndex = 0;

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
        if (string.IsNullOrWhiteSpace(raw) || raw.Trim().ToLower() == "null")
            return new int[0];

        return raw.Replace("\"", "") // 쌍따옴표 제거
                  .Split(',')
                  .Select(s => {
                      if (int.TryParse(s.Trim(), out int val)) return val;
                      Debug.LogWarning($"[ParseSides] 유효하지 않은 숫자 값: '{s}'");
                      return -1;
                  })
                  .Where(val => val >= 0)
                  .ToArray();
    }

}
