using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class StageButton : Button
{
    [SerializeField]
    private Image[] starImages = new Image[2];

#if UNITY_EDITOR

    protected override void OnValidate()
    {
        base.OnValidate();


        ExtensionMethod.Sort(ref starImages);
    }
#endif

    public void Set(bool? clear, UnityAction unityAction)
    {

    }
}
