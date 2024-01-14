using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerAnimationScript : MonoBehaviour
{
    PlayerInput m_input;
    Animator m_animator;
    // Start is called before the first frame update
    void Start()
    {
        m_input = GetComponent<PlayerInput>();
        m_animator = GetComponent<Animator>();  
        m_input.currentActionMap.FindAction("Move").performed += MoveStart;
        m_input.currentActionMap.FindAction("Move").canceled += MoveEnd;
    }

    void MoveStart(InputAction.CallbackContext context)
    {
        m_animator.SetBool("Moving", true);
        Vector2 temp = context.ReadValue<Vector2>();
        m_animator.SetFloat("X", temp.x);
        m_animator.SetFloat ("Y", temp.y);
    }

    void MoveEnd(InputAction.CallbackContext context)
    {
        m_animator.SetBool("Moving", false);
    }

    
}
