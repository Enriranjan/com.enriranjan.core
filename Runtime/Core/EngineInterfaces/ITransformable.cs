namespace EnriRanjan.Core.EngineInterfaces
{
    /// <summary>
    /// Engine-agnostic read/write access to a spatial transform (position,
    /// rotation, scale). Implemented in Unity by an adapter wrapping a
    /// Transform.
    /// </summary>
    public interface ITransformable
    {
        /// <summary>World-space position.</summary>
        Vector3Data Position { get; set; }

        /// <summary>World-space rotation.</summary>
        QuaternionData Rotation { get; set; }

        /// <summary>Local scale.</summary>
        Vector3Data Scale { get; set; }
    }
}
