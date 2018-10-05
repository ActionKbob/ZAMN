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
            public Transform Transform;
            public readonly FocusCamera Settings;
        }

        struct TargetFilter
        {
            public ComponentArray<FocusCameraTarget> Targets;
            public readonly ComponentArray<Transform> Transforms;
            public readonly int Length;   
        }

        [ Inject ]
        private TargetFilter targetFilter;

        protected override void OnUpdate()
        {
            Vector3 center = Vector3.zero;
            float furthestHorizontalDistance = 0;
            float furthestVerticalDistance = 0;

            for( int i = 0; i < targetFilter.Length; i++ )
            {
                center += targetFilter.Transforms[i].position;
            }

            center = center / targetFilter.Length;
            
            for( int i = 0; i < targetFilter.Length; i++ )
            {
                if( Mathf.Abs( targetFilter.Transforms[i].position.x ) > furthestHorizontalDistance )
                    furthestHorizontalDistance = Mathf.Abs( targetFilter.Transforms[i].position.x );
                
                if( Mathf.Abs( targetFilter.Transforms[i].position.y ) > furthestHorizontalDistance )
                    furthestVerticalDistance = Mathf.Abs( targetFilter.Transforms[i].position.y );
            }

            float deltaTime = Time.deltaTime;

            Debug.Log( string.Format( "hor: {0}, vert: {1}", furthestVerticalDistance, furthestHorizontalDistance / Camera.main.aspect ) );

            foreach( CameraFilter focusCamera in GetEntities<CameraFilter>() )
            {
                focusCamera.Transform.position = Vector3.MoveTowards( focusCamera.Transform.position, new Vector3( center.x, center.y, focusCamera.Transform.position.z ), deltaTime * focusCamera.Settings.Speed );
                
                float distanceToSize = Mathf.Max( furthestVerticalDistance + 1, ( furthestHorizontalDistance + 1 ) / focusCamera.Camera.aspect );
                float cameraSize = ( distanceToSize > focusCamera.Settings.MaxSize ) ? focusCamera.Settings.MaxSize : ( distanceToSize < focusCamera.Settings.MinSize ) ? focusCamera.Settings.MinSize : distanceToSize;

                focusCamera.Camera.orthographicSize = Mathf.Lerp( focusCamera.Camera.orthographicSize, cameraSize, deltaTime );
            }
        }
    }
}