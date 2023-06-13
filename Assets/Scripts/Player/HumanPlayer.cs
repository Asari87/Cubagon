using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HumanPlayer : Player
{
    private CubagonManager gameManager;
    private bool activeTurn = false;
    private HumanControls controls;
    protected override void Awake()
    {
        base.Awake();
        controls = new HumanControls();
        controls.HumanPlayer.Tap.performed += HandleInput;
        gameManager = FindObjectOfType<CubagonManager>();
    }
    private void OnEnable()
    {
        controls.HumanPlayer.Enable();
    }
    private void OnDisable()
    {
        controls.HumanPlayer.Disable();    
    }
    private void OnDestroy()
    {
        controls.HumanPlayer.Tap.performed -= HandleInput;
    }

    private void HandleInput(InputAction.CallbackContext context)
    {
        if (!activeTurn) return;

        Vector2 pos = controls.HumanPlayer.ScreenPoint.ReadValue<Vector2>();
        Vector3 mPos = Utilities.GetMouseWorldPosition(pos);
        gameManager.HandlePlayerInput(this, mPos);
        
    }

    public override void DisableControl()
    {
        activeTurn = false;
    }

    public override void EnableControl()
    {
        activeTurn = true;
    }
}
