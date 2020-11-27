using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]private float m_moveSpeed=5f;
    [SerializeField]private float m_smoothMoveTime=0.2f;
    [SerializeField]private float m_turnSpeed=5f;

    private float m_vertical;
    private float m_horizontal;

    private float m_currentAngle;
    private float m_smoothInputMagnitude;
    private float m_smoothMoveVelocity;

//`````````````````````````````````````````````````````````````````````````````````````````````````````````````````````````
//`````````````````````````````````````````````````````````````````````````````````````````````````````````````````````````

    private void Update()
    {
        m_vertical = Input.GetAxisRaw("Vertical");
        m_horizontal = Input.GetAxisRaw("Horizontal");

        PlayerMovement();
    }

//`````````````````````````````````````````````````````````````````````````````````````````````````````````````````````````
//`````````````````````````````````````````````````````````````````````````````````````````````````````````````````````````

    private void PlayerMovement(){

        Vector3 _moveDirection = new Vector3(m_horizontal,0,m_vertical).normalized;
        float _inputMagnitude = _moveDirection.magnitude;
        m_smoothInputMagnitude = Mathf.SmoothDamp(m_smoothInputMagnitude,_inputMagnitude,ref m_smoothMoveVelocity,m_smoothMoveTime);

        float _targetAngle = Mathf.Atan2(_moveDirection.x,_moveDirection.z)*Mathf.Rad2Deg;
        m_currentAngle = Mathf.LerpAngle(m_currentAngle,_targetAngle,Time.deltaTime * m_turnSpeed * _inputMagnitude);
        transform.eulerAngles = Vector3.up * m_currentAngle;

        transform.Translate(transform.forward * m_moveSpeed * m_smoothInputMagnitude * Time.deltaTime,Space.World);
    }

}
