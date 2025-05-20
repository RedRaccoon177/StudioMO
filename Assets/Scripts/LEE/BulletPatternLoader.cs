using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

/// <summary>
/// BulletPatternLoader�� CSV ���Ͽ��� ź�� ���� �����͸� �о�ͼ�
/// BulletPatternExecutor���� ����� �� �ֵ��� List<BulletSpawnData>�� �Ľ��ϴ� ������ ��.
/// </summary>
public class BulletPatternLoader : MonoBehaviour
{
    [Header("ź�� ���� CSV ����")]
    [Tooltip("Resources ���� ���� �ִ� CSV ���� (TextAsset ����)")]
    public TextAsset csvFile;

    [Header("�Ľ̵� ź�� ���� ������ ����Ʈ")]
    [Tooltip("BulletSpawnData ������ �Ľ̵� ź�� Ÿ�̹� ����")]
    public List<BulletSpawnData> patternData = new();

    /// <summary>
    /// ������ ���۵Ǹ� CSV ������ �Ľ��Ͽ� patternData ����Ʈ�� �ʱ�ȭ�Ѵ�.
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

            // 7���� �� �Ǹ� ����� �е� �߰�
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

        Debug.Log($"[CSV] �Ľ� �Ϸ�: {dataList.Count}��");
        return dataList;
    }


}
