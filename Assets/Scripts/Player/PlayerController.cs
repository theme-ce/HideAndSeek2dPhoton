using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviourPunCallbacks
{
    public float moveSpeed = 5f;

    private PlayerInput _playerInput;
    private Rigidbody2D _rb;
    private Vector2 _movement;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        _movement = _playerInput.MovementInput;
    }

    private void FixedUpdate()
    {
        MoveControl();
    }

    void MoveControl()
    {
        _rb.MovePosition(_rb.position + _movement * moveSpeed * Time.fixedDeltaTime);
    }
}
