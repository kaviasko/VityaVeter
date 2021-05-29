using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : GridBehaviour
{
    [SerializeField] private EventManager eventmgr;
    [SerializeField] private Transform view;

    public void Start()
    {
        eventmgr.WindStarted.AddListener(OnWindStarted);
    }

    private void OnWindStarted(Vector2 v)
    {
        MovementDirection = v;
    }

    public override void OnCollidedBody(Vector2 collisionDirection, GridBehaviour other, bool isMyFault)
    {
        view.DOComplete();
        view.DOPunchPosition(collisionDirection / 2, 0.3f);
        view.DOPunchScale(collisionDirection * 0.5f, 0.3f);
    }

    public override void OnCollidedWall(Vector2 collisionDirection)
    {
        view.DOComplete();
        view.DOPunchPosition(collisionDirection / 2, 0.3f);
        view.DOPunchScale(collisionDirection * 0.5f, 0.3f);
    }

    public override void OnStepWalked()
    {
    }
}
