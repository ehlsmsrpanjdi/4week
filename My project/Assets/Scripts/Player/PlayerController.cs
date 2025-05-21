using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    private Vector2 curMovementInput;
    public float jumpPower;

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;

    private Vector2 mouseDelta;

    GameObject planetObject;


    [HideInInspector]
    public bool canLook = true;

    private Rigidbody _rigidbody;

    Vector3 averageNormal = Vector3.zero;
    int HitCount = 0;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        //StartCoroutine(PlayerFlip());
    }

    private void FixedUpdate()
    {
        Grounded();
        Gravity();
        Move();
    }

    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }
    }

    public void OnLookInput(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGround == true)
        {
            _rigidbody.AddForce(averageNormal * jumpPower, ForceMode.Impulse);
        }
    }

    public void OnItemUseInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            string keyPressed = context.control.displayName;

            switch (keyPressed)
            {
                case "1":
                    UIManager.Instance.ItemUse(1);
                    Debug.Log("1번탬사용");
                    break;
                case "2":
                    UIManager.Instance.ItemUse(2);
                    Debug.Log("2번탬사용");
                    break;
                case "3":
                    UIManager.Instance.ItemUse(3);
                    Debug.Log("3번탬사용");
                    break;
                case "4":
                    UIManager.Instance.ItemUse(4);
                    Debug.Log("4번탬사용");
                    break;
                case "5":
                    UIManager.Instance.ItemUse(5);
                    Debug.Log("5번탬사용");
                    break;
                case "6":
                    UIManager.Instance.ItemUse(6);
                    Debug.Log("6번탬사용");
                    break;

            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Planet") == true)
        {
            if (planetObject != other.gameObject)
            {
                planetObject = other.gameObject;
                StartCoroutine(FlipPlayer(other.transform));
            }
        }
    }

    IEnumerator FlipPlayer(Transform _transform)
    {
        // 1. 중력 방향 구하기 (플레이어 → 행성 중심)
        Vector3 averageNormal = (_transform.position - transform.position).normalized;

        // 2. 회전 방향 구하기 (현재 up → 중력 반대 방향)
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, -averageNormal) * transform.rotation;

        // 3. 부드럽게 회전 (짧게 예시: 0.5초 동안 회전)
        float elapsed = 0f;
        float duration = 0.5f;

        Quaternion startRotation = transform.rotation;

        while (elapsed < duration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // 4. 최종 회전 정리
        transform.rotation = targetRotation;
    }

    private void Move()
    {
        if (!IsGround) return;
        Vector3 gravityDir = -averageNormal.normalized; // 중력 방향 (바닥에서 올라오는 방향)
        Vector3 forward = Vector3.ProjectOnPlane(transform.forward, gravityDir).normalized;
        Vector3 right = Vector3.Cross(gravityDir, -forward).normalized;

        Vector3 moveDir = forward * curMovementInput.y + right * curMovementInput.x;
        moveDir *= moveSpeed;

        // 중력 방향 속도는 유지 (예: 낙하속도)
        Vector3 vel = moveDir + Vector3.Project(_rigidbody.velocity, gravityDir);

        _rigidbody.velocity = vel;
    }

    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);

        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);
        //카메라 회전각은 똑같은데

        // 플레이어가 바닥 normal기준이 위니까 회전, 축 기준으로 rotation구한거임;  쿼터니엄은 각도, 축임
        Quaternion rotation = Quaternion.AngleAxis(mouseDelta.x * lookSensitivity, transform.up);
        transform.rotation = rotation * transform.rotation;    // 현재위치 * 변환행렬

    }
    bool IsGround = false;
    void Grounded()
    {
        Ray[] bottomrays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), -transform.up),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), -transform.up),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), -transform.up),
            new Ray(transform.position + (-transform.right * 0.2f) +(transform.up * 0.01f), -transform.up)
        };

        HitCount = 0;

        for (int i = 0; i < bottomrays.Length; i++)
        {
            if (Physics.Raycast(bottomrays[i], out RaycastHit hit, 5f))
            {
                averageNormal += hit.normal;
                ++HitCount;
            }
        }
        if (HitCount > 0)
        {
            averageNormal.Normalize();
            AlignToGravity(averageNormal);
            IsGround = true;
            return;
        }
        IsGround = false;
    }

    float rotateSpeed = 5;

    void AlignToGravity(Vector3 groundNormal)
    {
        // 원하는 방향: 위쪽을 groundNormal에 맞추기
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, groundNormal) * transform.rotation;

        // 부드럽게 회전 (slerp 사용)
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);
    }

    void Gravity()
    {
        if (IsGround == true)
        {
            _rigidbody.AddForce(-averageNormal * 30f, ForceMode.Acceleration);
        }
    }

    private void Update()
    {
    }

    public void ToggleCursor(bool toggle)
    {
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }
}