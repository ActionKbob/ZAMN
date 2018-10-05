namespace ZAMN
{
    using UnityEngine;
    using Unity.Entities;
    using Unity.Mathematics;

    public class PositionSyncSystem : ComponentSystem
    {
        struct PositionFilter
        {
            public readonly ComponentArray<Position2D> Positions;
            public readonly ComponentArray<Transform> Transforms;
            public readonly int Length;
        }

        [ Inject ]
        private PositionFilter positionFilter;

        protected override void OnUpdate()
        {
            for( int i = 0; i < positionFilter.Length; i++ )
            {
                float2 position = positionFilter.Positions[i].Value;
                Transform transform = positionFilter.Transforms[i];

                transform.position = new Vector3( position.x, position.y, transform.position.z );
            }
        }
    }
}