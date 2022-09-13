﻿using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts.FindingPath.Grid
{
    public class Grid 
    {
        private PathNode[,] _gridArray;
        private Vector3 _originPosition;
        private float _cellSize;

        public Grid(int width, int height, float cellSize, Vector3 originPosition)
        {
            _gridArray = new PathNode[width, height];
            _originPosition = originPosition;
            _cellSize = cellSize;

            for (int x = 0; x < _gridArray.GetLength(0); x++)
                for (int y = 0; y < _gridArray.GetLength(1); y++)
                {
                    int2 currentPosition = new int2(x, y);

                    Cost costNode = new Cost();
                    /*{
                        G = int.MaxValue,
                        H = CalculatedDistanceCost(currentPosition, endPosition)
                    };*/

                    _gridArray[x, y] = new PathNode
                    {
                        Index = CalculatedIndex(currentPosition),
                        Position = currentPosition,
                        Cost = costNode,

                        IsWalkable = true,
                        CameFromNodeIndex = -1 //
                    };
                }
            
            for (int x = 0; x <= _gridArray.GetLength(0); x++)
                Debug.DrawLine(GridCorners(x, 0), GridCorners(x, _gridArray.GetLength(1)), Color.green, 100);

            for (int y = 0; y <= _gridArray.GetLength(1); y++)
                Debug.DrawLine(GridCorners(0, y), GridCorners(_gridArray.GetLength(0), y), Color.green, 100);
        }

        public int CalculatedIndex(int2 currentPosition)
        {
            int max = math.max(_gridArray.GetLength(0), _gridArray.GetLength(1));
            return currentPosition.x + currentPosition.y * max;
        }

        public bool PositionToGrid(Vector3 positon)
        {
            int2 positionInGrid = PositionInGrid(positon);

            if (positionInGrid.x < _gridArray.GetLength(0) &&
                positionInGrid.x >= 0 &&
                positionInGrid.y < _gridArray.GetLength(1) &&
                positionInGrid.y >= 0)
                return true;
            else
                return false;
        }

        public void SetIsWall(bool isWall, Vector3 positon)
        {
            int2 positionInGrid = PositionInGrid(positon);
            _gridArray[positionInGrid.x, positionInGrid.y].IsWall = isWall;
        }

        public bool IsWall(bool isWall, Vector3 positon)
        {
            int2 positionInGrid = PositionInGrid(positon);
            return _gridArray[positionInGrid.x, positionInGrid.y].IsWall;
        }

        public void SetWall(Vector3 positon, GameObject prefabWall)
        {
            int2 positionInGrid = PositionInGrid(positon);
            Vector3 positionInGridWorld = GetWorldPosition(positionInGrid.x, positionInGrid.y);

            GameObject wall = UnityEngine.Object.Instantiate(prefabWall, positionInGridWorld, Quaternion.identity);
            wall.transform.localScale = new Vector3(_cellSize, _cellSize, 0);

            _gridArray[positionInGrid.x, positionInGrid.y].IsWall = true;
            _gridArray[positionInGrid.x, positionInGrid.y].WallGO = wall;
        }

        public void DestroyWall(Vector3 positon)
        {
            int2 positionInGrid = PositionInGrid(positon);
            _gridArray[positionInGrid.x, positionInGrid.y].IsWall = false;
            UnityEngine.Object.Destroy(_gridArray[positionInGrid.x, positionInGrid.y].WallGO);
        }

        public Vector3 PositionInGridWorld(Vector3 positionInWorld)
        {
            int2 positionInGridInt = PositionInGrid(positionInWorld);
            return GetWorldPosition(positionInGridInt.x, positionInGridInt.y);
        }

        public bool IsWall(Vector3 positionInWorld)
        {
            int2 positionInGrid = PositionInGrid(positionInWorld);

            return PositionToGrid(positionInWorld) && _gridArray[positionInGrid.x, positionInGrid.y].IsWall;
        }

        private Vector3 GridCorners(int x, int y)
        {
            return GetWorldPosition(x, y) - new Vector3(1, 1, 0) * .5f * _cellSize;
        }

        private Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector3(x, y, 0) * _cellSize + _originPosition;
        }

        private int2 PositionInGrid(Vector3 positionInWorld)
        {
            Vector3 positionInGrid = (positionInWorld - _originPosition) / _cellSize;
            int x = Mathf.RoundToInt(positionInGrid.x);
            int y = Mathf.RoundToInt(positionInGrid.y);

            return new int2(x, y);
        }
    }
}