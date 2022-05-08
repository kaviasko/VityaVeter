using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class VityaTrap : GridBehaviour
{
    [SerializeField] private SpriteRenderer image;
    [SerializeField] private Sprite occupiedVityaTrap;
    public AudioManager audioManager = null;

    private void Start()
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
        if (!other.IsTag("Player")) return;
        StartCoroutine(TrapCoroutine());

        IEnumerator TrapCoroutine()
        {
            audioManager.Play("Death");
            image.sprite = occupiedVityaTrap;
            other.Remove();
            transform.DOPunchScale(Vector3.one * 0.5f, 0.25f, 1);
            transform.DOJump(transform.position, 0.2f, 1, 0.25f);
            yield return new WaitForSeconds(1.5f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public override void OnCollidedWall(Vector2 collisionDirection)
    {
    }

    public override void OnStepWalked()
    {
    }
}
