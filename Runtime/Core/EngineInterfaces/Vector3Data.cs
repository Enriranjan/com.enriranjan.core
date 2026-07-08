using System;

namespace EnriRanjan.Core.EngineInterfaces
{
    /// <summary>
    /// Engine-agnostic 3D vector used to cross the boundary between pure C#
    /// systems and engine adapters, without exposing UnityEngine.Vector3 to
    /// code under EnriRanjan.Core.
    /// </summary>
    [Serializable]
    public struct Vector3Data : IEquatable<Vector3Data>
    {
        public float X;
        public float Y;
        public float Z;

        public Vector3Data(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>Shorthand for (0, 0, 0).</summary>
        public static readonly Vector3Data Zero = new Vector3Data(0f, 0f, 0f);

        public bool Equals(Vector3Data other) =>
            X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);

        public override bool Equals(object obj) => obj is Vector3Data other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 31 + X.GetHashCode();
                hash = hash * 31 + Y.GetHashCode();
                hash = hash * 31 + Z.GetHashCode();
                return hash;
            }
        }

        public override string ToString() => $"({X}, {Y}, {Z})";
    }
}
