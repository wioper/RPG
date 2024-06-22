using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{

    private PlayerInput _gameInputAction;

    public bool Attack => _gameInputAction.GamePlay.Attack.triggered;
    public bool Run => _gameInputAction.GamePlay.Run.IsPressed();
    public bool Move => _gameInputAction.GamePlay.Move.IsPressed();

    protected override void Awake() {
        base.Awake();
        _gameInputAction ??= new PlayerInput();
    }

    private void OnEnable() {
        _gameInputAction.Enable();
    }

    private void OnDisable() {
        _gameInputAction.Disable();
    }

    protected override void InvokeGameEndEvent() {
        Debug.Log("..");
    }

    protected override void ApplicationQuit() {
        Debug.Log("..");
    }
}
