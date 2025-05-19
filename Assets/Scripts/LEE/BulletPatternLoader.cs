using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// BulletPatternLoader는 CSV 파일에서 탄막 패턴 데이터를 읽어와
/// BulletPatternExecutor에서 사용할 수 있도록 List<BulletSpawnData>로 파싱하는 역할을 한다.
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

    /// <summary>
    /// CSV 텍스트를 파싱하여 BulletSpawnData 리스트로 변환한다.
    /// </summary>
    /// <param name="csv">CSV 텍스트 원문</param>
    /// <returns>BulletSpawnData 리스트</returns>
    List<BulletSpawnData> ParseCSV(string csv)
    {
        var lines = csv.Split('\n'); // 줄 단위로 분리
        var dataList = new List<BulletSpawnData>();

        for (int i = 1; i < lines.Length; i++) // 0번째 줄은 헤더이므로 스킵
        {
            var line = lines[i];
            if (string.IsNullOrWhiteSpace(line)) continue; // 빈 줄은 건너뛴다

            var cols = line.Split(','); // 쉼표로 열 구분
            if (cols.Length < 8) continue; // 열 개수가 부족하면 건너뜀 (데이터 손상 방지)


            if (!int.TryParse(cols[0].Trim(), out int beatIndex)) continue;
            if (!bool.TryParse(cols[1].Trim(), out bool generateA)) continue;
            if (!bool.TryParse(cols[4].Trim(), out bool generateB)) continue;

            BulletSpawnData data = new BulletSpawnData
            {
                beatIndex = beatIndex,
                generateA = generateA,
                aGenerateSide = cols[2].Trim(),
                aGenerateAmount = int.TryParse(cols[3], out var aAmt) ? aAmt : 0,
                generateB = generateB,
                bGenerateSide = cols[5].Trim(),
                bGenerateAmount = int.TryParse(cols[6], out var bAmt) ? bAmt : 0
            };

            dataList.Add(data); // 최종 리스트에 추가
        }

        return dataList;
    }
}
