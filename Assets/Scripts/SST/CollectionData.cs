using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CollectionData", menuName = "Scriptable Object/CollectionData", order = 1)]
public class CollectionData : ScriptableObject
{
    public string collectionName;
    public int score;
    public GameObject collectionPrefab;
}
