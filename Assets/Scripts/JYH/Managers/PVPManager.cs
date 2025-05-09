using UnityEngine;

public class PVPManager : Manager
{
    [SerializeField]
    private TestPhoton testPhoton;

    protected override void Awake()
    {
        base.Awake();
        Debug.Log("�� �� ������ �� �ϰ���?");
        testPhoton?.Initialize();
    }

    protected override void SetupDefaultConfig()
    {
    }
}
