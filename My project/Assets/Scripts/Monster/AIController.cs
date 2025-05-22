using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class AIController : MonoBehaviour
{
    Vector3 averageGravity;
    float gravityForce = 30f;

    float moveSpeed = 5f;

    bool isGround = false;

    float HitCount = 0;

    Rigidbody _rigidbody;
    CapsuleCollider _capsuleCollider;

    GameObject Target;

    private void Awake()
    {
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Target = CharacterManager.Instance.Player.gameObject;
    }

    float stunTime = 0f;

    private void Update()
    {
        if(stunTime > 0)
        {
            stunTime -= Time.deltaTime;
        }
    }

    public void Stun()
    {
        stunTime = 3f;
        _rigidbody.velocity = Vector3.zero;
    }

    private void FixedUpdate()
    {
        if (stunTime > 0) return;
        CalculateGravity();
        Gravity();
        Move();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile") == true)
        {
            Destroy(other.gameObject);
            Stun();
        }
    }


    void CalculateGravity()
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
                averageGravity += hit.normal;
                ++HitCount;
            }
        }
        if (HitCount > 0)
        {
            averageGravity.Normalize();
            AlignToGravity(averageGravity);
            isGround = true;
            return;
        }
        isGround = false;
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
        _rigidbody.AddForce(-averageGravity * gravityForce, ForceMode.Acceleration);
    }

    void Move()
    {
        float length = Vector3.Distance(Target.transform.position, transform.position);
        if (length > 20)
        {
            _rigidbody.velocity = Vector3.ProjectOnPlane(transform.forward, averageGravity).normalized * moveSpeed;
        }
        else
        {
            Vector3 toPlayer = (Target.transform.position - transform.position).normalized;
            Vector3 right = Vector3.Cross(averageGravity, toPlayer); // 수평 방향
            Vector3 forwardOnSurface = Vector3.Cross(right, averageGravity).normalized;

            _rigidbody.velocity = forwardOnSurface * moveSpeed;
        }


        
    }
}
