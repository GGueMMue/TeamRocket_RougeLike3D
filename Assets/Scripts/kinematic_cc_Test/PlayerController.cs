using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerController : MonoBehaviour
{
    [SerializeField]    PlayerCamera _playerCam;
    [SerializeField]    Transform _cameraFollowPoint;
    [SerializeField]    PlayerMovementController _characterController;
    [SerializeField]    Transform _WeaponPrefab;
    [SerializeField]    float _fireTimer = 0;

    Vector3 _lookInputVector;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _playerCam.SetFollowTransform(_cameraFollowPoint);
        _WeaponPrefab = FindCHildWithTag(GameObject.Find("Character").transform, "Weapon");

    }

    Transform FindCHildWithTag(Transform character, string tag)
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
        if (inputs.Sprint) Debug.Log("�޸��� ��");
        if (inputs.Non_Sprint) Debug.Log("�޸��� �ƴ�");

        if (Input.GetKey(KeyCode.Mouse0) && _WeaponPrefab.GetComponent<WeaponScript>().isMeele)
        {
            if(_fireTimer >= _WeaponPrefab.GetComponent<WeaponScript>().roundsPerMinute)
            {
                _fireTimer = 0;
                inputs.MeleeAttack = true;
            }
        }
        if (Input.GetKey(KeyCode.Mouse0) && !_WeaponPrefab.GetComponent<WeaponScript>().isMeele)
        {
            if (_fireTimer >= _WeaponPrefab.GetComponent<WeaponScript>().roundsPerMinute)
            {
                _fireTimer = 0;
                inputs.ShootingAttack = true;
            }
        }
        //inputs.Sprint = Input.GetKeyDown(KeyCode.LeftShift);
        //inputs.Non_Sprint = Input.GetKeyUp(KeyCode.LeftShift);
        inputs.Dodge = Input.GetKeyDown(KeyCode.Space);
        

        _characterController.SetInputs(ref inputs);
    }

    private void Update()
    {
        HandleCharacterInputs();
    }

    private void LateUpdate()
    {
        HandledCameraInput();
    }
}
    