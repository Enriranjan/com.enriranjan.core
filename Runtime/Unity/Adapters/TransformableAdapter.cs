using EnriRanjan.Core.EngineInterfaces;
using UnityEngine;

namespace EnriRanjan.Core.Unity.Adapters
{
    /// <summary>
    /// <see cref="ITransformable"/> adapter wrapping a Unity Transform.
    /// </summary>
    public sealed class TransformableAdapter : ITransformable
    {
        private readonly Transform _transform;

        public TransformableAdapter(Transform transform)
        {
            _transform = transform;
        }

        public Vector3Data Position
        {
            get => ToData(_transform.position);
            set => _transform.position = ToVector3(value);
        }

        public QuaternionData Rotation
        {
            get => ToData(_transform.rotation);
            set => _transform.rotation = ToQuaternion(value);
        }

        public Vector3Data Scale
        {
            get => ToData(_transform.localScale);
            set => _transform.localScale = ToVector3(value);
        }

        private static Vector3Data ToData(Vector3 v) => new Vector3Data(v.x, v.y, v.z);

        private static Vector3 ToVector3(Vector3Data v) => new Vector3(v.X, v.Y, v.Z);

        private static QuaternionData ToData(Quaternion q) => new QuaternionData(q.x, q.y, q.z, q.w);

        private static Quaternion ToQuaternion(QuaternionData q) => new Quaternion(q.X, q.Y, q.Z, q.W);
    }
}
