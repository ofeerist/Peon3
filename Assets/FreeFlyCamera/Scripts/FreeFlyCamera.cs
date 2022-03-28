//===========================================================================//
//                       FreeFlyCamera (Version 1.2)                         //
//                        (c) 2019 Sergey Stafeyev                           //
//===========================================================================//

using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FreeFlyCamera : MonoBehaviour
{
    #region UI

    [Space]

    [SerializeField]
    [Tooltip("The script is currently active")]
    private bool _active = true;

    [Space]

    [SerializeField]
    [Tooltip("Camera rotation by mouse movement is active")]
    private bool _enableRotation = true;

    [SerializeField]
    [Tooltip("Sensitivity of mouse rotation")]
    private float _mouseSense = 1.8f;

    [Space]

    [SerializeField]
    [Tooltip("Camera zooming in/out by 'Mouse Scroll Wheel' is active")]
    private bool _enableTranslation = true;

    [SerializeField]
    [Tooltip("Velocity of camera zooming in/out")]
    private float _translationSpeed = 55f;

    [Space]

    [SerializeField]
    [Tooltip("Camera movement by 'W','A','S','D','Q','E' keys is active")]
    private bool _enableMovement = true;

    [SerializeField]
    [Tooltip("Camera movement speed")]
    private float _movementSpeed = 10f;

    [SerializeField]
    [Tooltip("Speed of the quick camera movement when holding the 'Left Shift' key")]
    private float _boostedSpeed = 50f;

    [SerializeField]
    [Tooltip("Boost speed")]
    private KeyCode _boostSpeed = KeyCode.LeftShift;

    [SerializeField]
    [Tooltip("Move up")]
    private KeyCode _moveUp = KeyCode.E;

    [SerializeField]
    [Tooltip("Move down")]
    private KeyCode _moveDown = KeyCode.Q;

    [Space]

    [SerializeField]
    [Tooltip("Acceleration at camera movement is active")]
    private bool _enableSpeedAcceleration = true;

    [SerializeField]
    [Tooltip("Rate which is applied during camera movement")]
    private float _speedAccelerationFactor = 1.5f;

    [Space]

    [SerializeField]
    [Tooltip("This keypress will move the camera to initialization position")]
    private KeyCode _initPositonButton = KeyCode.R;

    #endregion UI

    private CursorLockMode _wantedMode;

    private float _currentIncrease = 1;
    private float _currentIncreaseMem = 0;

    private Vector3 _initPosition;
    private Vector3 _initRotation;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_boostedSpeed < _movementSpeed)
            _boostedSpeed = _movementSpeed;
    }
#endif


    private void Start()
    {
        var transform1 = transform;
        _initPosition = transform1.position;
        _initRotation = transform1.eulerAngles;
    }

    private void OnEnable()
    {
        if (_active)
            _wantedMode = CursorLockMode.Locked;
    }

    // Apply requested cursor state
    private void SetCursorState()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = _wantedMode = CursorLockMode.None;
        }

        if (Input.GetMouseButtonDown(0))
        {
            _wantedMode = CursorLockMode.Locked;
        }

        // Apply cursor state
        Cursor.lockState = _wantedMode;
        // Hide cursor when locking
        Cursor.visible = (CursorLockMode.Locked != _wantedMode);
    }

    private Vector3 _direction;
    private void Update()
    {
        var position = transform.position;
        
        _direction = Vector3.Lerp(_direction, Vector3.zero, 3f * Time.deltaTime);

        var mousePosition = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
            mousePosition += transform.forward;

        if (Input.GetKey(KeyCode.S))
            mousePosition -= transform.forward;

        if (Input.GetKey(KeyCode.A))
            mousePosition -= transform.right;

        if (Input.GetKey(KeyCode.D))
            mousePosition += transform.right;

        if (Input.GetKey(_moveUp))
            mousePosition += transform.up;

        if (Input.GetKey(_moveDown))
            mousePosition -= transform.up;

        _direction += mousePosition * _movementSpeed * Time.deltaTime;
        
        // Pitch
        Transform transform2;
        (transform2 = transform).rotation *= Quaternion.AngleAxis(
            -Input.GetAxis("Mouse Y") * _mouseSense,
            Vector3.right
        );

        // Paw
        Transform transform1;
        var eulerAngles = transform2.eulerAngles;
        (transform1 = transform).rotation = Quaternion.Euler(
            eulerAngles.x,
            eulerAngles.y + Input.GetAxis("Mouse X") * _mouseSense,
            transform.eulerAngles.z
        );
        

        position += _direction * Time.deltaTime;

        transform1.position = position;

        SetCursorState();
    }
}
