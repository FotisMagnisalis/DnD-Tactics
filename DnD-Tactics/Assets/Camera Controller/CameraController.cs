using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CameraController : MonoBehaviour
{
    //--//--// > BEFORE USING THE SCRIPT PLEASE READ THE MESSAGE BELOW < \\--\\--\\

    // Everything that do something is commented on the code, you can change anything as you like
    // Ex: You dont like the scroll up and down with mouse scroll, then just go to line 106 and comment that code
    // I am commenting it because i think that some of people use Unity for fun and dont know how to code

    // Pré-requirements -> A camera in the scene
    // Tutorial on How to Use ->

    // STEP 1 - Put this script in any gameobject on your scene
    // Nice job, you are done

    // If you are trying to do an Online RTS you can use something like "if(!LocalPlayer) Destroy(gameObject)"
    // Hope you like

    // Editor Properties
    public Camera TheCamera;
    public LayerMask GroundLayer;

    // Camera Properties
    public bool useDefaultSettings;
    [Range(8, 32)]
    public float cameraSpeed;
    [Range(.8f, 3.2f)]
    public float cameraBorder;

    // Minimun and maxium distance from the detected ground the Camera can be
    [Range(.8f, 32f)]
    public float cameraMinHeight;
    [Range(.8f, 32f)]
    public float cameraMaxHeight;

    // Map properties
    public float mapSize;

    // Properties that shold not change to make sure the camera will work
    private float _savedCameraSpeed;

    private RaycastHit _rayHit;
    private Vector2 _leftMouseInitial;
    private Vector2 _leftMouseFinal;
    private Vector3 _middleMouseInitial;

    void Start()
    {
        CheckSettings();
    }

    void Update()
    {
        Controller();
    }

    void CheckSettings()
    {
        if (useDefaultSettings)
        {
            TheCamera = Camera.main;
            cameraSpeed = 20;
            cameraBorder = 2f;
            cameraMinHeight = 2f;
            cameraMaxHeight = 20f;
            mapSize = float.MaxValue;
        }

        _savedCameraSpeed = cameraSpeed;
    }

    void Controller()
    {
        Vector3 position = TheCamera.transform.position;
        Vector3 rotation = new Vector3(60, TheCamera.transform.eulerAngles.y, 0);

        // W, A, S, D Movement
        if (Input.GetKey(KeyCode.W))
            position += new Vector3(TheCamera.transform.forward.x, 0, TheCamera.transform.forward.z) * (cameraSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.S))
            position -= new Vector3(TheCamera.transform.forward.x, 0, TheCamera.transform.forward.z) * (cameraSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.A))
            position -= new Vector3(TheCamera.transform.right.x, 0, TheCamera.transform.right.z) * (cameraSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.D))
            position += new Vector3(TheCamera.transform.right.x, 0, TheCamera.transform.right.z) * (cameraSpeed * Time.deltaTime);

        // Q, E, Alt Rotation
        if (Input.GetKey(KeyCode.Q))
            rotation.y -= cameraSpeed * (Time.deltaTime * 32);
        if (Input.GetKey(KeyCode.E))
            rotation.y += cameraSpeed * (Time.deltaTime * 32);
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            rotation.y = 0;
            TheCamera.transform.eulerAngles = new Vector3(TheCamera.transform.eulerAngles.x, 0, 0);
        }

        // Border Touch Movement
        if (Input.mousePosition.y >= Screen.height - cameraBorder)
            position += new Vector3(TheCamera.transform.forward.x, 0, TheCamera.transform.forward.z) * (cameraSpeed * Time.deltaTime);
        if (Input.mousePosition.y <= 0 + cameraBorder)
            position -= new Vector3(TheCamera.transform.forward.x, 0, TheCamera.transform.forward.z) * (cameraSpeed * Time.deltaTime);
        if (Input.mousePosition.x >= Screen.width - cameraBorder)
            position += new Vector3(TheCamera.transform.right.x, 0, TheCamera.transform.right.z) * (cameraSpeed * Time.deltaTime);
        if (Input.mousePosition.x <= 0 + cameraBorder)
            position -= new Vector3(TheCamera.transform.right.x, 0, TheCamera.transform.right.z) * (cameraSpeed * Time.deltaTime);

        // Mouse Rotation
        if (Input.GetKeyDown(KeyCode.Mouse2))
            _middleMouseInitial = Input.mousePosition;
        if (Input.GetKey(KeyCode.Mouse2))
        {
            if (_middleMouseInitial.x - Input.mousePosition.x > 100 || _middleMouseInitial.x - Input.mousePosition.x < -100)
                rotation.y -= (_middleMouseInitial.x - Input.mousePosition.x) / 60;
            if (_middleMouseInitial.y - Input.mousePosition.y > 100 || _middleMouseInitial.y - Input.mousePosition.y < -100)
                position += transform.up * -(_middleMouseInitial.y - Input.mousePosition.y) / 480;
        }

        // Mouse Scroll Zoom
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        position.y -= scroll * 20;

        // Shift Acelleration
        if (Input.GetKey(KeyCode.LeftShift))
            cameraSpeed = (_savedCameraSpeed * 2f);
        else
            cameraSpeed = _savedCameraSpeed;

        // Camera Collision Check
        Ray ray = new Ray(position, TheCamera.transform.forward);
        Physics.Raycast(ray, out _rayHit, 32, GroundLayer);

        // Don't allow the camera to leave the ground area
        position.x = Mathf.Clamp(position.x, 0 - (mapSize / 2), 0 + (mapSize / 2));
        position.y = Mathf.Clamp(position.y, _rayHit.point.y + cameraMinHeight, _rayHit.point.y + cameraMaxHeight);
        position.z = Mathf.Clamp(position.z, 0 - (mapSize / 2), 0 + (mapSize / 2));

        // Effects when camera hit the ground or the top surface
        if (position.y <= _rayHit.point.y + cameraMinHeight + 1)
            rotation.x = 20;
        else if (position.y >= _rayHit.point.y + cameraMaxHeight - 1)
            rotation.x = 70;

        // Save Changes
        TheCamera.transform.position = Vector3.Slerp(TheCamera.transform.position, position, .8f);
        TheCamera.transform.eulerAngles = Vector3.Slerp(TheCamera.transform.eulerAngles, rotation, .2f);
    }
}
