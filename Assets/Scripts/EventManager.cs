using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Manager")]
public class EventManager : ScriptableObject
{
    public UnityEvent<Vector2> WindStarted;

    public UnityEvent LevelWon;
}