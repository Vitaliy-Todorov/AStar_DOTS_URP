using Unity.Entities;
using Unity.Mathematics;

namespace Assets.Scripts.FindingPath
{
    public struct PathFindingComponent : IComponentData
    {
        public int2 StartPosition;
        public int2 EndPosition;
    }
}
