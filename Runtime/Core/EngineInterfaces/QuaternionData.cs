using System;

namespace EnriRanjan.Core.EngineInterfaces
{
    /// <summary>
    /// Engine-agnostic quaternion rotation used to cross the boundary between
    /// pure C# systems and engine adapters, without exposing
    /// UnityEngine.Quaternion to code under EnriRanjan.Core.
    /// </summary>
    [Serializable]
    public struct QuaternionData : IEquatable<QuaternionData>
    {
        public float X;
        public float Y;
        public float Z;
        public float W;

        public QuaternionData(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        /// <summary>Shorthand for the identity rotation (0, 0, 0, 1).</summary>
        public static readonly QuaternionData Identity = new QuaternionData(0f, 0f, 0f, 1f);

        public bool Equals(QuaternionData other) =>
            X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z) && W.Equals(other.W);

        public override bool Equals(object obj) => obj is QuaternionData other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 31 + X.GetHashCode();
                hash = hash * 31 + Y.GetHashCode();
                hash = hash * 31 + Z.GetHashCode();
                hash = hash * 31 + W.GetHashCode();
                return hash;
            }
        }

        public override string ToString() => $"({X}, {Y}, {Z}, {W})";
    }
}
