using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class cour1 : MonoBehaviour
{

    InputAction _moveAction;
    InputAction _jumpAction;
    InputAction _fireAction;
    Rigidbody _rigidbody;
    Vector2 _moveInput;
    bool _jumpInput;
    float _jumpHeight = 5f;
    float _moveSpeed = 5f;


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


        _rigidbody = GetComponent<Rigidbody>();
        _animatorController = GetComponent<Animator>();
        _collider = GetComponent<BoxCollider>();

        
    }

    // Update is called once per frame
    void Update()
    {
        if(_moveAction != null)
        {
            walk();
        }
    }

    void walk()
    {
        Vector3 move = new Vector3(_moveInput.x, 0, _moveInput.y);
        //this.transform.Translate(move * _moveSpeed * Time.deltaTime);
        _rigidbody.linearVelocity = new Vector3(move.x * _moveSpeed, _rigidbody.linearVelocity.y, move.z * _moveSpeed);
        if(_rigidbody.linearVelocity.magnitude > 0 && _rigidbody.linearVelocity.y == 0)
            _animatorController.SetBool("walk", true);
        else
            _animatorController.SetBool("walk", false);
    }

    void jump()
    {
        //_rigidbody.linearVelocity = new Vector3(_rigidbody.linearVelocity.x, _jumpHeight, _rigidbody.linearVelocity.z);
        _rigidbody.AddForce(new Vector3(0,_jumpHeight,0), ForceMode.Impulse);
            
    }

    void fire()
    {
        Vector3 offset = new Vector3(0,1.5f,0);
        GameObject p = Instantiate(_projectile, transform.position + transform.forward + offset, transform.rotation);
        Rigidbody rbp = p.GetComponent<Rigidbody>();
        rbp.linearVelocity +=  transform.forward * _projectileSpeed;
        Destroy(p,5f); 
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.CompareTag("Ground"))           
            _animatorController.SetBool("jump", false);
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("Ground"))
            _animatorController.SetBool("jump", true);
    }
}
