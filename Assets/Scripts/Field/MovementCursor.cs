using System;
using UnityEngine;

namespace Field
{
    public class MovementCursor : MonoBehaviour
    {
        [SerializeField]
        private int m_GridWidth;
        [SerializeField]
        private int m_GridHeight;

        [SerializeField]
        private MovementAgent m_MovementAgent;

        [SerializeField]
        private float m_NodeSize;

    
    
        private Camera m_Camera;

        private Vector3 m_Offset;
        
        [SerializeField]
        private GameObject m_Cursor;
        

        private void Start()
        {
            m_Camera = Camera.main;

            float width = m_GridWidth * m_NodeSize;
            float height = m_GridHeight * m_NodeSize;

            // Default plane size is 10 by 10
            transform.localScale = new Vector3(
                width * 0.1f, 
                1f,
                height * 0.1f);

            m_Offset = transform.position -
                       (new Vector3(width, 0f, height) * 0.5f);
        }

        private void OnValidate()
        {
            float width = m_GridWidth * m_NodeSize;
            float height = m_GridHeight * m_NodeSize;

            // Default plane size is 10 by 10
            transform.localScale = new Vector3(
                width * 0.1f, 
                1f,
                height * 0.1f);

            m_Offset = transform.position -
                       (new Vector3(width, 0f, height) * 0.5f);
        }

        private void Update()
        {
            if (m_Camera == null)
            {
                return;
            }

            Vector3 mousePosition = Input.mousePosition;

            Ray ray = m_Camera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform != transform)
                {
                    return;
                }
                if (m_Cursor.activeSelf == false)
                {
                    m_Cursor.SetActive(true);
                }
                
                Vector3 hitPosition = hit.point;
                Vector3 difference = hitPosition - m_Offset;

                int x = (int) (difference.x / m_NodeSize);
                int z = (int) (difference.z / m_NodeSize);

                //Debug.Log(x + " " + z);
                
                Vector3 nodeCenter = new Vector3(
                    m_Offset.x + x * m_NodeSize + m_NodeSize * 0.5f,
                    0f,
                    m_Offset.z + z * m_NodeSize + m_NodeSize * 0.5f);
                m_Cursor.transform.position = nodeCenter;
                if (Input.GetMouseButton(0))
                {
                    Debug.Log("click" + " " + x + " " + z);
                    m_MovementAgent.SetTarget(nodeCenter);
                }
                
            }
            else
            {
                m_Cursor.SetActive(false);
            }
        }

        private void OnDrawGizmos()
        {
            float width = m_GridWidth * m_NodeSize;
            float height = m_GridHeight * m_NodeSize;
            Gizmos.color = Color.black;
            for (int i = 0; i < m_GridWidth; i++)
            {
                Vector3 start = new Vector3(
                    i * m_NodeSize,
                    0f,
                    0f) + m_Offset;
                Vector3 end = new Vector3(
                    i * m_NodeSize,
                    0f,
                    height) + m_Offset;
                Gizmos.DrawLine(start, end);
            }
            for (int j = 0; j <= m_GridHeight; j++)
            {
                Vector3 start = new Vector3(
                    0f,
                    0f,
                    j * m_NodeSize) + m_Offset;
                Vector3 end = new Vector3(
                    width,
                    0f,
                    j * m_NodeSize) + m_Offset;
                Gizmos.DrawLine(start, end);
            }
        }
    }
}