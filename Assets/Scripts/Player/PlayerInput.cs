using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerInput : MonoBehaviour
{
    PhotonView view;
    Animator animator;

    public Vector2 MovementInput
    {
        get
        {
            return m_Movement;
        }
    }

    public bool IsDash
    {
        get
        {
            return m_Dash;
        }
    }

    private Vector2 m_Movement;
    private bool m_Dash;

    private void Start()
    {
        animator = GetComponent<Animator>();
        view = GetComponent<PhotonView>();    
    }

    private void Update()
    {
        if(view.IsMine)
        {
            ProcessMovementInput();
        }
    }

    void ProcessMovementInput()
    {
        float movement_X = Input.GetAxisRaw("Horizontal");
        float movement_Y = Input.GetAxisRaw("Vertical");

        m_Movement = new Vector2(movement_X, movement_Y);

        animator.SetFloat("Speed", m_Movement.magnitude);
    }

    void ProcessDashInput()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Dashing());
        }
    }

    IEnumerator Dashing()
    {
        m_Dash = true;

        yield return new WaitForSeconds(0.2f);

        m_Dash = false;
    }
}
