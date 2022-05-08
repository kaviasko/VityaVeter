using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class BreakableWall : GridBehaviour
{
    [SerializeField] private Sprite[] breakingAnimation;
    [SerializeField] private float framerate;
    [Space]
    [SerializeField] private UnityEvent onBreak;
    public AudioManager audioManager;

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
        audioManager.Play("Crumble");
        StartCoroutine(DelayedBreak());
        
        IEnumerator DelayedBreak()
        {
            var renderer = GetComponent<SpriteRenderer>();
            var wait = new WaitForSeconds(1 / framerate);
            foreach (var frame in breakingAnimation)
            {
                renderer.sprite = frame;
                yield return wait;
            }
            yield return null;
            onBreak.Invoke();
            Remove();
        }
    }

    public override void OnCollidedWall(Vector2 collisionDirection)
    {
    }

    public override void OnStepWalked()
    {
    }
}
