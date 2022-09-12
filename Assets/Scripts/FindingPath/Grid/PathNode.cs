using Unity.Mathematics;

namespace Assets.Scripts.FindingPath.Grid
{
    public struct PathNode
    {
        public int Index;
        public int2 Position;

        public bool IsWalkable;
        public int CameFromNodeIndex;

        public Cost Cost;
    }
}