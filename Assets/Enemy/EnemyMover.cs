using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour 
{
    [SerializeField] [Range(0f, 5f)] private float speed = 1f;
    
    private List<Node> path = new List<Node>();
    private Enemy enemy;

    private GridManager _gridManager;
    private Pathfinder _pathfinder;
    
    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        _gridManager = FindObjectOfType<GridManager>();
        _pathfinder = FindObjectOfType<Pathfinder>();
    }
    
    private void OnEnable() 
    {
        ReturnToStart();
        RecalculatePath(true);
    }

    void RecalculatePath(bool resetPath)
    {
        Vector2Int coordinates = new Vector2Int();

        if (resetPath)
        {
            coordinates = _pathfinder.StartCoordinates;
        }
        else
        {
            coordinates = _gridManager.GetCoordinatesFromPosition(transform.position);
        }
        
        StopAllCoroutines();
        path.Clear();
        path = _pathfinder.GetNewPath(coordinates);
        StartCoroutine(FollowPath());
    }

    void ReturnToStart()
    {
        transform.position = _gridManager.GetPositionFromCoordinates(_pathfinder.StartCoordinates);
    }

    private void FinishPath()
    {
        enemy.StealGold();
        gameObject.SetActive(false);
    }
    
    private IEnumerator FollowPath()
    {
        for (int i = 1; i < path.Count; i++) 
        {
            Vector3 startPosition = transform.position;
            Vector3 endPosition = _gridManager.GetPositionFromCoordinates(path[i].coordinates);
            float travelPercent = 0f;

            transform.LookAt(endPosition);

            while (travelPercent < 1f) 
            {
                travelPercent += Time.deltaTime * speed;
                transform.position = Vector3.Lerp(startPosition, endPosition, travelPercent);
                yield return new WaitForEndOfFrame();
            }
        }

        FinishPath();
    }
}
