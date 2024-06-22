using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{

    private PlayerMovementController _playerMovementController;
    
    [SerializeField] private Photographer _photographer;

    [SerializeField] private Transform _followingTarget;

    private void Awake() {
        _playerMovementController = GetComponent<PlayerMovementController>();

        _photographer.InitCamera(_followingTarget);
    }

    private void Update() {
        UpdateMovementInput();
        UpdateRotationInput();
    }

    private void UpdateRotationInput() {
        Quaternion rot = Quaternion.Euler(0, _photographer.Yaw, 0);
        _playerMovementController.SetRotationInput(rot);
    }

    private void UpdateMovementInput() {
        Quaternion rot = Quaternion.Euler(0, _photographer.Yaw, 0);
        _playerMovementController.SetMovementInput(rot * Vector3.forward * Input.GetAxis("Vertical") +
                                                   rot * Vector3.right * Input.GetAxis("Horizontal"));
    }
}
