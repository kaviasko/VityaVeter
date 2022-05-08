using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class GridBehaviour : MonoBehaviour
{
    public Vector2 Position { get; set; }
    public Vector2 MovementDirection { get; set; }
    public int Priority { get; set; }
    public bool Removed => removed;

    public abstract void OnCollidedWall(Vector2 collisionDirection);
    public abstract void OnCollidedBody(Vector2 collisionDirection, GridBehaviour other, bool isMyFault);
    public abstract void OnStepWalked();

    private GridManager grid = null;
    private bool removed = false;

    public virtual void GoTo(Vector2 newPosition, float time)
    {
        transform.DOMove(newPosition, time);
    }

    public virtual void Remove()
    {
        removed = true;
        Destroy(gameObject);
    }

    public bool IsTag(string tag)
    {
        return gameObject.CompareTag(tag);
    }

    public void Awake()
    {
        transform.position = Vector3Int.FloorToInt(transform.position);
        Position = transform.position;
        grid = FindObjectOfType<GridManager>();
    }

    public void OnEnable()
    {
        grid.AddBody(this);
    }

    // public void OnDisable()
    public void OnDestroy()
    {
        grid.RemoveBody(this);
    }
}
