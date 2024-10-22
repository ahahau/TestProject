using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private NewControls _keyAction;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();

        _keyAction = new NewControls();
        _keyAction.PlayerInput.Enable();
        _keyAction.PlayerInput.Jump.performed += JumpPerformed;

        _keyAction.PlayerInput.Move.performed += MovementPerformed;
    }

    private void Update()
    {
        Vector2 inputVector = _keyAction.PlayerInput.Move.ReadValue<Vector2>();
        float speed = 20f;
        _rigidbody.AddForce(new Vector3(inputVector.x, 0, inputVector.y) * speed, ForceMode.Force);
    }
    private void MovementPerformed(InputAction.CallbackContext context)
    {
        Debug.Log(context);
        Vector2 inputVector = context.ReadValue<Vector2>();
        float speed = 20f;
        _rigidbody.AddForce(new Vector3(inputVector.x, 0, inputVector.y) * speed, ForceMode.Force);
    }


    private void JumpPerformed(InputAction.CallbackContext context)
    {
        _rigidbody.AddForce(Vector3.up * 5f, ForceMode.Impulse);
    }

}
