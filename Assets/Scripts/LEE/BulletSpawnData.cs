using System;

[Serializable]
public class BulletSpawnData
{
    public int beatIndex; // 몇 번째 beat인지
    public bool generateA;
    public string aGenerateSide;
    public int aGenerateAmount;

    public bool generateB;
    public string bGenerateSide;
    public int bGenerateAmount;
}

