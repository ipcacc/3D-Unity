using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Cinemachine;
using Cinemachine.Utility;

public class PalyerMove : MonoBehaviour
{
    public float speed = 5f;

    public float JumpPower = 5f;

    public float gravity = -9.81f;

    public CinemachineVirtualCamera virtualCam;

    public float rotationSpeed = 10f;

    public CinemachineSwitcher CS;

    private CinemachinePOV pov;

    private CharacterController controller;

    private Vector3 velocity;

    public bool isGrounded;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        pov = virtualCam.GetCinemachineComponent<CinemachinePOV>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 camForward = virtualCam.transform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = virtualCam.transform.right;
        camRight.y = 0;
        camRight.Normalize();

        Vector3 move = (camForward * z + camRight * x).normalized;
        if(!CS.usingFreeLook) 
            controller.Move(move * speed * Time.deltaTime);

        float cmaeraYaw = pov.m_HorizontalAxis.Value;
        Quaternion targetRot = Quaternion.Euler(0f, cmaeraYaw, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = JumpPower;
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed = 10f;
            virtualCam.m_Lens.FieldOfView = 80f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = 5f;
            virtualCam.m_Lens.FieldOfView = 60f;
        }


    }
}
