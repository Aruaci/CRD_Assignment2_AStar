using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class AStar : MonoBehaviour
{
    [SerializeField]
    private int _width; // Number of cells horizontally
    [SerializeField]
    public int _height;
    [SerializeField] 
    private GameObject _player;
    [SerializeField] 
    private GameObject _goal;
    
    private GameObject[,] _gridCells;
    private float _raycastDistance = 50f;
    private int[,] _obstacleMatrix;

    private int _startX;
    private int _startY;
    private int _goalX;
    private int _goalY;

    void Start()
    {
        _gridCells = new GameObject[_width, _height];
        _obstacleMatrix = new int[_width, _height];
        GetGrid();
        GetObstacleMatrix();
        _startX = Convert.ToInt32(_player.transform.position.x);
        _startY = Convert.ToInt32(_player.transform.position.z);
        _goalX = Convert.ToInt32(_goal.transform.position.x);
        _goalY  = Convert.ToInt32(_goal.transform.position.z);
        FindShortestPath(_player, _goal);

    }

    private void GetGrid()
    {
        foreach (Transform cell in transform.GetComponentInChildren<Transform>())
        {
            float _x = cell.position.x;
            float _z = cell.position.z;
            int xIndex = Mathf.FloorToInt(_x);
            int zIndex = Mathf.FloorToInt(_z);
            _gridCells[xIndex, zIndex] = cell.gameObject;
        }
    }

    private void GetObstacleMatrix()
    {
        foreach (Transform cell in transform.GetComponentInChildren<Transform>())
        {
            float _x = cell.position.x;
            float _z = cell.position.z;
            int xIndex = Mathf.FloorToInt(_x);
            int zIndex = Mathf.FloorToInt(_z);

            //if (cell.gameObject.GetComponent<TileInfo>()._hasObstacle)
            if (DetectObstacle(cell))
            {
                _obstacleMatrix[xIndex, zIndex] = 1;
            }
            else
            {
                _obstacleMatrix[xIndex, zIndex] = 0;
            }
        }
    }
    
    private bool DetectObstacle(Transform cell)
    {
        Ray ray = new Ray(cell.position, transform.up);
        RaycastHit hit;
        
        // _hasObstacle = false;
        
        if (Physics.Raycast(ray, out hit, _raycastDistance))
        {
            // Collision occurred, information about hit object can be accessed
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject.CompareTag("Obstacle"))
            {
                print(hitObject.name);
                return true;
                // Debug.DrawRay(ray.origin, ray.direction, Color.red);
            }
        }
        return false;
    }

    void FindShortestPath(GameObject player, GameObject goal)
    {
        Node startNode = GameObjectToNode(player);
        Node targetNode = GameObjectToNode(goal);
        
        print(startNode.position);
        print(targetNode.position);
        
        List<Node> openTiles = new List<Node>();
        List<Node> closedTiles = new List<Node>();

        while (openTiles.Count > 0)
        {
            Node currentNiode = openTiles[0];
        }
    }

    private Node GameObjectToNode(GameObject _object)
    {
        return new Node(false, _object.transform.position);
    }
}
