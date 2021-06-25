using Unity.Entities;

namespace Samples.NetCube
{
    [GenerateAuthoringComponent]
    public struct PatrolComponent : IComponentData
    {
        public bool MovingRight;
        public float Speed;
        public float PosLimit;
    }
}
