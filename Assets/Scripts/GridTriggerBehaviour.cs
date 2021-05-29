using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GridTriggerBehaviour : MonoBehaviour
{
    public Vector2 Position { get; set; }
    private GridManager grid = null;

    public abstract void OnBodyStepped(GridBehaviour other);

    public void Awake()
    {
        transform.position = Vector3Int.FloorToInt(transform.position);
        Position = transform.position;
        grid = FindObjectOfType<GridManager>();
    }

    public void OnEnable()
    {
        grid.AddThrower(this);
    }
}
