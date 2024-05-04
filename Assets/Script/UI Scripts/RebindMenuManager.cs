using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RebindMenuManager : MonoBehaviour
{
    public InputActionReference MoveRef, JumpRef, FireRef;

    void Start()
    {

    }


    private void OnEnable()
    {
        MoveRef.action.Disable();
        JumpRef.action.Disable();
        FireRef.action.Disable();
    }

    private void OnDisable()
    {
        MoveRef.action.Enable();
        JumpRef.action.Enable();
        FireRef.action.Enable();
    }

}
