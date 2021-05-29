using UnityEngine;
using System.Collections;

public class Windable : MonoBehaviour
{
    [SerializeField] private EventManager eventManager = null;
    public float speed = 10f;


    private Rigidbody2D body = null;
    private Coroutine windRoutine = null;


    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Use this for initialization
    void Start()
    {
        eventManager.WindStarted.AddListener(OnWindStarted);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (windRoutine != null)
        {
            StopCoroutine(windRoutine);
            Align();
        }
    }


    private IEnumerator MoveTowards(Vector2 direction)
    {
        body.AddForce(direction * speed * 100f, ForceMode2D.Force);
        yield break;
    }

    private void Align()
    {
        var position = body.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);
        body.MovePosition(position);
    }

    private void OnWindStarted(Vector2 direction)
    {
        Debug.Log("HOLADNA");
        if (windRoutine != null)
        {
            StopCoroutine(windRoutine);
            Align();
        }
        windRoutine = StartCoroutine(MoveTowards(direction));
    }
}
