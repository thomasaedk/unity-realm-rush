using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Serialization;

[ExecuteAlways]
[RequireComponent(typeof(TextMeshPro))]
public class CoordinateLabeler : MonoBehaviour 
{
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color blockedColor = new Color(0.65f, 0.65f, 0.65f, 1f);
    [SerializeField] private Color exploredColor = Color.yellow;
    [SerializeField] private Color pathhColor = new Color(1f, 0.5f, 0f, 1f);
    
    TextMeshPro label;
    Vector2Int coordinates;
    private GridManager gridManager; 

    void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        label = GetComponent<TextMeshPro>();
        label.enabled = false;
        
        DisplayCoordinates();
    }

    // Update is called once per frame
    void Update() 
    {
        if (!Application.isPlaying) 
        {
            DisplayCoordinates();
            UpdateObjectName();
            label.enabled = true;
        }
        SetLabelColor();
        if (Debug.isDebugBuild)
        {
            ToggleLabels();
        }
    }

    void ToggleLabels()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            label.enabled = !label.IsActive();
        }
    }

    private void SetLabelColor()
    {
        if (gridManager == null)
        {
            return;
        }
        Node node = gridManager.GetNode(coordinates);
        if (node == null)
        {
            return;
        }
        
        if (!node.isWalkable || node.isStartOrDestinationNode)
        {
            label.color = blockedColor;
        }
        else if (node.isPath)
        {
            label.color = pathhColor;
        }
        else if (node.isExplored)
        {
            label.color = exploredColor;
        }
        else
        {
            label.color = defaultColor;
        }
    }

    private void DisplayCoordinates() 
    {
        if (gridManager == null)
        {
            return;
        }
        coordinates.x = Mathf.RoundToInt(transform.parent.position.x / gridManager.UnityGridsize);
        coordinates.y = Mathf.RoundToInt(transform.parent.position.z / gridManager.UnityGridsize);

        label.text = $"{coordinates.x},{coordinates.y}";
    }

    private void UpdateObjectName() 
    {
        transform.parent.name = coordinates.ToString();
    }
}
