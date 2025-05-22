using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

//��� ���� ��Ʈ�ڽ��� �ٴ� ��ũ��Ʈ(������ ��ȣ�ۿ��� �ڽſ� ���� ���� ��ȯ)
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
            // �� �浹�� ������Ʈ�� CollectionObject ��ũ��Ʈ�� �ִ��� üũ
            CollectionObject item = other.GetComponent<CollectionObject>();

            // �� ������ return
            if (item == null) return;

            // �� ä���� ä���� ���°� �ƴ϶��
            if (!item.IsCollecting)
            {
                // �� ä�� �����̴� ǥ���ϰ� ä�� ����
                item.StartCollecting(player.transform);
            }

            controller.ReportHit(hitboxID, item);
        }
    }
}
