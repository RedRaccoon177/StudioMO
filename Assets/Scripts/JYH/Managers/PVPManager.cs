using UnityEngine;

public class PVPManager : Manager
{
    [SerializeField]
    private TestPhoton testPhoton;

    protected override void Awake()
    {
        base.Awake();
        if (Application.isPlaying == true)
        {
            testPhoton?.Initialize();
        }
    }

    protected override void SetupDefaultConfig()
    {
    }
}
