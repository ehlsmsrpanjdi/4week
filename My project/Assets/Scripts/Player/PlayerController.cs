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
    public LayerMask groundLayerMask;

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;

    private Vector2 mouseDelta;

    bool isJumping = false;

    Coroutine flipCoroutine;

    Coroutine gravityCoroutine;

    [HideInInspector]
    public bool canLook = true;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        StartCoroutine(PlayerFlip());
    }

    private void FixedUpdate()
    {
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
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            _rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
            isJumping = true;
            if (flipCoroutine == null)
            {
                flipCoroutine = StartCoroutine(FlipFunction());
            }
            else
            {
                StopCoroutine(flipCoroutine);
                flipCoroutine = StartCoroutine(FlipFunction());
            }
        }
    }

    IEnumerator FlipFunction()
    {
        yield return new WaitForSeconds(1f);

        isJumping = false;

        float FlipTime = 0f;

        while (FlipTime < 3)
        {
            FlipTime += Time.deltaTime;
            if (true == IsFlip())
            {
                StopCoroutine(flipCoroutine);
                //StartCoroutine()
            }
            yield return null;
        }
        yield return null;
    }

    IEnumerator PlayerFlip()
    {
        Quaternion playerrotation = gameObject.transform.rotation;
        Quaternion fliprotation = playerrotation * Quaternion.Euler(180, 0, 0);

        float flipTime = 0f;

        while (flipTime < 3f)
        {
            flipTime += Time.deltaTime;
            gameObject.transform.rotation = Quaternion.Lerp(playerrotation, fliprotation, flipTime / 3f);
            yield return null;
        }
        gameObject.transform.rotation = fliprotation;
        yield return null;
    }

    private void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;
        dir.y = _rigidbody.velocity.y;

        _rigidbody.velocity = dir;
    }

    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    bool IsGrounded()
    {
        Ray[] bottomrays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) +(transform.up * 0.01f), Vector3.down)
        };


        for (int i = 0; i < bottomrays.Length; i++)
        {
            if (Physics.Raycast(bottomrays[i], 1f, groundLayerMask))
            {
                return true;
            }
        }

        return false;
    }

    bool IsFlip()
    {
        Ray[] toprays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.1f), Vector3.up),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.1f), Vector3.up),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.1f), Vector3.up),
            new Ray(transform.position + (-transform.right * 0.2f) +(transform.up * 0.1f), Vector3.up)
        };

        for (int i = 0; i < toprays.Length; i++)
        {
            if (Physics.Raycast(toprays[i], 5f, groundLayerMask))
            {
                return true;
            }
        }
        return false;
    }
    Ray[] boray = new Ray[4];
    Ray[] toray = new Ray[4];


    private void Update()
    {
        boray[0] = new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down);
        boray[1] = new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down);
        boray[2] = new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down);
        boray[3] = new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down);

        toray[0] = new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.1f), Vector3.up);
        toray[1] = new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.1f), Vector3.up);
        toray[2] = new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.1f), Vector3.up);
        toray[3] = new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.1f), Vector3.up);


        foreach (Ray ray in boray)
        {
            Debug.DrawRay(ray.origin, ray.direction * 1f, Color.red);
        }

        foreach (Ray ray in toray)
        {
            Debug.DrawRay(ray.origin, ray.direction * 5f, Color.blue);
        }
    }

    public void ToggleCursor(bool toggle)
    {
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }
}