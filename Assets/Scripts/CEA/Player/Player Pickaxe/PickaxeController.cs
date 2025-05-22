using System.Collections.Generic;
using UnityEngine;

//��� ��Ʈ�ڽ��� �Ѱ��Ͽ� ��ȣ�ۿ� ���� ��ü�� ��ȣ�ۿ��ϴ� ��ũ��Ʈ
public class PickaxeController : MonoBehaviour
{
    [Header("��"),SerializeField]
    private PickaxeHitboxReporter pick;
    [Header("Ȩ"), SerializeField]
    private PickaxeHitboxReporter eye;
    [Header("��"),SerializeField]
    private PickaxeHitboxReporter chisel;

    //ä���� �̳׶� ������ ��ȯ�ϴ� �Լ�
    public uint GetMineralCount()
    {
        uint value = 0;
        Dictionary<Mineral, bool> minerals = new Dictionary<Mineral, bool>();
        if (pick != null)
        {
            IEnumerable<Mineral> contents = pick.GetMinerals();
            if (contents != null)
            {
                foreach (Mineral mineral in contents)
                {
                    if (minerals.ContainsKey(mineral) == false)
                    {
                        minerals.Add(mineral, false);
                    }
                }
            }
        }
        if (chisel != null)
        {
            IEnumerable<Mineral> contents = chisel.GetMinerals();
            if (contents != null)
            {
                foreach (Mineral mineral in contents)
                {
                    if (minerals.ContainsKey(mineral) == false)
                    {
                        minerals.Add(mineral, false);
                    }
                }
            }
        }
        if (eye != null)
        {
            IEnumerable<Mineral> contents = eye.GetMinerals();
            if (contents != null)
            {
                foreach (Mineral mineral in contents)
                {
                    if (minerals.ContainsKey(mineral) == true) //�ߺ��̶�� 50%
                    {
                        minerals[mineral] = true;
                    }
                    else
                    {
                        minerals.Add(mineral, false);
                    }
                }
            }
        }
        foreach(KeyValuePair<Mineral, bool> keyValuePair in minerals)
        {
            value += keyValuePair.Key.GetMineral(keyValuePair.Value);
        }
        return value;
    }
}