using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static SwitchCreateMode;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private SwitchCreateMode _createMode;
    [SerializeField] private CinemachineVirtualCamera _camera;
    [SerializeField] private GameObject _cameraFollow;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float dragSpeed = .5f;
    [SerializeField] float FOV_MAX = 30f;
    [SerializeField] float FOV_MIN = 10f;
    private bool dragActive;
    private Vector2 lastMousePos;
    private float targetFOV = 30f;
    private bool _canZoom;


    private void Update()
    {
        if (_createMode.currentMode.Equals(CreateMode.MapMode))
        {
            HandleCameraMovement();
            HandleCameraRotation();
            HandleCameraZoom();
        }
    }

    private void HandleCameraMovement()
    {
        //KeyBoardMovement();
        DragPanMovement();
    }

    private void HandleCameraRotation()
    {
        /*float rotateDir = 0f;
        if (Input.GetKey(KeyCode.Q)) rotateDir = +1f;
        if (Input.GetKey(KeyCode.E)) rotateDir = -1f;

        _cameraFollow.transform.eulerAngles += new Vector3(0, rotateDir * rotateSpeed * Time.deltaTime, 0);*/
    }

    private void KeyBoardMovement()
    {
        Vector3 inputDir = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W)) inputDir.z = +1f;
        if (Input.GetKey(KeyCode.S)) inputDir.z = -1f;
        if (Input.GetKey(KeyCode.A)) inputDir.x = -1f;
        if (Input.GetKey(KeyCode.D)) inputDir.x = +1f;
        Vector3 moveDir = transform.up * inputDir.z + transform.right * inputDir.x;
        _cameraFollow.transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    private void DragPanMovement()
    {
        Vector3 inputDir = new Vector3(0, 0, 0);
        if (Input.GetMouseButtonDown(1))
        {
            dragActive = true;
            lastMousePos = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(1))
        {
            dragActive = false;
        }
        if (dragActive)
        {
            Vector2 mouseMovementDelta = (Vector2)Input.mousePosition - lastMousePos;
            lastMousePos = Input.mousePosition;
            inputDir.x = -mouseMovementDelta.x * dragSpeed;
            inputDir.z = -mouseMovementDelta.y * dragSpeed;
        }

        Vector3 moveDir = transform.up * inputDir.z + transform.right * inputDir.x;
        _cameraFollow.transform.position += moveDir * dragSpeed * Time.deltaTime;
    }

    private void HandleCameraZoom()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            targetFOV -= 5;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            targetFOV += 5;
        }
        targetFOV = Mathf.Clamp(targetFOV, FOV_MIN, FOV_MAX);
        _camera.m_Lens.FieldOfView = Mathf.Lerp(_camera.m_Lens.FieldOfView, targetFOV, Time.deltaTime * 10);
    }
}
