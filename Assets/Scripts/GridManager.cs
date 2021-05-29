using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    private List<GridBehaviour> bodies = new List<GridBehaviour>();
    private List<GridTriggerBehaviour> throwers = new List<GridTriggerBehaviour>();
    [SerializeField] Tilemap tmap;
    [SerializeField] float waitingTime;

    public void AddBody(GridBehaviour body)
    {
        Debug.Log($"Added {body}");
        bodies.Add(body);
    }

    public void AddThrower(GridTriggerBehaviour thrower)
    {
        //Debug.Log($"Added {body}");
        throwers.Add(thrower);
    }

    public void RemoveBody(GridBehaviour body)
    {
        Debug.Log($"Removed {body}");
        bodies.Remove(body);
    }

    public bool IsWall(Vector2 pos)
    {
        Vector3Int position = tmap.WorldToCell(pos);
        TileBase tile = tmap.GetTile(position);
        return tile != null;
    }

    private IEnumerator MoveOneStep(List<GridBehaviour> bodiesToMove, List<GridTriggerBehaviour> throwers)
    {
        foreach (GridBehaviour body in bodiesToMove)
        {
            Vector2 destination = body.Position + body.MovementDirection.normalized;
            if (IsWall(destination))
            {
                body.MovementDirection = Vector2.zero;
                body.OnCollidedWall(destination - body.Position);
                continue;
            }
        }
        CheckBodyToBody(bodiesToMove);
        bodiesToMove.RemoveAll(body => body.MovementDirection == Vector2.zero);
        foreach (GridBehaviour body in bodiesToMove)
        {
            body.GoTo(body.Position + body.MovementDirection.normalized, waitingTime);
        }
        yield return new WaitForSeconds(waitingTime);
        foreach (GridBehaviour body in bodiesToMove)
        {
            body.Position += body.MovementDirection.normalized;
            body.OnStepWalked();
        }
        foreach (GridBehaviour body in bodiesToMove)
        {
            foreach (GridTriggerBehaviour thrower in throwers) {
                if (body.Position == thrower.Position) thrower.OnBodyStepped(body);
            }
            
        }
    }

    public IEnumerator PlayUntilEveryoneHalt()
    {
        List<GridBehaviour> copy = new List<GridBehaviour>(bodies);
        List<GridTriggerBehaviour> copyT = new List<GridTriggerBehaviour>(throwers);

        while (copy.Count != 0)
        {
            yield return MoveOneStep(copy, copyT);
        }
    }

    void CheckBodyToBody(List<GridBehaviour> bodiesToMove)
    {
        Dictionary<Vector2, GridBehaviour> bodiesMap = new Dictionary<Vector2, GridBehaviour>();
        Dictionary<GridBehaviour, bool> isMovable = new Dictionary<GridBehaviour, bool>();

        foreach (GridBehaviour body in bodies)
        {
            bodiesMap.Add(body.Position, body);
        }

        bool CheckBody(GridBehaviour body)
        {
            if (!isMovable.ContainsKey(body))
            {
                isMovable.Add(body, false);
                if (body.MovementDirection == Vector2.zero)
                {
                    isMovable[body] = false;
                }
                else
                {
                    Vector2 destination = body.Position + body.MovementDirection.normalized;
                    if (bodiesMap.ContainsKey(destination))
                    {
                        GridBehaviour next = bodiesMap[destination];
                        bool isBlocking = CheckBody(next);
                        if (!isBlocking)
                        {
                            Vector2 direction = body.MovementDirection;
                            body.MovementDirection = Vector2.zero;
                            body.OnCollidedBody(direction, next, true);
                            next.OnCollidedBody(-direction, body, false);
                            if (body.Removed) isBlocking = true;
                            if (next.Removed) isBlocking = true;
                        }
                        isMovable[body] = isBlocking;
                    }
                    else
                    {
                        isMovable[body] = true;
                    }
                }
            }
            return isMovable[body];
        }

        foreach (GridBehaviour body in bodiesToMove)
        {
            CheckBody(body);
        }
    }
}
