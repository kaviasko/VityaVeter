using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Trap : GridBehaviour
{
    [SerializeField] private SpriteRenderer image;
    [SerializeField] private Sprite occupiedSprite;
    public AudioManager audioManager;

    private bool isOccupied = false;

    public void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    public override void OnMoveStarted()
    {
        Priority = int.MaxValue;
        MovementDirection = Vector2.zero;
    }

    public override void OnCollidedBody(Vector2 collisionDirection, GridBehaviour other, bool isMyFault)
    {
        if (isOccupied) return;
        if (!other.IsTag("Enemy")) return;
        audioManager.Play("CatTrapped");
        isOccupied = true;
        image.sprite = occupiedSprite;
        other.Remove();
        transform.DOPunchScale(Vector3.one * 0.5f, 0.25f, 1);
        transform.DOJump(transform.position, 0.2f, 1, 0.25f);
    }

    public override void OnCollidedWall(Vector2 collisionDirection)
    {
    }

    public override void OnStepWalked()
    {
    }
}
