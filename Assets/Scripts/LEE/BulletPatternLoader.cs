using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

/// <summary>
/// BulletPatternLoader는 CSV 파일에서 탄막 패턴 데이터를 읽어와서
/// BulletPatternExecutor에서 사용할 수 있도록 List<BulletSpawnData>로 파싱하는 역할을 함.
/// </summary>
public class BulletPatternLoader : MonoBehaviour
{
    [Header("탄막 패턴 CSV 파일")]
    [Tooltip("Resources 폴더 내에 있는 CSV 파일 (TextAsset 형식)")]
    public TextAsset csvFile;

    [Header("파싱된 탄막 패턴 데이터 리스트")]
    [Tooltip("BulletSpawnData 구조로 파싱된 탄막 타이밍 정보")]
    public List<BulletSpawnData> patternData = new();

    /// <summary>
    /// 게임이 시작되면 CSV 파일을 파싱하여 patternData 리스트를 초기화한다.
    /// </summary>
    void Awake()
    {
        patternData = ParseCSV(csvFile.text);
    }

    List<BulletSpawnData> ParseCSV(string csv)
    {
        var lines = csv.Split('\n');
        var dataList = new List<BulletSpawnData>();

        for (int i = 1; i < lines.Length; i++)
        {
            var line = lines[i].Trim();
            if (string.IsNullOrWhiteSpace(line)) continue;

            var cols = line.Replace("\"", "").Split(',');

            // 7열이 안 되면 비워서 패딩 추가
            while (cols.Length < 7)
            {
                Array.Resize(ref cols, cols.Length + 1);
                cols[cols.Length - 1] = "";
            }

            if (!int.TryParse(cols[0].Trim(), out int beatIndex)) continue;

            bool generateA = cols[1].Trim().ToLower() == "true";
            string aSide = generateA ? cols[2].Trim() : "";
            int aAmount = generateA && int.TryParse(cols[3].Trim(), out int aAmt) ? aAmt : 0;

            bool generateB = cols[4].Trim().ToLower() == "true";
            string bSide = generateB ? cols[5].Trim() : "";
            int bAmount = generateB && int.TryParse(cols[6].Trim(), out int bAmt) ? bAmt : 0;

            BulletSpawnData data = new BulletSpawnData
            {
                beatIndex = beatIndex,
                generateA = generateA,
                aGenerateSide = aSide,
                aGenerateAmount = aAmount,
                generateB = generateB,
                bGenerateSide = bSide,
                bGenerateAmount = bAmount
            };

            dataList.Add(data);
        }

        Debug.Log($"[CSV] 파싱 완료: {dataList.Count}줄");
        return dataList;
    }


}
