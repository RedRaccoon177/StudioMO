using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeHitA : MonoBehaviour
{
    public bool isBoxAInteracted = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            isBoxAInteracted = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            isBoxAInteracted = false;
        }
    }
}
