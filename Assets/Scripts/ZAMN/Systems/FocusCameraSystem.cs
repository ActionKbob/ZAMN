namespace ZAMN
{
    using UnityEngine;
    using Unity.Entities;
    using Unity.Mathematics;

    public class FocusCameraSystem : ComponentSystem
    {
        struct CameraFilter
        {
            public Camera Camera;
            public Position2D Position;
            public readonly FocusCamera Settings;
        }

        struct TargetFilter
        {
            public ComponentArray<FocusCameraTarget> Targets;
            public readonly ComponentArray<Position2D> Positions;
            public readonly int Length;   
        }

        [ Inject ]
        private TargetFilter targetFilter;

        protected override void OnUpdate()
        {
            float2 center = float2.zero;
            float furthestHorizontalDistance = 0;
            float furthestVerticalDistance = 0;

            for( int i = 0; i < targetFilter.Length; i++ )
            {
                center += targetFilter.Positions[i].Value;
            }

            center = center / targetFilter.Length;
            
            for( int i = 0; i < targetFilter.Length; i++ )
            {
                if( math.abs( targetFilter.Positions[i].Value.x ) > furthestHorizontalDistance )
                    furthestHorizontalDistance = math.abs( targetFilter.Positions[i].Value.x );
                
                if( math.abs( targetFilter.Positions[i].Value.y ) > furthestHorizontalDistance )
                    furthestVerticalDistance = math.abs( targetFilter.Positions[i].Value.y );
            }

            float deltaTime = Time.deltaTime;
            
            foreach( CameraFilter focusCamera in GetEntities<CameraFilter>() )
            {
                float positionX = math.lerp( focusCamera.Position.Value.x, center.x, focusCamera.Settings.MoveSpeed * deltaTime );
                float positionY = math.lerp( focusCamera.Position.Value.y, center.y, focusCamera.Settings.MoveSpeed * deltaTime );
                focusCamera.Position.Value = new float2( positionX, positionY );
                
                float distanceToSize = Mathf.Max( furthestVerticalDistance + 1, ( furthestHorizontalDistance + 1 ) / focusCamera.Camera.aspect );
                float cameraSize = ( distanceToSize > focusCamera.Settings.MaxSize ) ? focusCamera.Settings.MaxSize : ( distanceToSize < focusCamera.Settings.MinSize ) ? focusCamera.Settings.MinSize : distanceToSize;

                focusCamera.Camera.orthographicSize = Mathf.Lerp( focusCamera.Camera.orthographicSize, cameraSize, deltaTime * focusCamera.Settings.ZoomSpeed );
            }
        }
    }
}