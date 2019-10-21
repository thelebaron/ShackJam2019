using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace ShackJam
{
    public struct UserInput : IComponentData
    {
        public bool LeftClick;
        public bool RightClick;
        public float3 MousePosition;
    }
    
    public struct MenuState : IComponentData
    {
        public bool   Started;
        public bool   Escape;
        public float InputBlockTimer;
    }
    
    public struct PlayerInputData : IComponentData
    {
        public float3 CurrentMousePosition;
        public float3 ClickPosition;
    }
    
    public class MouseSystem : ComponentSystem
    {
        public EntityQuery MouseInputQuery;

        protected override void OnCreate()
        {
            base.OnCreate();
            MouseInputQuery = GetEntityQuery(typeof(UserInput));
        }

        protected override void OnUpdate()
        {
            Entities.With(MouseInputQuery).ForEach((Entity entity, ref UserInput input, ref PlayerInputData playerInputData, ref MenuState menuState) =>
                {
                    
                    
                    input.LeftClick = Input.GetKey(KeyCode.Mouse0);
                    input.RightClick = Input.GetKey(KeyCode.Mouse1);
                    input.MousePosition = Input.mousePosition;

                    if (Input.anyKey)
                        menuState.Started = true;
                    
                    if(!menuState.Started)
                        return;
                    
                    var camera = Camera.main;
                    
                    if (camera != null)
                    {
                        var ray = camera.ScreenPointToRay(input.MousePosition);

                        if (Physics.Raycast(ray, out var hit)) 
                        {
                            var objectHit = hit.transform;
                            playerInputData.CurrentMousePosition = hit.point;


                            var interactable = objectHit.GetComponent<Interactable>();
                            if (interactable != null)
                                interactable.transform.localScale = interactable.recordedScale * 1.2f;
                            //Debug.Log(objectHit);
                            // Do something with the object that was hit by the raycast.
                        }
                    }
                });
        }
    }

}
