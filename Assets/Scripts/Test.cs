using Assets.Scripts.FindingPath.Grid;
using Assets.Scripts.Services;
using Assets.Scripts.Services.InputServices;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Grid = Assets.Scripts.FindingPath.Grid.Grid;

namespace Assets.Scripts
{
    //[AlwaysUpdateSystem]
    //[UpdateAfter(typeof(RegisterServices))]
    [DisableAutoCreation]
    public partial class Test : SystemBase
    {
        private InputKeyboardMouseService _inputService;
        private Click _click;
        private BlobAssetStore _blobAssetStore;
        private World _worldEntities;

        private EndFixedStepSimulationEntityCommandBufferSystem _endFixedStepECBSystem;

        private Grid _grid;
        private float _scalleGrid = 1.5f;

        private string _addressPrefabWall = "Wall";
        private GameObject _prefabWall;

        public void Constract()
        {
            base.OnCreate();

            _prefabWall = Resources.Load<GameObject>(_addressPrefabWall);

            _inputService = AllServices.Container.Single<InputKeyboardMouseService>();
            _click = _inputService.Click;

            _blobAssetStore = new BlobAssetStore();
            _worldEntities = World.DefaultGameObjectInjectionWorld;

             _endFixedStepECBSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<EndFixedStepSimulationEntityCommandBufferSystem>();

            _grid = new Grid(9, 5, _scalleGrid, new Vector3(-4, -2, 0) * _scalleGrid);
        }

        protected override void OnUpdate()
        {
            EntityCommandBuffer ecb = _endFixedStepECBSystem.CreateCommandBuffer();
            if (_click.Up && _grid.PositionToGrid(_click.StaryPosition))
                SetIsWall(_click.StaryPosition, ecb);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _blobAssetStore.Dispose();
        }

        public void SetIsWall(Vector3 positon, EntityCommandBuffer ecb)
        {
            if (_grid.IsWall(positon))
                _grid.DestroyWall(positon, ecb);
            else
            {
                Entity wall = GObjToEntity(_prefabWall);
                SetWall(positon, wall);
            }
        }

        private void SetWall(Vector3 positon, Entity wall)
        {
            Translation translationWall = GetComponent<Translation>(wall);
            CompositeScale localToWorldWall = new CompositeScale { Value = float4x4.Scale(_grid.CellSize) };
            EntityManager.AddComponentData(wall, localToWorldWall);

            _grid.SetWall(positon, wall, ref translationWall);

            SetComponent(wall, translationWall);
        }

        private Entity GObjToEntity(GameObject prefab)
        {
            GameObject GObj = Object.Instantiate(_prefabWall);

            GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(_worldEntities, _blobAssetStore);
            Entity entity = GameObjectConversionUtility.ConvertGameObjectHierarchy(GObj, settings);

            Object.Destroy(GObj);

            return entity;
        }
    }
}