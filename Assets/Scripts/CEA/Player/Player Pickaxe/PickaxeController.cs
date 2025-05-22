using System.Collections.Generic;
using UnityEngine;

//모든 히트박스를 총괄하여 상호작용 가능 물체와 상호작용하는 스크립트
public class PickaxeController : MonoBehaviour
{
    [Header("정"),SerializeField]
    private PickaxeHitboxReporter pick;
    [Header("홈"), SerializeField]
    private PickaxeHitboxReporter eye;
    [Header("끌"),SerializeField]
    private PickaxeHitboxReporter chisel;

    //채취한 미네랄 개수를 반환하는 함수
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
                    if (minerals.ContainsKey(mineral) == true) //중복이라면 50%
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