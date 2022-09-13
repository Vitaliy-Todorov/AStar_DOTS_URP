using Assets.Scripts.FindingPath.Grid;
using Assets.Scripts.Services;
using Assets.Scripts.Services.InputServices;
using UnityEngine;
using Grid = Assets.Scripts.FindingPath.Grid.Grid;

namespace Assets.Scripts
{
    public class Test : MonoBehaviour
    {
        [SerializeField]
        private GameObject _prefabWall;

        private InputKeyboardMouseService _inputService;
        private Click _click;

        // private EndSimulationEntityCommandBufferSystem _endSimulationECDSystem;

        private Grid _grid;
        private float _scalleGrid = .5f;

        void Start()
        {
            _inputService = AllServices.Container.Single<InputKeyboardMouseService>();
            _click = _inputService.Click;

            // _endSimulationECDSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();

            // _grid = new Grid(8, 5, .5f, new Vector3(-4, -2, 0));
            _grid = new Grid(8, 5, _scalleGrid, new Vector3(0, 0, 0));
        }

        void Update()
        {
            if (_click.Up && _grid.PositionToGrid(_click.StaryPosition))
                SetIsWall(_click.StaryPosition);
        }

        public void SetIsWall(Vector3 positon)
        {
            if (_grid.IsWall(positon))
                _grid.DestroyWall(positon);
            else
                _grid.SetWall(positon, _prefabWall);
        }
    }
}