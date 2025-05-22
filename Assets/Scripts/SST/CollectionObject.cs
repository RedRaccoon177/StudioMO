using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CollectionObject : MonoBehaviour
{
    private CollectionData collectionData;          // SO ������
    private CollectionPool pools;                   // �� ������Ʈ ��ȯ�� ������Ʈ Ǯ ����
    private CollectionSpawner spawner;

    private Transform playerTransform;              // �÷��̾�� �Ÿ� ��� ����

    [SerializeField] private Collider selfColider;  // ä�� �÷��̾� ��ȣ�ۿ� üũ �ݶ��̴�
    [SerializeField] private CollectionUICtrl ui;   // �����̴� UI ��Ʈ�ѷ� ����

    private float currentGage = 0f;                 // ä�� �����̴� ������ 0 ~ 100
    private float coolTime = 0.5f;                  // ä�� �ݶ��̴� ��Ʈ ��Ÿ��
    private float gagePerHit = 10f;                 // ���� �� ������ ��

    private bool isCoolTime = false;                // ��Ÿ�� üũ��
    
    public bool IsCollecting { get { return isCollecting;}}

    private bool isCollecting = false;              // ���� ä�� ������ �ƴ��� üũ
    private bool isCollected = false;               // ä�� �Ϸ� ���� üũ

    // ä���� ������, Ǯ �� �޾ƿ��� �Լ�
    public void InitializeCollectionObject(CollectionData data, CollectionPool pool, CollectionSpawner collectionSpawner)
    {
        collectionData = data;      // ä�� ������ ����
        pools = pool;               // ������Ʈ Ǯ ����
        spawner = collectionSpawner;
    }

    // ä�� ���۽� ȣ��Ǵ� �Լ�, ä�� �õ��ϴ� �÷��̾� ��ġ�� ���ڷ� ����
    public void StartCollecting(Transform playerPos)
    {
        isCollecting = true;                // ä������ ���·� ����
        isCollected = false;                // ä�� �Ϸ� �ʱ�ȭ
        currentGage = 0f;                   // ä�� ������ 0���� �ʱ�ȭ
        ui.ShowCollectionSlider();          // ä�� �����̴� UI ǥ��
        Debug.Log("ä���� ���� �Ǿ����ϴ�.");
    }

    // �� ä���� �����̴� �� ���� �õ��ϴ� �Լ� - ���� ������ �����ϰ� ��Ÿ�� ó��
    public void TryAddCollectGage(float value)
    {
        // �� �̹� ä���� �Ϸ� �Ǿ��ų�, ��Ÿ�� ���̰ų�, ä�� ���°� �ƴ϶�� ����
        if (isCollected || isCoolTime || !isCollecting) return;

        isCoolTime = true;                  // ��Ÿ�� ����

        selfColider.enabled = false;        // �ݶ��̴� ��� ���� �ߺ� ��Ʈ ����

        // �� ������ ���� �� ä�� �Ϸ� �Ǵ��� �ڷ�ƾ���� �ϵ��� �ٲ�
        StartCoroutine(TryCollectGageCoroutine(value));
    }

    // �ܺο��� ������ ä�� �������� ä�� �׽�Ʈ�� �� �ִ� �Լ�
    public void AddCollectGauge(float amount)
    {
        currentGage += amount;                                  // ������ ����
        currentGage = Mathf.Clamp(currentGage, 0f, 100f);       // 0 ~ 100 ���� ���ѵΰ�

        ui.SliderValueUpdate(currentGage);                      // �����̴� �� ������Ʈ

        if (currentGage >= 100f)    // �����̴� ������ �� 100�ϰ� ���ų� �Ѿ��
        {
            CollectCompleted();     // ä�� �Ϸ�
        }
    }

    // ä�� �Ϸ����� �� ȣ��Ǵ� �Լ�
    public void CollectCompleted()
    {
        isCollected = true;             // ä�� �Ϸ� ���·� ����
        isCollecting = false;           // ä�� �� ���� �ƴ�
        ui.HideCollectionSlider();      // �����̴� UI ����

        // ���� ���� �� ó�� �ʿ�
        Debug.Log($"{collectionData.collectionName} ä�� �Ϸ�!");
       
        // �� Ǯ�� ���� ������Ʈ ��ȯ
        pools.ReturnObject(collectionData, this.gameObject); 

        // �� Spawner�� �˷���, Ȱ��ȭ�� ä�� ����Ʈ���� ����
        spawner.RemoveFromActiveList(this.gameObject);
    }

    // ������ ���� �� ä�� �Ϸ� �Ǵ��� �ڷ�ƾ����
    IEnumerator TryCollectGageCoroutine(float value)
    {
        AddCollectGauge(value);                         // ���⼭ ������ ���� �� ä�� �Ϸ� ���� üũ

        // �� ä�� �Ϸ��ٸ�, ������Ʈ Ǯ�� ��ȯ�ǰ� ��Ȱ��ȭ�ǹǷ�, �ڷ�ƾ ����
        if (isCollected)
        {
            yield break;
        }

        yield return new WaitForSeconds(coolTime);      // ��Ÿ�� ���
        isCoolTime = false;                             // ��Ÿ�� ����
        selfColider.enabled = true;                     // �ݶ��̴� �ٽ� Ȱ��ȭ
    }    
}


