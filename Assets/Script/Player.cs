using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]private float m_moveSpeed;

    private float m_vertical;
    private float m_horizontal;

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

        float _targetAngle = Mathf.Atan2(_moveDirection.x,_moveDirection.z)*Mathf.Rad2Deg;
        transform.eulerAngles = Vector3.up * _targetAngle;

        transform.Translate(transform.forward*m_moveSpeed*_inputMagnitude*Time.deltaTime,Space.World);
    }

}
