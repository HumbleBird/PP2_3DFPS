using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayerManager : PlayerManager
{
    public CameraHandler cameraHandler;
    public InputHandler inputHandler;

    protected override void Awake()
    {
        base.Awake();

        inputHandler = GetComponent<InputHandler>();
        cameraHandler = GetComponentInChildren<CameraHandler>();
    }

    protected override void Start()
    {
        base.Start();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
