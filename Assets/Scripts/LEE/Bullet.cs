using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    //����Ƽ ���� ������Ʈ Ǯ�� ���
    IObjectPool<Bullet> _pool;

    public void SetPool(IObjectPool<Bullet> pool)
    {
        _pool = pool;
    }

    void OnCollisionEnter(Collision collision)
    {
        string tag = collision.gameObject.tag;

        //�÷��̾� or ������ �ϰ��
        if (tag == "Player" || tag == "Structures")
        {
            //TODO: �÷��̾� �� ��� ä���� ���� -- ����

            // �浹 �� �ڵ� �ݳ�
            _pool?.Release(this);
        }
    }

    public void OnSpawn()
    {
        //TODO: ������ �� �ʱ�ȭ ó�� ��
    }

    public void BulletMove()
    {
        //TODO: ź���� �̵�
    }
}
