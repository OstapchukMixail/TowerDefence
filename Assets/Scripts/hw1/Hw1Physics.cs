using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hw1Physics : MonoBehaviour
{
    [SerializeField]
    private Vector3 m_rStart;
    [SerializeField]
    private Vector3 m_VStart;
    
    [SerializeField]
    private float m_M;
    
    [SerializeField]
    private float m_G;
    
    [SerializeField]
    private Vector3 m_R;

    private Vector3 r;
    
    private Vector3 V;
    void Start()
    {
        r = m_rStart;
        V = m_VStart;
        transform.position = m_rStart;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 a = (float)(m_G * m_M / (Math.Pow((m_R - r).magnitude, 2))) * (m_R - r).normalized;
        Vector3 r_next = r + V * Time.deltaTime + (float)(0.5 * Math.Pow(Time.deltaTime, 2)) * a;
        Vector3 V_next = V + a * Time.deltaTime;
        Vector3 dir = (r_next - transform.position).normalized;
        Vector3 delta = dir * (V_next.magnitude * Time.deltaTime);
        transform.Translate(delta);
        r = transform.position;
        V = V_next;

    }
}
