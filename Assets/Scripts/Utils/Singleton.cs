using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T:Singleton<T>
{
    private static T _instance;
    private static object _lock = new object();

    protected Action OnGameEndEvent;

    public static T MainInstance
    {
        get
        {
            if (_instance==null) {
                lock (_lock) {
                    _instance = FindObjectOfType<T>() as T;
                    if (_instance==null) {
                        GameObject go = new GameObject(typeof(T).Name);
                        _instance = go.AddComponent<T>();
                    }
                }
            }

            return _instance;
        }
        
    }

    protected virtual void Awake() {
        if (_instance==null) {
            _instance = (T)this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    protected abstract void InvokeGameEndEvent();
    protected abstract void ApplicationQuit();

    private void OnApplicationQuit() {
        ApplicationQuit();
        _instance = null;
    }
}
