using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviourPunCallbacks
{
    [SerializeField] float dashSpeed = 10f;

    public float moveSpeed = 5f;

    private PlayerInput _playerInput;
    private Rigidbody2D _rb;
    private Vector2 _movement;
    [SerializeField]
    private bool _isDash;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        _movement = _playerInput.MovementInput;
        _isDash = _playerInput.IsDash;
    }

    private void FixedUpdate()
    {
        MoveControl();
    }

    void MoveControl()
    {
        if (_isDash)
        {
            _rb.velocity = _movement * dashSpeed;
        }
        else
        {
            _rb.velocity = _movement * moveSpeed;
        }
    }
}
