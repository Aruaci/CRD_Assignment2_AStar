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
    [SerializeField] 
    private Animator _animator;

    private Node[,] _gridNodes;
    private float _raycastDistance = 50f;
    private int[,] _obstacleMatrix;

    private int _startX;
    private int _startY;
    private int _goalX;
    private int _goalY;

    private List<Node> ShortestPath;
    private List<Transform> ShortestPathTransforms;
    
    Coroutine MoveIE;
    
    [SerializeField]
    private float _moveSpeed = 8;
    [SerializeField]
    private float _rotationDuration = 2;
    [SerializeField]
    private float _moveStartDelay = 0.0f;

    void Start()
    {
        _gridNodes = new Node[_width, _height];
        PopulateGridOfNodes();
        FindShortestPath(_player, _goal);
        ColorPathTiles(ShortestPath);
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

    private void FindShortestPath(GameObject player, GameObject goal)
    {
        Node startNode = GameObjectToNode(player);
        Node targetNode = GameObjectToNode(goal);

        List<Node> openTiles = new List<Node>();
        List<Node> closedTiles = new List<Node>();

        openTiles.Add(startNode);

        while (openTiles.Count > 0)
        {
            Node currentNode = openTiles[0];
            for (int i = 1; i < openTiles.Count; i++)
            {
                if (openTiles[i].fCost < currentNode.fCost || 
                    openTiles[i].fCost == currentNode.fCost)
                {
                    if (openTiles[i].hCost < currentNode.hCost)
                    {
                        currentNode = openTiles[i];
                    }
                }
            }

            openTiles.Remove(currentNode);
            closedTiles.Add(currentNode);

            if (currentNode.gridX == targetNode.gridX && currentNode.gridY == targetNode.gridY)
            {

                RetraceFoundPath(startNode, currentNode);
            }
            
            foreach (Node neighbour in GetNodeNeighbours(currentNode))
            {
                if (!neighbour.isWalkable || closedTiles.Contains(neighbour)) continue;

                int newMovementCostToNeighbour = currentNode.gCost + GetDistanceOf(currentNode, neighbour);
                if (newMovementCostToNeighbour < neighbour.gCost || !openTiles.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistanceOf(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openTiles.Contains(neighbour)) openTiles.Add(neighbour);
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
        Node newNode = new Node(false, position, (int)x, (int)z);

        return newNode;
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

    private void RetraceFoundPath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode.gridX != startNode.gridX || currentNode.gridY != startNode.gridY)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        ShortestPath = new List<Node>();
        for (int i = path.Count - 1; i >= 0; i--)
        {
            ShortestPath.Add(path[i]);
        }
    }

    private void ColorPathTiles(List<Node> pathNodes)
    {
        StartCoroutine(ColorPathTilesWithDelay(pathNodes));
    }

    private IEnumerator ColorPathTilesWithDelay(List<Node> pathNodes)
    {
        ShortestPathTransforms = new List<Transform>();
        foreach (var node in pathNodes)
        {
            foreach (Transform cell in transform.GetComponentInChildren<Transform>())
            {
                if (cell.transform.position.x == node.gridX && cell.transform.position.z == node.gridY)
                {
                    yield return new WaitForSeconds(0.5f);
                    cell.GetComponent<TileInfo>()._hasPath = true;
                    CreatePathTransformList(cell);
                }
            }
        }
        
        StartMovingCoroutineWithDelay();
    }

    void CreatePathTransformList(Transform cell)
    {
        ShortestPathTransforms.Add(cell);
    }
    
    void StartMovingCoroutineWithDelay()
    {
        StartCoroutine(moveOnPath(_moveStartDelay));
    }
    IEnumerator moveOnPath(float delay)
    {
        yield return new WaitForSeconds(delay);
        for (int i = 0; i < ShortestPathTransforms.Count; i++)
        {
            MoveIE = StartCoroutine(Moving(i));
            yield return MoveIE;
        }
    }
    
    IEnumerator Moving(int currentPosition)
    {
        Vector3 targetPosition = new Vector3(ShortestPathTransforms[currentPosition].position.x, _player.transform.position.y, ShortestPathTransforms[currentPosition].position.z);
        Quaternion startRotation = _player.transform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(targetPosition - _player.transform.position);

        float t = 0f;
        
        while (_player.transform.position != targetPosition)
        {
            while (t < _rotationDuration && Quaternion.Angle(startRotation, targetRotation) > 0.01f)
            {
                _animator.SetBool("isIdle", true);
                t += Time.deltaTime;
                float normalizedTime = t / _rotationDuration;
                float easedTime = EaseInOutBack(normalizedTime);
                _player.transform.rotation = new Quaternion(
                    startRotation.x * (1 - easedTime) + targetRotation.x * easedTime,
                    startRotation.y * (1 - easedTime) + targetRotation.y * easedTime,
                    startRotation.z * (1 - easedTime) + targetRotation.z * easedTime,
                    startRotation.w * (1 - easedTime) + targetRotation.w * easedTime
                );
                yield return null;
            }

            _animator.SetBool("isIdle", false);
            _player.transform.position = Vector3.MoveTowards(_player.transform.position, targetPosition, _moveSpeed * Time.deltaTime);
            yield return null;
        }
        _animator.SetBool("isIdle", true);
    }

    private float EaseInOutBack(float x)
    {
        float c1 = 1.70158f;
        float c2 = c1 * 1.525f;

        return x < 0.5f
            ? (Mathf.Pow(2f * x, 2f) * ((c2 + 1f) * 2f * x - c2)) / 2f
            : (Mathf.Pow(2f * x - 2f, 2f) * ((c2 + 1f) * (x * 2f - 2f) + c2) + 2f) / 2f;
    }
}
