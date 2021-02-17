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
        m_rStart = new Vector3(10f, 3f, 4f);
        m_VStart = new Vector3(0f, 3f, 2f);
        m_G = 6.67f * (float)Math.Pow(10, -11);
        m_R = new Vector3(100000f, 23434f, 523423f);
        m_M = 10000000000;
        r = m_rStart;
        V = m_VStart;
        transform.position = m_rStart;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 a = (float)(m_G * m_M / (Math.Pow((m_R - r).magnitude, 2))) * (m_R - r).normalized;
        Vector3 r_next = r + V.normalized * (Time.deltaTime * V.magnitude) +
                         ((float)(0.5 * Math.Pow(Time.deltaTime, 2) * a.magnitude)) * a.normalized;
        Vector3 V_next = V + a.normalized * ((float) a.magnitude * Time.deltaTime);
        Vector3 dir = (r_next - transform.position).normalized;
        Vector3 delta = dir * (V_next.magnitude * Time.deltaTime);
        transform.Translate(delta);
        r = r_next;
        V = V_next;

    }
}
