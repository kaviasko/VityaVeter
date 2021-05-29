using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Kotik : GridBehaviour
{
    [SerializeField] private EventManager eventmgr;
    [Space]
    [SerializeField] private Transform view;
    [SerializeField] private SpriteRenderer spriterenderer;
    public AudioManager audioManager;

    public void Start()
    {
        eventmgr.WindStarted.AddListener(OnWindStarted);
        FindObjectOfType<WinCondition>().AddEnemy(this);
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void OnDisable()
    {
        var winCondition = FindObjectOfType<WinCondition>();
        if (winCondition != null) winCondition.EnemyDead(this);
    }

    private void OnWindStarted(Vector2 v)
    {
        MovementDirection = v;
    }

    public override void OnCollidedBody(Vector2 collisionDirection, GridBehaviour other, bool isMyFault)
    {
        if (!isMyFault) {
            audioManager.Play("Meow");
            return;
        }
        view.DOComplete();
        view.DOPunchPosition(collisionDirection / 2, 0.3f);
        view.DOPunchScale(collisionDirection * 0.5f, 0.3f);
    }

    public override void OnCollidedWall(Vector2 collisionDirection)
    {
        audioManager.Play("Wall");
        view.DOComplete();
        view.DOPunchPosition(collisionDirection / 2, 0.3f);
        view.DOPunchScale(collisionDirection * 0.5f, 0.3f);
    }

    override public void OnStepWalked()
    {
        spriterenderer.flipX = MovementDirection.x > 0;
    }
}
