using UnityEngine;

public class PVPManager : Manager
{
    [SerializeField]
    private TestPhoton testPhoton;

    protected override void Awake()
    {
        base.Awake();
        Debug.Log("두 번 동작은 안 하겠지?");
        testPhoton?.Initialize();
    }

    protected override void SetupDefaultConfig()
    {
    }
}
