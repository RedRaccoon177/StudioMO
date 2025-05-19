using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PickaxeHitB : MonoBehaviour
{
    public bool isBoxBInteracted = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            isBoxBInteracted = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            isBoxBInteracted = false;
        }
    }
}
