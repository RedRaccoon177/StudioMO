using UnityEngine;

[DisallowMultipleComponent]
[AddComponentMenu("BulletSystem/SideTag")]
public class SideTag : MonoBehaviour
{
    [Tooltip("�� �����ʰ� ����� ��ġ �ε��� (��: 2=Bottom, 4=Left, 6=Right, 8=Top)")]
    public int side;
}
