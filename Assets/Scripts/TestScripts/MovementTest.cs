//using UnityEditor.Experimental.GraphView;
//using UnityEngine;

//public class MovementTest : MonoBehaviour
//{
//    // Based Character controll
//    // Mix with FIxed Update + update callbacks 
//    [Header("Player Property")]
//    [SerializeField] CharacterController playerCC;
//    // ������ ��� ������ ���� �÷��̾� ĳ���� ���� ��ȯ �ڵ� �߰� �ʿ�

//    [Header("Movement")]
//    [SerializeField] float playerSpeed;
//    [SerializeField] float gravityAc;

//    [Header("Ground Checker Property")]
//    [SerializeField] Vector3 boxCastSize;
//    [SerializeField] float maxDIs;
//    [SerializeField] LayerMask landLayer;

//    [Header("Debugger")]
//    [SerializeField] bool drawedGizmo;

//    float getHorizontal;
//    float getVertical;

//    Vector3 velocity;
//    Vector3 lastFixedPos;
//    Quaternion lastFixedRotation;
//    Vector3 nextFixedPos;
//    Quaternion nextFixedRotation;


//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    void Start()
//    {
//        velocity = Vector3.zero;

//        lastFixedPos = transform.position;
//        nextFixedPos = transform.position;

//        gravityAc = 3f;
//        playerSpeed = 7f;
//        playerCC = GetComponent<CharacterController>();    
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        getVertical = Input.GetAxis("Vertical");
//        getHorizontal = Input.GetAxis("Horizontal");

//        float timeLerp = (Time.time - Time.fixedTime) / Time.fixedDeltaTime;
//        playerCC.Move(Vector3.Lerp(lastFixedPos, nextFixedPos, timeLerp) - this.transform.position);
//    }

//    private void FixedUpdate()
//    {
//        lastFixedPos = nextFixedPos;

//        Vector3 planeVelocity = GetVerticalVelocity(getHorizontal, getVertical);
//        float yVelo = GetHorizontalVelotcity();
//        velocity = new Vector3(planeVelocity.x, yVelo, planeVelocity.z);

//        // �̵���Ű�� ����, �� �ܰ� ��ġ��
//        Vector3 nextPosCanMove = nextFixedPos + velocity * Time.fixedDeltaTime;
//        //�浹 �÷��� Ȯ��
//        CollisionFlags checker = playerCC.Move(nextPosCanMove - nextFixedPos);

//        // �浹 Ȯ��
//        if ((checker & CollisionFlags.CollidedSides) == 0)
//        {
//            //��ü�� ����� �� ���� ������ ����� ������ ��
//            nextFixedPos = nextPosCanMove;

//        }

//        // ���� ���� �߻���
//        //nextFixedPos += velocity * Time.fixedDeltaTime;
//    }

//    private void OnDrawGizmos()
//    {
//        if (!drawedGizmo) return;

//        Gizmos.color = Color.red;
//        Gizmos.DrawCube(transform.position - transform.up * maxDIs, boxCastSize);

//    }
//    bool goundChecker()
//    {
//        return Physics.BoxCast(transform.position, boxCastSize, -transform.up, transform.rotation, maxDIs, landLayer);
//    }

//    Vector3 GetVerticalVelocity(float horizontal, float vertical)
//    {
//        Vector3 moveVelo = this.transform.forward * getVertical + this.transform.right * getHorizontal;
//        Vector3 moveDir = moveVelo.normalized;

//        float moveSpd = Mathf.Min(moveVelo.magnitude, 1f) * playerSpeed;

//        return moveDir * moveSpd;
//    }
//    float GetHorizontalVelotcity()
//    {
//        if (!goundChecker())
//        {
//            return velocity.y - gravityAc * Time.fixedDeltaTime;
//        }
//        return Mathf.Max(.0f, velocity.y);
//    }
//}


using UnityEngine;

public class MovementTest : MonoBehaviour
{
    [Header("Player Property")]
    [SerializeField] CharacterController playerCC;

    [Header("Movement")]
    [SerializeField] float playerSpeed = 7f;
    [SerializeField] float gravity = 9.8f;

    [Header("Ground Checker Property")]
    [SerializeField] LayerMask landLayer;

    [Header("Debugger")]
    [SerializeField] bool drawedGizmo;

    float getHorizontal;
    float getVertical;
    Vector3 velocity;

    void Start()
    {
        playerCC = GetComponent<CharacterController>();
        velocity = Vector3.zero;
    }

    void Update()
    {
        getVertical = Input.GetAxis("Vertical");
        getHorizontal = Input.GetAxis("Horizontal");
    }

    void FixedUpdate()
    {
        Vector3 moveDirection = (transform.forward * getVertical + transform.right * getHorizontal).normalized;
        Vector3 moveVelocity = moveDirection * playerSpeed;

        if (playerCC.isGrounded)
        {
            velocity.y = -1f;  
        }
        else
        {
            velocity.y -= gravity * Time.fixedDeltaTime;
        }
        Vector3 finalVelocity = new Vector3(moveVelocity.x, velocity.y, moveVelocity.z);
        playerCC.Move(finalVelocity * Time.fixedDeltaTime);
    }
}
