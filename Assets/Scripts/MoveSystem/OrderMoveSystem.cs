using Assets.Scripts.FindingPath;
using Assets.Scripts.Services;
using Assets.Scripts.Services.InputServices;
using System.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Assets.Scripts.MoveSystem
{
    public partial class OrderMoveSystem : SystemBase
    {
        private InputKeyboardMouseService _inputService;
        private Click _click;

        private EndSimulationEntityCommandBufferSystem _endSimulationECDSystem;

        protected override void OnStartRunning()
        {
            base.OnStartRunning();
            _inputService = AllServices.Container.Single<InputKeyboardMouseService>();
            _click = _inputService.Click;

            _endSimulationECDSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            if (_click.Up)
            {
                var ecbEndSimulation = _endSimulationECDSystem.CreateCommandBuffer();

                int2 endPosition = new int2( (int) _click.EndPosition.x, (int)_click.EndPosition.y);

                Entities.ForEach((Entity entity, ref Translation translation) =>
                {
                    PathFindingComponent pathFindingComponent = new PathFindingComponent
                    {
                        StartPosition = new int2((int)translation.Value.x, (int)translation.Value.y),
                        EndPosition = endPosition,

                    };

                    ecbEndSimulation.AddComponent(entity, pathFindingComponent);
                }).Run();
            }
        }
    }
}