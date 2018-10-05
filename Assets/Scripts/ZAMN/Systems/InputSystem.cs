namespace ZAMN
{
    using Unity.Entities;
    using Unity.Mathematics;
    
    public class InputSystem : ComponentSystem
    {
        struct InputFilter
        {
            public ComponentArray<PlayerInput> Inputs;
            public readonly ComponentArray<Player> Players;
            public readonly int Length;
        }

        [ Inject ]
        private InputFilter inputFilter;

        protected override void OnUpdate()
        {

            Rewired.Player[] rewiredPlayers = GameManager.Instance.RewiredPlayers;

            for( int i = 0; i < inputFilter.Length; i++ )
            {
                PlayerInput input = inputFilter.Inputs[i];
                Player player = inputFilter.Players[i];
                Rewired.Player rewiredPlayer = rewiredPlayers[ player.Id ];

                float hMove = rewiredPlayer.GetAxis( "Move Horizontally" );
                float vMove = rewiredPlayer.GetAxis( "Move Vertically" );
                
                input.Movement = new float2( hMove, vMove );
            }
        }
    }
}