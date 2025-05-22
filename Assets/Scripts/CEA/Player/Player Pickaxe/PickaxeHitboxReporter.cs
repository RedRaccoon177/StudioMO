using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

//곡괭이 하위 히트박스에 붙는 스크립트(광물과 상호작용한 자신에 대한 정보 반환)
public class PickaxeHitboxReporter : MonoBehaviour
{
    public string hitboxID;

    private PickaxeController controller;

    PlayerController player;

    void Awake()
    {
        player = GetComponentInParent<PlayerController>();
        controller = GetComponentInParent<PickaxeController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable") && player.RightSelectOn)
        {
            // ▼ 충돌한 오브젝트에 CollectionObject 스크립트가 있는지 체크
            CollectionObject item = other.GetComponent<CollectionObject>();

            // ▼ 없으면 return
            if (item == null) return;

            // ▼ 채집물 채집중 상태가 아니라면
            if (!item.IsCollecting)
            {
                // ▼ 채집 슬라이더 표시하고 채집 시작
                item.StartCollecting(player.transform);
            }

            controller.ReportHit(hitboxID, item);
        }
    }
}
