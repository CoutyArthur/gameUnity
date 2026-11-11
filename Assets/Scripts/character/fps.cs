using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class fps : MonoBehaviour
{

    InputAction _moveAction;
    InputAction _jumpAction;
    InputAction _fireAction;
    InputAction _sprintAction;
    Rigidbody _rigidbody;
    Vector2 _moveInput;
    float _jumpHeight = 5f;
    [SerializeField] float _moveSpeed = 5f;
    float _sprintBoost = 1.5f;
    bool _sprintInput;

    //Rotation de la caméra
    float rotationX = 0;
    [SerializeField] public float rotationSpeed = 0.5f;
    public float rotationXLimit = 45.0f;

    InputAction _lookAction;
    Vector2 _lookInput;
    [SerializeField] Transform _cameraTransform; // à assigner dans l'Inspector

    bool onGround = true;

    Collider _collider;

    bool _fireInput;
    float nextTimeToFire = 0f;
    float fireRate = 0.15f;
    float fireRange = 100f;
    public static event Action<bool> OnTargetHit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _moveAction = InputSystem.actions.FindAction("move");
        _moveAction.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
        _moveAction.canceled += ctx => _moveInput = Vector2.zero;

        _jumpAction = InputSystem.actions.FindAction("jump");
        _jumpAction.performed += ctx => jump();

        _fireAction = InputSystem.actions.FindAction("attack");
        _fireAction.performed += ctx => _fireInput = true;
        _fireAction.canceled += ctx => _fireInput = false;

        _lookAction = InputSystem.actions.FindAction("look");
        _lookAction.performed += ctx => _lookInput = ctx.ReadValue<Vector2>();
        _lookAction.canceled += ctx => _lookInput = Vector2.zero;

        _sprintAction = InputSystem.actions.FindAction("sprint");
        _sprintAction.performed += ctx => _sprintInput = true;
        _sprintAction.canceled += ctx => _sprintInput = false;

        Cursor.lockState = CursorLockMode.Locked;


        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<BoxCollider>();


    }

    // Update is called once per frame
    void Update()
    {
        if (_moveAction != null)
        {
            walk();
        }
        if(_fireInput) fire();
        look();
    }

    void walk()
    {
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        // On ignore la composante Y pour ne pas incliner le déplacement si la caméra a un pitch
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 move = (forward * _moveInput.y + right * _moveInput.x) * _moveSpeed;
        if( _sprintInput )
            move = move * _sprintBoost;
        //this.transform.Translate(move * _moveSpeed * Time.deltaTime);
        _rigidbody.linearVelocity = new Vector3(move.x * _moveSpeed, _rigidbody.linearVelocity.y, move.z * _moveSpeed);
    }

    void jump()
    {
        //_rigidbody.linearVelocity = new Vector3(_rigidbody.linearVelocity.x, _jumpHeight, _rigidbody.linearVelocity.z);
        if(onGround)
            _rigidbody.AddForce(new Vector3(0, _jumpHeight, 0), ForceMode.Impulse);

    }

    void fire()
    {
        if (Time.time < nextTimeToFire) return;
        nextTimeToFire = Time.time + fireRate;

        Ray ray = new Ray(_cameraTransform.position, _cameraTransform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, fireRange, ~0, QueryTriggerInteraction.Ignore))
        {
            if (hit.collider.CompareTag("Target"))
            {
                OnTargetHit?.Invoke(true);
                Destroy(hit.collider.gameObject);
            }
        }
    }

    void look()
    {
        // Yaw : rotation horizontale appliquée au corps (le joueur tourne entièrement)
        transform.Rotate(Vector3.up * _lookInput.x * rotationSpeed);

        // Pitch : rotation verticale appliquée uniquement à la caméra, avec clamp
        rotationX -= _lookInput.y * rotationSpeed;
        rotationX = Mathf.Clamp(rotationX, -rotationXLimit, rotationXLimit);

        if (_cameraTransform != null)
            _cameraTransform.localRotation = Quaternion.Euler(rotationX, 0, 0);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Ground"))
            onGround = true;
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("Ground"))
            onGround = false;
    }
}
