using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour 
{

    [SerializeField] private Tower towerPrefab;

    [SerializeField] private bool isPlaceable;
    public bool IsPlaceable => isPlaceable;

    private GridManager _gridManager;
    private Pathfinder _pathfinder;
    private Vector2Int coordinates = new Vector2Int();

    private void Awake()
    {
        _gridManager = FindObjectOfType<GridManager>();
        _pathfinder = FindObjectOfType<Pathfinder>();
    }

    private void Start()
    {
        if (_gridManager != null)
        {
            coordinates = _gridManager.GetCoordinatesFromPosition(transform.position);

            if (!isPlaceable)
            {
                _gridManager.BlockNode(coordinates);
            }
        }
    }

    private void OnMouseDown()
    {
        Node node = _gridManager.GetNode(coordinates);
        if (!node.isStartOrDestinationNode && node.isWalkable && !_pathfinder.WillBlockPath(coordinates))
        {
            bool isSuccessful = towerPrefab.CreateTower(towerPrefab, transform.position);
            if (isSuccessful)
            {
                _gridManager.BlockNode(coordinates);
                _pathfinder.NotifyReceivers();
            }
        }
    }
}
