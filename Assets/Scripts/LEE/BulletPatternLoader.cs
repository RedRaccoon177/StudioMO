using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// BulletPatternLoader�� CSV ���Ͽ��� ź�� ���� �����͸� �о��
/// BulletPatternExecutor���� ����� �� �ֵ��� List<BulletSpawnData>�� �Ľ��ϴ� ������ �Ѵ�.
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

    /// <summary>
    /// CSV �ؽ�Ʈ�� �Ľ��Ͽ� BulletSpawnData ����Ʈ�� ��ȯ�Ѵ�.
    /// </summary>
    /// <param name="csv">CSV �ؽ�Ʈ ����</param>
    /// <returns>BulletSpawnData ����Ʈ</returns>
    List<BulletSpawnData> ParseCSV(string csv)
    {
        var lines = csv.Split('\n'); // �� ������ �и�
        var dataList = new List<BulletSpawnData>();

        for (int i = 1; i < lines.Length; i++) // 0��° ���� ����̹Ƿ� ��ŵ
        {
            var line = lines[i];
            if (string.IsNullOrWhiteSpace(line)) continue; // �� ���� �ǳʶڴ�

            var cols = line.Split(','); // ��ǥ�� �� ����
            if (cols.Length < 8) continue; // �� ������ �����ϸ� �ǳʶ� (������ �ջ� ����)


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

            dataList.Add(data); // ���� ����Ʈ�� �߰�
        }

        return dataList;
    }
}
