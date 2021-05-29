using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ParticleSystem))]
public class WindEffect : MonoBehaviour
{
    [SerializeField] private EventManager eventManager = null;
    public AudioManager audioManager;

    private ParticleSystem fx = null;


    void Start()
    {
        fx = GetComponent<ParticleSystem>();
        eventManager.WindStarted.AddListener(OnWindStarted);
        audioManager = FindObjectOfType<AudioManager>();
    }

    void OnWindStarted(Vector2 direction)
    {
        audioManager.Play("WindSound");
        if (direction != Vector2.zero)
        {
            transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
            fx.Play();
        }
    }
}
