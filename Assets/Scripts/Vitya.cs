using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Vitya : GridBehaviour
{
    [SerializeField] private EventManager eventmgr;
    public AudioManager audioManager;
    public static bool isThrown = false;

    public void Start()
    {
        eventmgr.WindStarted.AddListener(OnWindStarted);
        audioManager = FindObjectOfType<AudioManager>();
    }

    public override void GoTo(Vector2 newPosition, float time)
    {
        transform.DOJump(newPosition, 0.3f, 1, time);
    }

    private void OnWindStarted(Vector2 v)
    {
        MovementDirection = v;
    }

    public override void OnCollidedBody(Vector2 collisionDirection, GridBehaviour other, bool isMyFault)
    {
        if (!other.IsTag("Enemy") && !other.IsTag("Spikes")) return;
        audioManager.Play("Death");
        Vector3 spin = 180 * (collisionDirection.x < 0 ? Vector3.back : Vector3.forward);
        Vector2 jumpEnd = (Vector2) transform.position - collisionDirection * 5;
        DOTween.Sequence()
            .Append(transform.DOJump(jumpEnd, 2f, 1, 1f))
            .Join(transform.DORotate(spin, 1f))
            .AppendCallback(() =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            });
        Debug.Log("COLLISON !!");
    }

    public override void OnCollidedWall(Vector2 collisionDirection)
    {
        Vitya.isThrown = false;
    }

    override public void OnStepWalked()
    {
        if (isThrown) return;
        MovementDirection = Vector2.zero;
    }
}
