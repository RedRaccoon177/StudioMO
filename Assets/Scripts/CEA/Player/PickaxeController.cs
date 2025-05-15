using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeController : MonoBehaviour
{
    PlayerController player;

    private void Start()
    {
        player = GetComponentInParent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable") && player.RightSelectOn)
        {
            Debug.Log("Ã¤Áý");
        }
    }
}
