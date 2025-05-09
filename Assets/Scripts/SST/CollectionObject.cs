using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionObject : MonoBehaviour
{
    private CollectionData collectionData;
    private CollectionPool pools;

    public void InitializeCollectionObject(CollectionData data, CollectionPool pool)
    {
        collectionData = data;
        pools = pool;
    }
}
