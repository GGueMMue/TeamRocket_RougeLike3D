using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerController : MonoBehaviour
{
    [SerializeField]    PlayerCamera _playerCam;
    [SerializeField]    Transform _cameraFollowPoint;
    [SerializeField]    PlayerMovementController _characterController;
    [SerializeField]    Transform _WeaponPrefab;
    [SerializeField]    WeaponScript _weaponScript;
    [SerializeField]    float _fireTimer = 0;
    [SerializeField]    GameObject bulletTrailPrefab;
    [SerializeField]    float _reRoadTimer = 0;
    RaycastHit hit;

    Vector3 _lookInputVector;

    //private void Awake()
    //{
    //    _WeaponPrefab = FindCHildWithTag(GameObject.Find("Character").transform, "Weapon");
    //}
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _playerCam.SetFollowTransform(_cameraFollowPoint);
        _WeaponPrefab = FindChildWithTag(_characterController.gameObject.transform, "Weapon");
        _weaponScript = _WeaponPrefab.GetComponent<WeaponScript>();

    }

    Transform FindChildWithTag(Transform character, string tag)
    {
        foreach(Transform childs in character.transform.GetComponentInChildren<Transform>())
        {
            if(childs.CompareTag(tag))
            {
                return childs;
            }
        }

        return null;
    }

    void HandledCameraInput()
    {
        float mouseUp = Input.GetAxisRaw("Mouse Y");
        float mouseRIght = Input.GetAxisRaw("Mouse X");

        _lookInputVector = new Vector3(mouseRIght, mouseUp, 0f);

        // if(Physics.Raycast(_playerCam.gameObject.transform.position, -_playerCam.gameObject.transform.forward, out _playerCam.hit, _playerCam.raycastDis))
        // {
        //     _playerCam.UpdatePlayerStickOnWall(Time.deltaTime, _lookInputVector);
        // }
        // Doesnt works

        float scrollInput = -Input.GetAxis("Mouse ScrollWheel");
        _playerCam.UpdateWithInput(Time.deltaTime, scrollInput, _lookInputVector);
    }
    void HandleCharacterInputs()
    {
        _fireTimer += Time.deltaTime;

        PlayerInput inputs = new PlayerInput();
        inputs.AxisFwd = Input.GetAxisRaw("Vertical");
        inputs.AxisRight = Input.GetAxisRaw("Horizontal");
        inputs.CameraRotation = _playerCam.transform.rotation;
        inputs.CrouchDown = Input.GetKeyDown(KeyCode.LeftControl);
        inputs.CrouchUp = Input.GetKeyUp(KeyCode.LeftControl);
        inputs.Sprint = Input.GetKey(KeyCode.LeftShift);
        inputs.Non_Sprint = !inputs.Sprint;
        //inputs.Reroading = Input.GetKeyDown(KeyCode.R);

        if (Input.GetKeyDown(KeyCode.R) && !inputs.Reroading)
        {
            inputs.Reroading = true;
            _characterController.playerState = PlayerState.REROADING;
        }

        if (inputs.Reroading)
        {
            _reRoadTimer += Time.deltaTime;

            if (_reRoadTimer >= _weaponScript.weaponReroadTime)
            {
                inputs.Reroading = false;
                _weaponScript.nowBullet = _weaponScript.maxBullet;
                _characterController.playerState = PlayerState.IDLE;
                _reRoadTimer = 0;
            }
        }
        if (inputs.Sprint) Debug.Log("�޸��� ��");
        if (inputs.Non_Sprint) Debug.Log("�޸��� �ƴ�");
        if (Input.GetKey(KeyCode.Mouse0))
        {
            FireGun(inputs);
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            _characterController.playerState = PlayerState.IDLE;
        }

        //if (Input.GetKey(KeyCode.Mouse0) && !_weaponScript.isMeele
        //    && !_weaponScript.nowReroading && _weaponScript.nowBullet > 0)
        //{
        // �׽�Ʈ ��ũ��Ʈ. ���� �۵���
        //if(_fireTimer >= _WeaponPrefab.GetComponent<WeaponScript>().roundsPerMinute)
        //{
        //    _fireTimer = 0;
        //    inputs.MeleeAttack = true;
        //    _WeaponPrefab.GetComponent<WeaponScript>().nowBullet--;

        //    Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);

        //    Ray ray = _playerCam.gameObject.GetComponent<Camera>().ScreenPointToRay(screenCenter);
        //    if (Physics.Raycast(ray, out hit, 100f))
        //    {
        //        Debug.Log($"Hit {hit.collider.name} ��� �Ϸ�");
        //    }
        //}
        //}
        //if (Input.GetKey(KeyCode.Mouse0) && _WeaponPrefab.GetComponent<WeaponScript>().isMeele)
        //{
        //    if (_fireTimer >= _WeaponPrefab.GetComponent<WeaponScript>().roundsPerMinute)
        //    {
        //        _fireTimer = 0;
        //        inputs.ShootingAttack = true;
        //    }
        //}
        //inputs.Sprint = Input.GetKeyDown(KeyCode.LeftShift);
        //inputs.Non_Sprint = Input.GetKeyUp(KeyCode.LeftShift);
        inputs.Dodge = Input.GetKeyDown(KeyCode.Space);
        

        _characterController.SetInputs(ref inputs);
    }

    void FireGun(PlayerInput inputs)
    {
        if (_weaponScript.nowReroading || _weaponScript.nowBullet <= 0 || _weaponScript.isMeele) return;
        if(_fireTimer < _weaponScript.roundsPerMinute) return;

        _characterController.playerState = PlayerState.SHOOTINGATTACK;
        _fireTimer = 0;
        _weaponScript.nowBullet--;

        // ���� �� ���� ���� ���� (��������)
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        //Vector3 spreadDir = GetSpreadDirection();

        Ray ray = _playerCam.GetComponent<Camera>().ScreenPointToRay(screenCenter);
        //ray.direction += spreadDir;

        _playerCam.ApplyRecoilShake(Random.Range(-_weaponScript.horizontalAmount, _weaponScript.horizontalAmount), _weaponScript.verticalAmount);

        if (Physics.Raycast(ray, out RaycastHit hit, _weaponScript.maxFireDistance))
        {
            Debug.Log($"Hit {hit.collider.name} ||||||||| {hit.point}");
            // ���⿡ ������ ó�� �ʿ���. �ϴ� �׽�Ʈ
            ShowBulletTestTrail(ray.origin, hit.point);
        }
    }
    void ShowBulletTestTrail(Vector3 start, Vector3 end)
    {
        GameObject trail = Instantiate(bulletTrailPrefab);
        LineRenderer lr = trail.GetComponent<LineRenderer>();
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);

        Destroy(trail, 0.1f); // ��� �����ְ� ����
    }
    /*
    Vector3 GetSpreadDirection()
    {
        // ī�޶� ���� ���� ���� (�¿� ���� ����)
        float spreadX = Random.Range(-_weaponScript.spreadAmount, _weaponScript.spreadAmount);
        float spreadY = Random.Range(-_weaponScript.spreadAmount, _weaponScript.spreadAmount);

        Vector3 right = _playerCam.transform.right;
        Vector3 up = _playerCam.transform.up;

        return (right * spreadX + up * spreadY);
    }*/
    private void Update()
    {
        HandleCharacterInputs();
    }

    private void LateUpdate()
    {
        HandledCameraInput();
    }
}
    