using UnityEngine;

public class PVPManager : Manager
{
    [SerializeField]
    private TestPhoton testPhoton;

    protected override void Initialize()
    {
        testPhoton?.Initialize();
    }

}
