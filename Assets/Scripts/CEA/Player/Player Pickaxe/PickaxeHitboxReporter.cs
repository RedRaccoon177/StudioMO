using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//곡괭이 하위 히트박스에 붙는 스크립트(광물과 상호작용한 자신에 대한 정보 반환)
public class PickaxeHitboxReporter : MonoBehaviour
{
    public string hitboxID;

    private PickaxeController controller;

    private CollectionObject item;

    PlayerController player;

    void Start()
    {
        player = GetComponentInParent<PlayerController>();
        controller = GetComponentInParent<PickaxeController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable") && player.RightSelectOn)
        {
            item = other.gameObject.GetComponent<CollectionObject>();  
            controller.ReportHit(hitboxID , item);
        }
    }
}
