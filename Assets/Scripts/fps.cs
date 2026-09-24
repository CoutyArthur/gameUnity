using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

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


    Animator _animatorController;
    Collider _collider;

    [SerializeField] GameObject _projectile;
    float _projectileSpeed = 50f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _moveAction = InputSystem.actions.FindAction("move");
        _moveAction.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
        _moveAction.canceled += ctx => _moveInput = Vector2.zero;

        _jumpAction = InputSystem.actions.FindAction("jump");
        _jumpAction.performed += ctx => jump();

        _fireAction = InputSystem.actions.FindAction("attack");
        _fireAction.performed += ctx => fire();

        _lookAction = InputSystem.actions.FindAction("look");
        _lookAction.performed += ctx => _lookInput = ctx.ReadValue<Vector2>();
        _lookAction.canceled += ctx => _lookInput = Vector2.zero;

        _sprintAction = InputSystem.actions.FindAction("sprint");
        _sprintAction.performed += ctx => _sprintInput = true;
        _sprintAction.canceled += ctx => _sprintInput = false;

        Cursor.lockState = CursorLockMode.Locked;


        _rigidbody = GetComponent<Rigidbody>();
        _animatorController = GetComponent<Animator>();
        _collider = GetComponent<BoxCollider>();


    }

    // Update is called once per frame
    void Update()
    {
        if (_moveAction != null)
        {
            walk();
        }
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
        if (_rigidbody.linearVelocity.magnitude > 0 && _rigidbody.linearVelocity.y == 0)
            _animatorController.SetBool("walk", true);
        else
            _animatorController.SetBool("walk", false);
    }

    void jump()
    {
        //_rigidbody.linearVelocity = new Vector3(_rigidbody.linearVelocity.x, _jumpHeight, _rigidbody.linearVelocity.z);
        _rigidbody.AddForce(new Vector3(0, _jumpHeight, 0), ForceMode.Impulse);

    }

    void fire()
    {
        Vector3 offset = new Vector3(0, 1.5f, 0);
        GameObject p = Instantiate(_projectile, transform.position + transform.forward + offset, transform.rotation);
        Rigidbody rbp = p.GetComponent<Rigidbody>();
        rbp.linearVelocity += transform.forward * _projectileSpeed;
        Destroy(p, 5f);
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
            _animatorController.SetBool("jump", false);
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("Ground"))
            _animatorController.SetBool("jump", true);
    }
}
