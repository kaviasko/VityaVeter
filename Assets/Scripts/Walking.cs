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
        //SwipeDetector.OnSwipe += SwipeMovement;
    }

    void Start()
    {
        StartCoroutine(InputCoroutine());
        //SwipeDetector.OnSwipe += SwipeMovement;
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

    private IEnumerator InputCoroutine() {

        while (true)
        {
            bool AnyKey()
            {
                return Input.anyKey;
            }
            if (tut != null && tut.getActive()) yield return new WaitUntil(AnyKey);
            bool IsKeyDown()
            {
                return SwipeDetector.up
                    || SwipeDetector.down
                    || SwipeDetector.left
                    || SwipeDetector.right;
            }
            yield return new WaitUntil(IsKeyDown);
            Vector3 direction =
                 SwipeDetector.up ? Vector3.up :
                 SwipeDetector.down ? Vector3.down :
                 SwipeDetector.left ? Vector3.left :
                 SwipeDetector.right ? Vector3.right :
                 Vector3.zero;
            if (SwipeDetector.right) spriterenderer.flipX = false;
            if (SwipeDetector.left) spriterenderer.flipX = true;
            Vector3 newPosition = transform.position + direction;

            if (!CanWalkTo(direction)) continue;

            eventManager.WindStarted.Invoke(direction);
            yield return FindObjectOfType<GridManager>().PlayUntilEveryoneHalt();
        }
    }

    /*private void SwipeMovement(SwipeData data)
    {
        StartCoroutine(SwipeCoroutine2());

        IEnumerator SwipeCoroutine2()
        {
            bool AnyKey()
            {
                return Input.anyKey;
            }
            if (tut != null && tut.getActive()) yield return new WaitUntil(AnyKey);

            Vector3 direction =
                                data.Direction == SwipeDirection.Up ? Vector3.up :
                                data.Direction == SwipeDirection.Down ? Vector3.down :
                                data.Direction == SwipeDirection.Left ? Vector3.left :
                                data.Direction == SwipeDirection.Right ? Vector3.right :
                                Vector3.zero;
            if (data.Direction == SwipeDirection.Right) spriterenderer.flipX = false;
            if (data.Direction == SwipeDirection.Left) spriterenderer.flipX = true;

            eventManager.WindStarted.Invoke(direction);
            yield return FindObjectOfType<GridManager>().PlayUntilEveryoneHalt();
        }

        IEnumerator SwipeCouroutine()
        {
            bool AnyKey()
            {
                return Input.anyKey;
            }
            if (tut != null && tut.getActive()) yield return new WaitUntil(AnyKey);

            bool IsTouched()
            {
                return data.Direction == SwipeDirection.Up
                    || data.Direction == SwipeDirection.Down
                    || data.Direction == SwipeDirection.Left
                    || data.Direction == SwipeDirection.Right;
            }
            yield return new WaitUntil(IsTouched);
            Vector3 direction =
                                data.Direction == SwipeDirection.Up ? Vector3.up :
                                data.Direction == SwipeDirection.Down ? Vector3.down :
                                data.Direction == SwipeDirection.Left ? Vector3.left :
                                data.Direction == SwipeDirection.Right ? Vector3.right :    
                                Vector3.zero;
            if (data.Direction == SwipeDirection.Right) spriterenderer.flipX = false;
            if (data.Direction == SwipeDirection.Left) spriterenderer.flipX = true;

            Vector3 newPosition = transform.position + direction;
            //if (!CanWalkTo(direction)) continue;
            eventManager.WindStarted.Invoke(direction);
            yield return FindObjectOfType<GridManager>().PlayUntilEveryoneHalt();
        }
    }*/

    /*private IEnumerator InputCoroutine(SwipeData data)
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
    }*/

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
