using System;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]

public class PlayerControllerClase : MonoBehaviour
{

    [SerializeField] private Rigidbody m_rigidbody;
    [SerializeField] private float moveSpeed = 5.0f;
     [SerializeField] private float RotationSpeed = 1.5f;

     [SerializeField] private Transform cameraTransform;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    public void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        transform.Rotate(mouseX*RotationSpeed * transform.up);

        cameraTransform.Rotate(-1 * mouseY * RotationSpeed * Vector3.up);

        float rotationCamera = cameraTransform.localRotation.eulerAngles.x + (-1 * mouseY * RotationSpeed);

        float maxRotation = 90.0f;
        rotationCamera = Mathf.Clamp(rotationCamera, -maxRotation, maxRotation);

        cameraTransform.localRotation = Quaternion.Euler(rotationCamera, 0, 0);
    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 moveDir = Vector3.right * h + Vector3.forward * v;

        Vector3 velocity = moveDir * moveSpeed;

        velocity.y = m_rigidbody.linearVelocity.y;

        m_rigidbody.linearVelocity = velocity;
    }
}