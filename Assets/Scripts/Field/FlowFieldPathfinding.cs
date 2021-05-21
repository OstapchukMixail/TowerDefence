using System.Collections.Generic;
using UnityEngine.Serialization;
using UnityEngine;
using System;

 namespace Field
 {
     public class FlowFieldPathfinding
     {
         private Grid m_Grid;
         private Vector2Int m_Target;
         private Vector2Int m_Start;
         
         public FlowFieldPathfinding(Grid grid, Vector2Int target, Vector2Int start)
         {
             m_Grid = grid;
             m_Target = target;
             m_Start = start;
         }

         public void UpdateField()
         {
             ResetWeights();
             Queue<Vector2Int> queue = new Queue<Vector2Int>();

             queue.Enqueue(m_Target);
             m_Grid.GetNode(m_Target).m_PathWeight = 0f;

             Node currentNode;
             while (queue.Count > 0)
             {
                 Vector2Int current = queue.Dequeue();
                 currentNode = m_Grid.GetNode(current);

                 foreach (Connection neighbour in GetNeighbours(current))
                 {
                     Node neighbourNode = m_Grid.GetNode(neighbour.Coordinate);
                     if (currentNode.m_PathWeight + neighbour.Weight < neighbourNode.m_PathWeight)
                     {
                         neighbourNode.m_NextNode = currentNode;
                         neighbourNode.m_PathWeight = currentNode.m_PathWeight + neighbour.Weight;
                         queue.Enqueue(neighbour.Coordinate);
                     }
                 }
             }
             

             CalculateOccupationAvailability();
         }

         private void ResetWeights()
         {
             foreach (Node node in m_Grid.EnumerateAllNodes())
             {
                 node.ResetWeight();
             }
         }
         
         private void CalculateOccupationAvailability()
         {
             foreach (Node node in m_Grid.EnumerateAllNodes())
             {
                 if (node.m_IsOccupied)
                 {
                     node.m_OccupationAvailability = OccupationAvailability.CanNotOccupy;
                 }
                 else
                 {
                     node.m_OccupationAvailability = OccupationAvailability.CanOccupy;
                 }
             }
             Node currentNode = m_Grid.GetNode(m_Start);
             Node targetNode = m_Grid.GetNode(m_Target);
             currentNode.m_OccupationAvailability = OccupationAvailability.CanNotOccupy;
             targetNode.m_OccupationAvailability = OccupationAvailability.CanNotOccupy;

             // Go from start to target and mark the way
             currentNode = currentNode.m_NextNode;
             while (currentNode != null && currentNode != targetNode)
             {
                 currentNode.m_OccupationAvailability = OccupationAvailability.Undefined;
                 currentNode = currentNode.m_NextNode;
             }
         }
         
         public bool CanOccupy(Vector2Int coordinate)
         {
             Node nodeToOccupy = m_Grid.GetNode(coordinate);
             if (nodeToOccupy.m_OccupationAvailability == OccupationAvailability.CanOccupy)
             {
                 return true;
             }
             if (nodeToOccupy.m_OccupationAvailability == OccupationAvailability.CanNotOccupy)
             {
                 return false;
             }
             nodeToOccupy.m_IsOccupied = true;

             ResetWeights();
             Queue<Vector2Int> queue = new Queue<Vector2Int>();

             queue.Enqueue(m_Target);
             m_Grid.GetNode(m_Target).m_PathWeight = 0f;

             while (queue.Count > 0)
             {
                 Vector2Int current = queue.Dequeue();
                 float currentPathWeight = m_Grid.GetNode(current).m_PathWeight;

                 foreach (Connection connection in GetNeighbours(current))
                 {
                     if (connection.Coordinate == m_Start)
                     {
                         nodeToOccupy.m_IsOccupied = false;
                         nodeToOccupy.m_OccupationAvailability = OccupationAvailability.CanOccupy;
                         return true;
                     }

                     Node neighbourNode = m_Grid.GetNode(connection.Coordinate);
                     if (currentPathWeight + connection.Weight < neighbourNode.m_PathWeight)
                     {
                         neighbourNode.m_PathWeight = currentPathWeight + connection.Weight;
                         queue.Enqueue(connection.Coordinate);
                     }
                 }
             }

             nodeToOccupy.m_IsOccupied = false;
             nodeToOccupy.m_OccupationAvailability = OccupationAvailability.CanNotOccupy;
             return false;
         }
         
         private IEnumerable<Connection> GetNeighbours(Vector2Int coordinate)
         {
             float straightMovementWeight = 1;
             float diagonalMovementWeight = (float) Math.Sqrt(2f);

             Vector2Int rightCoordinate = coordinate + Vector2Int.right;
             Vector2Int leftCoordinate = coordinate + Vector2Int.left;
             Vector2Int upCoordinate = coordinate + Vector2Int.up;
             Vector2Int downCoordinate = coordinate + Vector2Int.down;

             Vector2Int rightUpCoordinate = rightCoordinate + Vector2Int.up;
             Vector2Int rightDownCoordinate = rightCoordinate + Vector2Int.down;
             Vector2Int leftUpCoordinate = leftCoordinate + Vector2Int.up;
             Vector2Int leftDownCoordinate = leftCoordinate + Vector2Int.down;

             bool hasRightNode = rightCoordinate.x < m_Grid.Width && !m_Grid.GetNode(rightCoordinate).m_IsOccupied;
             bool hasLeftNode = leftCoordinate.x >= 0 && !m_Grid.GetNode(leftCoordinate).m_IsOccupied;
             bool hasUpNode = upCoordinate.y < m_Grid.Height && !m_Grid.GetNode(upCoordinate).m_IsOccupied;
             bool hasDownNode = downCoordinate.y >= 0 && !m_Grid.GetNode(downCoordinate).m_IsOccupied;

             if (hasRightNode)
             {
                 yield return new Connection(rightCoordinate, straightMovementWeight);

                 if (hasUpNode && !m_Grid.GetNode(rightUpCoordinate).m_IsOccupied)
                 {
                     yield return new Connection(rightUpCoordinate, diagonalMovementWeight);
                 }

                 if (hasDownNode && !m_Grid.GetNode(rightDownCoordinate).m_IsOccupied)
                 {
                     yield return new Connection(rightDownCoordinate, diagonalMovementWeight);
                 }
             }

             if (hasLeftNode)
             {
                 yield return new Connection(leftCoordinate, straightMovementWeight);

                 if (hasUpNode && !m_Grid.GetNode(leftUpCoordinate).m_IsOccupied)
                 {
                     yield return new Connection(leftUpCoordinate, diagonalMovementWeight);
                 }

                 if (hasDownNode && !m_Grid.GetNode(leftDownCoordinate).m_IsOccupied)
                 {
                     yield return new Connection(leftDownCoordinate, diagonalMovementWeight);
                 }
             }

             if (hasUpNode)
             {
                 yield return new Connection(upCoordinate, straightMovementWeight);
             }

             if (hasDownNode)
             {
                 yield return new Connection(downCoordinate, straightMovementWeight);
             }
         }
     }
 } 