using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boulder : Danger
{
    [SerializeField]
    float _boulderSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 nextPos = transform.localPosition;
        if (dir == direction.Up)
        {
            nextPos.y += _boulderSpeed;
            if (nextPos.y >= 12)
            {
                increaseScore();
            }
        }
        else if (dir == direction.Down)
        {
            nextPos.y -= _boulderSpeed;
            if (nextPos.y <= -3)
            {
                increaseScore();
            }
        }
        else if (dir == direction.Left)
        {
            nextPos.x -= _boulderSpeed;
            if (nextPos.x <= -3)
            {
                increaseScore();
            }
        }
        else
        {
            nextPos.x += _boulderSpeed;
            if (nextPos.x >= 12)
            {
                increaseScore();
            }
        }
        transform.localPosition = nextPos;
    }
}
