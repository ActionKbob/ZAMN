namespace ZAMN
{
    using Unity.Entities;
    using Unity.Mathematics;

    public class MoveSystem : ComponentSystem
    {
        struct MoveFilter
        {
            public readonly ComponentArray<Move2D> Moves;
            public readonly int Length;
        }

        [ Inject ]
        private MoveFilter moveFilter;

        protected override void OnUpdate()
        {
            
        }
    }
}