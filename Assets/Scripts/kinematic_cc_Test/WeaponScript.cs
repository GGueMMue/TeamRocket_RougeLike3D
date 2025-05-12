using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    [Header("Weapon's Basic Value | If the weapon is meele, set roundsPerMinute as animation delay")]
    public bool isMeele = true;
    public bool isShotgun = false;
    public int weaponDamage = 0;
    public float weaponReroadTime = 0;
    public int nowBullet = 12;
    public int maxBullet = 12;
    public bool nowReroading = false;
    public float roundsPerMinute;


    [Header("Weapon's Spread Value | use only for Gun")]
    public float maxFireDistance = 100f;
    public float spreadAmount = 0.1f;         // ���� ȭ�� ���� ������
    public float maxSpread = 0.3f;            // �ִ� ���� ������
    public float spreadPerShot = 0.02f;       // �ߴ� ���� ������ ������ġ
    public float spreadRecoverySpeed = 0.05f; // ���� ȸ�� �ӵ�
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
