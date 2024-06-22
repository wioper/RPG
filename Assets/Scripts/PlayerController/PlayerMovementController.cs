using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementController : MonoBehaviour
{
    private Rigidbody _rigidbody;
    public Vector3 CurrentInput { get; private set; }
    public float MaxWalkSpeed = 5;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        // _characterController.Move(_rigidbody.position + CurrentInput * MaxWalkSpeed);
        _rigidbody.MovePosition(_rigidbody.position + CurrentInput * MaxWalkSpeed*Time.fixedDeltaTime);
    }

    public void SetMovementInput(Vector3 input) {
        CurrentInput = Vector3.ClampMagnitude(input, 1);
    }
}
