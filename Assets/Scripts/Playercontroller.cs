using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
public class PlayerController : MonoBehaviour
{
    [Header("�̵� ����")]
    public float moveSpeed = 5f;
    private Rigidbody rb;
    private Vector3 moveDirection;

    [Header("ü�� ����")]
    public float maxHealth = 20f;          // �ִ� ü�� (��Ʈ 3�� = 6HP) ���氡��
    public float currentHealth = 20f;      // ���� ü�� (0.5 ���� ����)

    [Header("��Ʈ UI ����")]
    public GameObject heartPrefab;        // ��Ʈ �ϳ��� �̹��� ������
    public Sprite fullHeart, halfHeart, emptyHeart;
    public Transform heartContainer;      // ��Ʈ���� ���� �θ� ������Ʈ (HeartPanel)
    private List<Image> hearts = new List<Image>();

    [Header("Ÿ�� txt, ���ھ� txt")]
    public TextMeshProUGUI timeText; 
    private float timer = 0f;
    public TextMeshProUGUI scoreText;
    private int score = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        CreateHearts(); //ü�¿� ���� ��Ʈ UI����
        UpdateHearts(); //���� ü�¿� ���� ��Ʈ UI����
    }

    void Update()
    {
        // �̵� �Է� ó��
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector3(moveX, 0f, moveZ).normalized;

        timer += Time.deltaTime; //�ð� �ؽ�Ʈ

        int minutes = Mathf.FloorToInt(timer / 60f); //��
        int seconds = Mathf.FloorToInt(timer % 60f); //��

        timeText.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);
    }


    //�÷��̾� ������
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
    }

    //���ھ� �ؽ�Ʈ
    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = "Score\n"+ score.ToString();
    }

    // ���� �浹 �� ������ 0.5 ��Ʈ (1HP) ���ھ� 200
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            TakeDamage(1f); // ��ĭ ������
            AddScore(200); // ���� +200 
            Destroy(other.gameObject); // �� ����
        }
    }


    //������ ����
    public void TakeDamage(float amount)
    {
        currentHealth = Mathf.Max(currentHealth - amount, 0f);
        Debug.Log("���� ü��: " + currentHealth);
        UpdateHearts();
    }

    // ü��ȸ��
    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        UpdateHearts();
    }


    //��Ʈ prefab�ҷ�����
    void CreateHearts()
    {
        int heartCount = Mathf.CeilToInt(maxHealth / 2f);
        for (int i = 0; i < heartCount; i++)
        {
            GameObject heart = Instantiate(heartPrefab, heartContainer);
            Image img = heart.GetComponent<Image>();
            hearts.Add(img);
        }
    }


    // �� ĭ ��Ʈ, �� ĭ ��Ʈ, �� ��Ʈ ����
    void UpdateHearts()
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            float heartHealth = Mathf.Clamp(currentHealth - (i * 2f), 0f, 2f);

            if (heartHealth >= 2f)
                hearts[i].sprite = fullHeart;  //�� ĭ= ��ĭ 2��
            else if (heartHealth >= 1f)
                hearts[i].sprite = halfHeart; //�� ĭ
            else
                hearts[i].sprite = emptyHeart;
        }
    }
}
