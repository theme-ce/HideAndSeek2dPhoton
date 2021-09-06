using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerInput : MonoBehaviourPunCallbacks
{
    private Animator _animator;

    public Vector2 MovementInput
    {
        get
        {
            return m_Movement;
        }
    }

    private Vector2 m_Movement;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            ProcessMovementInput();
        }
    }

    void ProcessMovementInput()
    {
        float movement_X = Input.GetAxisRaw("Horizontal");
        float movement_Y = Input.GetAxisRaw("Vertical");

        m_Movement = new Vector2(movement_X, movement_Y);

        if (m_Movement != Vector2.zero)
        {
            _animator.SetFloat("Horizontal", movement_X);
            _animator.SetFloat("Vertical", movement_Y);
        }
        
        _animator.SetFloat("Speed", m_Movement.magnitude);
    }
}
