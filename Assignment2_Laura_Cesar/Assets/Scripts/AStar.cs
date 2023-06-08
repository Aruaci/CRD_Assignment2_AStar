using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

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
    
    private Node[,] _gridNodes;
    private float _raycastDistance = 50f;
    private int[,] _obstacleMatrix;

    private int _startX;
    private int _startY;
    private int _goalX;
    private int _goalY;

    void Start()
    {
        _gridNodes = new Node[_width, _height];
        PopulateGridOfNodes();
        List<Node> N = GetNodeNeighbours((GameObjectToNode(_player)));
        print(GetDistanceOf(GameObjectToNode(_player),GameObjectToNode(_goal)));
        // FindShortestPath(_player, _goal);

    }

    private void PopulateGridOfNodes()
    {
        foreach (Transform cell in transform.GetComponentInChildren<Transform>())
        {
            Vector3 originalPosition = cell.transform.position;
            Node node = new Node(!DetectObstacle(cell), originalPosition, (int)originalPosition.x, (int)originalPosition.z);
            
            _gridNodes[(int)originalPosition.x, (int)originalPosition.z] = node;
        }
    }

    private bool DetectObstacle(Transform cell)
    {
        Ray ray = new Ray(cell.position, transform.up);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, _raycastDistance))
        {
            // Collision occurred, information about hit object can be accessed
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject.CompareTag("Obstacle"))
            {
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

        List<Node> openTiles = new List<Node>();
        List<Node> closedTiles = new List<Node>();

        while (openTiles.Count > 0)
        {
            Node currentNode = openTiles[0];
            for (int i = 1; i < openTiles.Count; i++)
            {
                if (openTiles[i].fCost < currentNode.fCost || 
                    openTiles[i].fCost == currentNode.fCost && 
                    openTiles[i].hCost < currentNode.hCost)
                {
                    currentNode = openTiles[i];
                }
            }

            openTiles.Remove(currentNode);
            closedTiles.Add(currentNode);

            if (currentNode == targetNode) {}
            {
                print("Target found: " +  currentNode.position);
                return;
            }

            foreach (Node neighbour in GetNodeNeighbours(currentNode))
            {
                if (!neighbour.isWalkable || closedTiles.Contains(neighbour))
                {
                    continue;
                }
            }
        }
    }

    private Node GameObjectToNode(GameObject _object)
    {
        Vector3 originalPosition = _object.transform.position;
        
        float x = Mathf.RoundToInt(originalPosition.x);
        float z = Mathf.RoundToInt(originalPosition.z);

        Vector3 position = new Vector3(x, 0, z);
        return new Node(false, position, (int)x, (int)z);
    }

    private List<Node> GetNodeNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                // NOTE: not sure if _width is right
                if (checkX >= 0 && checkX < _width && checkY >= 0 && checkY < _height)
                {
                    neighbours.Add(_gridNodes[checkX, checkY]);
                }
            }
        }
        return neighbours;
    }

    private int GetDistanceOf(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (distX > distY) return 14 * distY + 10 * (distX - distY);
        return 14 * distX + 10 * (distY - distX);
    }
}
