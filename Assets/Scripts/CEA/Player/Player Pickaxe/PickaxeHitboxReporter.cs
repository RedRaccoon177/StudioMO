using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��� ���� ��Ʈ�ڽ��� �ٴ� ��ũ��Ʈ(������ ��ȣ�ۿ��� �ڽſ� ���� ���� ��ȯ)
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
