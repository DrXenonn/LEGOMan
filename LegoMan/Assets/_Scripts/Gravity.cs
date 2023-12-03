using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class Gravity : MonoBehaviour
{
    private float _fallInterval;
    private float _timeSinceLastFall = 0f;
    private List<Collider2D> _collidingObjects = new List<Collider2D>();
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = transform.root.GetComponent<AudioSource>();
    }

    private void Start()
    {
        _fallInterval = GameManager.Instance.fallInterval;
    }

    private void OnEnable()
    {
        _collidingObjects.Clear();
    }

    private void Update()
    {
        if (!GameManager.Instance.isHolding)
        {
            _timeSinceLastFall += Time.deltaTime;
            if (_timeSinceLastFall >= _fallInterval)
            {
                StartCoroutine(MoveDownWithDelay());
                _timeSinceLastFall = 0f;
            }
        }
    }

    private IEnumerator MoveDownWithDelay()
    {
        yield return new WaitForSeconds(0.02f);

        if (_collidingObjects.Count == 0)
        {
            MoveDown();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Brick") || other.CompareTag("Ground"))
        {
            _collidingObjects.Add(other);
            if (!GameManager.Instance.isHolding)
            {
                _audioSource.Play();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Brick") || other.CompareTag("Ground"))
        {
            _collidingObjects.Remove(other);
        }
    }

    private void MoveDown()
    {
        if (GameManager.Instance.isHolding) return;
        transform.parent.position -= new Vector3(0f, 0.16f, 0f);
    }
}