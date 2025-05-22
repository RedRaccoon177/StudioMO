using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mineral : MonoBehaviour
{
    [Header("광물 최대한도")]
    [SerializeField] uint maxValue;
    [Header("1회 채취시 획득량")]
    [SerializeField] uint getAmount;
    [Header("광물 소진 시 충전시간")]
    [SerializeField] float chargeTime;
}
