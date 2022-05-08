using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Thrower : GridTriggerBehaviour
{
    public enum Orientation { Up, Down, Left, Right }

    public Orientation orientation;

    /*public override void OnCollidedBody(Vector2 collisionDirection, GridBehaviour other, bool isMyFault)
    {
        Vector2 throwDirection = Vector2.zero;
        switch (orientation)
        {
            case Orientation.Up:
                {
                    throwDirection = Vector2.up;
                    break;
                }
            case Orientation.Down:
                {
                    throwDirection = Vector2.down;
                    break;
                }
            case Orientation.Left:
                {
                    throwDirection = Vector2.left;
                    break;
                }
            case Orientation.Right:
                {
                    throwDirection = Vector2.right;
                    break;
                }
        }
        other.MovementDirection = throwDirection;
    }*/

    public override void OnBodyStepped(GridBehaviour other)
    {
        if (other.gameObject.CompareTag("Player")) {
            Vitya.isThrown = true;
        }
        Vector2 throwDirection = Vector2.zero;
        switch (orientation)
        {
            case Orientation.Up:
                {
                    throwDirection = Vector2.up;
                    break;
                }
            case Orientation.Down:
                {
                    throwDirection = Vector2.down;
                    break;
                }
            case Orientation.Left:
                {
                    throwDirection = Vector2.left;
                    break;
                }
            case Orientation.Right:
                {
                    throwDirection = Vector2.right;
                    break;
                }
        }
        other.MovementDirection = throwDirection;
        other.Priority++;
    }
}
