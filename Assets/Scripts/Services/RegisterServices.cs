using Assets.Scripts.Services.InputServices;
using System;
using Unity.Entities;
using UnityEngine;

namespace Assets.Scripts.Services
{
    //[AlwaysUpdateSystem]
    public partial class RegisterServices : SystemBase
    {
        private AllServices _containerServices = AllServices.Container;
        private World _world;

        protected override void OnCreate()
        {
            base.OnCreate();

            _world = World.DefaultGameObjectInjectionWorld;

            _containerServices.RegisterSingle(CreateSystemBase<InputKeyboardMouseService>());

            GridSystem test = CreateSystemBase<GridSystem>();
            test.Constract();
        }

        protected override void OnUpdate() { }

        private T CreateSystemBase<T>() where T : SystemBase
        {
            var systemGroup = _world.GetOrCreateSystem<SimulationSystemGroup>();
            var system = _world.GetOrCreateSystem<T>();
            systemGroup.AddSystemToUpdateList(system);
            systemGroup.SortSystems();

            return system;
        }
    }
}