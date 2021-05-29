using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Walking : MonoBehaviour
{
    [SerializeField] private float speed = 2;
    [SerializeField] private EventManager eventManager = null;
    [SerializeField] private LayerMask blockers = new LayerMask();
    [SerializeField] private SpriteRenderer spriterenderer;
    public TutorialImage tut = null;

    private Rigidbody2D body = null;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        StartCoroutine(InputCoroutine());
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Align();
    }

    private IEnumerator MoveTo(Vector2 position)
    {
        while ((body.position - position).magnitude > 0)
        {
            body.MovePosition(Vector2.MoveTowards(body.position, position, speed * Time.fixedDeltaTime));
            yield return new WaitForFixedUpdate();
     
        }
    }

    private IEnumerator InputCoroutine()
    {
        while (true)
        {
            bool AnyKey()
            {
                return Input.anyKey;
            }
            if(tut != null && tut.getActive()) yield return new WaitUntil(AnyKey);
            bool IsKeyDown()
            {
                return Input.GetKeyDown(KeyCode.W)
                    || Input.GetKeyDown(KeyCode.A)
                    || Input.GetKeyDown(KeyCode.S)
                    || Input.GetKeyDown(KeyCode.D)
                    || Input.GetKeyDown(KeyCode.Space);
            }
            yield return new WaitUntil(IsKeyDown);
            Vector3 direction =
                 Input.GetKeyDown(KeyCode.W) ? Vector3.up :
                 Input.GetKeyDown(KeyCode.S) ? Vector3.down :
                 Input.GetKeyDown(KeyCode.A) ? Vector3.left :
                 Input.GetKeyDown(KeyCode.D) ? Vector3.right :
                 Vector3.zero;
            if (Input.GetKeyDown(KeyCode.D)) spriterenderer.flipX = false;
            if (Input.GetKeyDown(KeyCode.A)) spriterenderer.flipX = true;
            if (Input.GetKeyDown(KeyCode.Space)) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Vector3 newPosition = transform.position + direction;

            if (!CanWalkTo(direction)) continue;

            eventManager.WindStarted.Invoke(direction);
            yield return FindObjectOfType<GridManager>().PlayUntilEveryoneHalt();
        }
    }

    private void Align()
    {
        var position = body.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);
        body.MovePosition(position);
    }


    private bool CanWalkTo(Vector2 direction)
    {
        var contact = Physics2D.OverlapBox(body.position + direction, Vector2.one * 0.9f, direction.magnitude, blockers);
        return contact == null;
    }
}
