using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [RequireComponent(typeof(Rigidbody))]
public class GridGenerator : MonoBehaviour
{
    [SerializeField] 
    private GameObject _gridCell;
    [SerializeField]// Prefab for individual grid cell
    private int _width = 5; 
    [SerializeField]// Number of cells horizontally
    private int _height = 5; // Number of cells vertically
    
    // private Rigidbody _rigidBody;
    void Awake()
    {
        // _rigidBody = GetComponent<Rigidbody>();
        // _rigidBody.constraints = RigidbodyConstraints.FreezePosition;
        // _rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
        
        // Loop through each cell position
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                // Instantiate a grid cell
                GameObject cell = Instantiate(_gridCell, transform);

                // Calculate position based on grid size and current cell index
                Vector3 position = new Vector3(x, 0, y);
    
                // Assign position to the cell
                cell.transform.position = position;
            }
        }
    }
}
