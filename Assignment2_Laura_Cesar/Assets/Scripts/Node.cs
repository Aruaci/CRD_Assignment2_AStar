using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public bool isWalkable;
    public Vector3 position;

    public int gCost;
    public int hCost;
    public int fCost => gCost + hCost;

    public Node(bool _isWalkable, Vector3 _position)
    {
        isWalkable = _isWalkable;
        position = _position;
    }

    
}
