using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _tileWidth, _tileHeight, _speed;

    private Queue<KeyCode> _moveQueue;

    private Vector2 _nextPos;
    // Start is called before the first frame update
    void Start()
    {
        _moveQueue = new Queue<KeyCode>();
        StartCoroutine(moveManager());
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            _moveQueue.Enqueue(KeyCode.UpArrow);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            _moveQueue.Enqueue(KeyCode.LeftArrow);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            _moveQueue.Enqueue(KeyCode.DownArrow);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            _moveQueue.Enqueue(KeyCode.RightArrow);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }

    public void setLocation(float x, float y)
    {
        transform.localPosition = new Vector2(x, y);
    }

    public void setTileSize(float w, float h)
    {
        _tileWidth = w;
        _tileHeight = h;
    }

    IEnumerator moveManager()
    {
        while (true)
        {
            if(_moveQueue.Count > 0)
            {
                yield return StartCoroutine(move(_moveQueue.Dequeue()));
            }
            yield return null;
        }
    }

    IEnumerator move(KeyCode nextDir)
    {
        GetComponent<CircleCollider2D>().radius = 0.1f; 
        _nextPos = transform.localPosition;
        switch (nextDir)
        {
            case KeyCode.UpArrow:
                _nextPos.y += _tileHeight;
                break;
            case KeyCode.DownArrow:
                _nextPos.y -= _tileHeight;
                break;
            case KeyCode.RightArrow:
                _nextPos.x += _tileWidth;
                break;
            case KeyCode.LeftArrow:
                _nextPos.x -= _tileWidth;
                break;
            default: break;
        }

        while (transform.localPosition.x != _nextPos.x || transform.localPosition.y != _nextPos.y)
        {
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, _nextPos, Time.deltaTime * _speed);
            yield return null;
        }
        GetComponent<CircleCollider2D>().radius = 0.35f;
    }

}

