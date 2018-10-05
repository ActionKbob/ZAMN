namespace ZAMN
{
    using UnityEngine;
    using Rewired;

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance{ get; private set; }

        public Rewired.Player[] RewiredPlayers;

        void Awake()
        {
            if( Instance == null )
                Instance = this;
            else if( Instance != this )
                Destroy( gameObject );

            CollectRewiredPlayers();
        }

        private void CollectRewiredPlayers()
        {
            RewiredPlayers = new Rewired.Player[ ReInput.players.playerCount ];
            for( int i = 0; i < RewiredPlayers.Length; i++ )
            {
                RewiredPlayers[ i ] = ReInput.players.GetPlayer( i );
            }
        }
    }
}