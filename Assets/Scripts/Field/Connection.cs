using UnityEngine;

namespace Field
{
    public struct Connection
    {
        public Vector2Int Coordinate;
        public float Weight;

        public Connection(Vector2Int coordinate, float weight)
        {
            this.Coordinate = coordinate;
            this.Weight = weight;
        }
    }
}