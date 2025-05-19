using UnityEngine;

[DisallowMultipleComponent]
[AddComponentMenu("BulletSystem/SideTag")]
public class SideTag : MonoBehaviour
{
    [Tooltip("이 스포너가 담당할 위치 인덱스 (예: 2=Bottom, 4=Left, 6=Right, 8=Top)")]
    public int side;
}
