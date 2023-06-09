using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class TileInfo : MonoBehaviour
{
    public bool _hasObstacle = false;
    private bool _hasPlayer = false;
    private bool _hasGoal = false;
    public bool _hasPath = false;
    
    [SerializeField]
    private Material tileBaseMaterial;
    [SerializeField]
    private Material obstacleMaterial;
    [SerializeField]
    private Material playerMaterial;
    [SerializeField]
    private Material goalMaterial;

    private MeshRenderer meshRenderer;

    private float _raycastDistance = 50f;
    private Vector3 _elevated;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        
        DetectObstacle();
        UpdateMaterial();
    }

    private void Update()
    {
        UpdateMaterial();
    }

    private void DetectObstacle()
    {
        Ray ray = new Ray(transform.position, transform.up);
        RaycastHit hit;
        
        
        if (Physics.Raycast(ray, out hit, _raycastDistance))
        {
            // Collision occurred, information about hit object can be accessed
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject.CompareTag("Obstacle"))
            {
                _hasObstacle = true;
                return;
                // Debug.DrawRay(ray.origin, ray.direction, Color.red);
            } 
            if (hitObject.CompareTag("Player"))
            {
                _hasPlayer = true;
                return;
                // Debug.DrawRay(ray.origin, ray.direction, Color.blue);
            }
            if (hitObject.CompareTag("Goal"))
            {
                _hasGoal = true;
                return;
                // Debug.DrawRay(ray.origin, ray.direction, Color.green);
            }
        }
        
        _hasObstacle = false;
        _hasPlayer = false;
        _hasGoal = false;
    }

    private void UpdateMaterial()
    {
        if (_hasGoal)
        {
            meshRenderer.material = goalMaterial;
        }
        else if (_hasPlayer || _hasPath)
        {
            meshRenderer.material = playerMaterial;
        }
        else if (_hasObstacle)
        {
            meshRenderer.material = obstacleMaterial;
        }
        else
        {
            meshRenderer.material = tileBaseMaterial;
        }
    }
}
