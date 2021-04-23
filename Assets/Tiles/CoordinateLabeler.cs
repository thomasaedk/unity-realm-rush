using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[ExecuteAlways]
[RequireComponent(typeof(TextMeshPro))]
public class CoordinateLabeler : MonoBehaviour 
{
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color blockedColor = new Color(0.65f, 0.65f, 0.65f, 1f);
    
    TextMeshPro label;
    Vector2Int coordinates;
    Waypoint waypoint; 

    void Awake() 
    {
        label = GetComponent<TextMeshPro>();
        label.enabled = false;
        
        waypoint = GetComponentInParent<Waypoint>();
        DisplayCoordinates();
    }

    // Update is called once per frame
    void Update() 
    {
        if (!Application.isPlaying) 
        {
            DisplayCoordinates();
            UpdateObjectName();
        }
        SetLabelColor();
        ToggleLabels();
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
        if (waypoint.IsPlaceable)
        {
            label.color = defaultColor;            
        }
        else
        {
            label.color = blockedColor;
        }
    }

    private void DisplayCoordinates() 
    {
        coordinates.x = Mathf.RoundToInt(transform.parent.position.x / UnityEditor.EditorSnapSettings.move.x);
        coordinates.y = Mathf.RoundToInt(transform.parent.position.z / UnityEditor.EditorSnapSettings.move.z);

        label.text = $"{coordinates.x},{coordinates.y}";
    }

    private void UpdateObjectName() 
    {
        transform.parent.name = coordinates.ToString();
    }
}
