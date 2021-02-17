 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementAgent : MonoBehaviour
{
    [SerializeField]
    private float m_Speed; 
    [SerializeField]
    private Vector3 m_Target;
    private const float TOLERANCE = 0.1f;
    void Start()
    {
        m_Speed = 5f;
        m_Target = new Vector3(x: 10f, y: 0f, z: 10f);
    }

    // Update is called once per frame
    void Update()
    {
        float distance = (m_Target - transform.position).magnitude;
        if (distance < TOLERANCE)
        {
            return;
        }

        Vector3 dir = (m_Target - transform.position).normalized;
        Vector3 delta = dir * (m_Speed * Time.deltaTime);
        transform.Translate(delta);
    }
}
