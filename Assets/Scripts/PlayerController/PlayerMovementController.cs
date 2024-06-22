using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementController : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Animator _animator;
    public Vector3 CurrentInput { get; private set; }
    public float MaxWalkSpeed;
    public float MaxRunSpeed;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void FixedUpdate() {
        if (CurrentInput!=Vector3.zero) {
            _rigidbody.MovePosition(_rigidbody.position + CurrentInput * MaxWalkSpeed*Time.fixedDeltaTime);
            _animator.SetBool("walk",true);
        }
        else {
            _animator.SetBool("walk",false);
        }
    }

    public void SetMovementInput(Vector3 input) {
        CurrentInput = Vector3.ClampMagnitude(input, 1);
    }
    
    public void SetRotationInput(Quaternion input) {
        _rigidbody.MoveRotation(input);
    }
}
