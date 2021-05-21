using System;
 using UnityEngine;
 using UnityEngine.Serialization;

 namespace Field
 {
    public class GridHolder : MonoBehaviour
    {
        [SerializeField]
        private int m_GridWidth;
         [SerializeField]
         private int m_GridHeight;

         [SerializeField] 
         private Vector2Int m_TargetCoordiate;
         [SerializeField] 
         private Vector2Int m_StartCoordinate;

         [SerializeField]
         private float m_NodeSize;

         private Grid m_Grid;
         
         private Camera m_Camera;

         private Vector3 m_Offset;

         public Vector2Int MStartCoordinate => m_StartCoordinate;

         public Grid MGrid => m_Grid;

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
                        (new Vector3(width, 0, height) * 0.5f);

             m_Grid = new Grid(m_GridWidth, m_GridHeight, m_Offset, m_NodeSize, m_TargetCoordiate, m_StartCoordinate);
         }
         
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
                        (new Vector3(width, 0, height) * 0.5f);

             m_Grid = new Grid(m_GridWidth, m_GridHeight, m_Offset, m_NodeSize, m_TargetCoordiate, m_StartCoordinate);
         }
         

         private void Update()
        {
            if (m_Grid == null || m_Camera == null)
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
                Vector3 hitPosition = hit.point;
                 Vector3 difference = hitPosition - m_Offset;

                 int x = (int) (difference.x / m_NodeSize);
                 int y = (int) (difference.z / m_NodeSize);
                 Vector3 targetPosition = new Vector3(
                     x * m_NodeSize + m_Offset.x + m_NodeSize * 0.5f,
                     0,
                     y * m_NodeSize + m_Offset.z + m_NodeSize * 0.5f);

                 if (Input.GetMouseButtonDown(0))
                 {
                     m_Grid.TryOccupyNode(new Vector2Int(x, y));
                 }

                 if (Input.GetMouseButtonDown(1))
                 {
                     Node gridNode = m_Grid.GetNode(x, y);
                     if (gridNode.m_IsOccupied)
                     {
                         gridNode.m_IsOccupied = false;
                         m_Grid.UpdatePathFinding();
                     }
                 }
             }
         }

         private void OnDrawGizmos()
         {
             if (m_Grid == null)
             {
                 return;
             }

             foreach (Node node in m_Grid.EnumerateAllNodes())
             {
                 if (node.m_OccupationAvailability == OccupationAvailability.CanOccupy)
                 {
                     Gizmos.color = Color.gray;
                 }
                 if (node.m_OccupationAvailability == OccupationAvailability.CanNotOccupy)
                 {
                     Gizmos.color = Color.red;
                 }
                 if (node.m_OccupationAvailability == OccupationAvailability.Undefined)
                 {
                     Gizmos.color = Color.green;
                 }
                 Gizmos.DrawCube(node.m_Position, new Vector3(m_NodeSize * 0.90f, 0.001f, m_NodeSize * 0.90f));

                 Gizmos.color = Color.red;
                 if (node.m_NextNode == null)
                 {
                     continue;
                 }

                 if (node.m_IsOccupied)
                 {
                     Gizmos.color = Color.black;
                     Gizmos.DrawSphere(node.m_Position, 0.5f);
                     continue;
                 }
                 Vector3 start = node.m_Position;
                 Vector3 end = node.m_NextNode.m_Position;

                 Vector3 dir = (end - start);

                 start -= dir * 0.25f;
                 end -= dir * 0.75f;

                 Gizmos.DrawLine(start, end);
                 Gizmos.DrawSphere(end, 0.1f);
             }
         }
     }
 } 