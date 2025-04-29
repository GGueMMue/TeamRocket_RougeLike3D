using UnityEngine;
using System.Collections;

public class BossControl : MonoBehaviour
{
    public Transform player;
    MeshRenderer[] meshs;

    public Transform LHand;
    public Transform RHand;

    public GameObject straightBulletPrefab;
    public GameObject homingBulletPrefab;

    public float dashSpeed = 20f;
    public float patternCooldown = 2f;

    private bool isAttacking = false;

    void Start()
    {
        meshs = GetComponentsInChildren<MeshRenderer>();
        StartCoroutine(PatternLoop());
    }

    IEnumerator PatternLoop()
    {
        while (true)
        {
            if (!isAttacking)
            {
                isAttacking = true;

                int pattern = Random.Range(0, 3);
                switch (pattern)
                {
                    case 0:
                        yield return StartCoroutine(DashPattern());
                        break;
                    case 1:
                        yield return StartCoroutine(StraightShotPattern());
                        break;
                    case 2:
                        yield return StartCoroutine(HomingShotPattern());
                        break;
                }

                yield return new WaitForSeconds(patternCooldown);
                isAttacking = false;
            }

            yield return null;
        }
    }

    // ���� 1: �÷��̾ ���� ����
    IEnumerator DashPattern()
    {
        // �÷��̾� ��ġ �ν�
        Vector3 targetPosition = player.position;
        targetPosition.y = transform.position.y;
        // �÷��̾ �ٶ󺸰�
        Vector3 targetDirection = new Vector3(targetPosition.x - transform.position.x, 0, targetPosition.z - transform.position.z).normalized;
        transform.rotation = Quaternion.LookRotation(targetDirection);

        foreach (MeshRenderer mesh in meshs)
            mesh.material.color = Color.green;
        yield return new WaitForSeconds(2f);
        foreach (MeshRenderer mesh in meshs)
            mesh.material.color = Color.white;

        transform.forward = (targetPosition - transform.position).normalized;

        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, dashSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(1f);
    }

    // ���� 2: �� �տ��� 5�߾� ������ źȯ �߻�
    IEnumerator StraightShotPattern()
    {
        for (int i = 0; i < 5; i++)
        {
            // �����ư��� ���� ����
            Transform fireHand = (i % 2 == 0) ? RHand : LHand;

            FireStraightBullet(fireHand);
            yield return new WaitForSeconds(0.5f); // ���� ���� ����
        }

        yield return new WaitForSeconds(1f); // ���� ���� �� ���
    }

    // ���� 3: �� �տ��� 1�߾� ���� źȯ �߻�
    IEnumerator HomingShotPattern()
    {
        FireHomingBullet(RHand);
        yield return new WaitForSeconds(1f);

        FireHomingBullet(LHand);
        yield return new WaitForSeconds(1f); // ���� ���ϰ��� ���� ����
    }

    void FireStraightBullet(Transform hand)
    {
        GameObject bullet = Instantiate(straightBulletPrefab, hand.position, Quaternion.identity);
        NBullet sb = bullet.GetComponent<NBullet>();
        if (sb != null)
        {
            sb.target = player;
        }
    }

    void FireHomingBullet(Transform hand)
    {
        GameObject bullet = Instantiate(homingBulletPrefab, hand.position, Quaternion.identity);
        Homing homing = bullet.GetComponent<Homing>();
        if (homing != null)
        {
            homing.target = player;
        }
    }
}
