using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeController : MonoBehaviour
{
    PlayerController player;

    CollectionObject item;

    float collectGage = 25;

    private void Start()
    {
        player = GetComponentInParent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable") && player.RightSelectOn)
        {
            Debug.Log("채집");
            item = other.gameObject.GetComponent<CollectionObject>();

            if(item != null)
            {
                item._Slider.value += collectGage;

                if (item._Slider.value >= 100)
                {
                    Debug.Log("채집 완료");
                }
            }
        }
    }
}
