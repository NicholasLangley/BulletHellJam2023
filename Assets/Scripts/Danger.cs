using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Danger : MonoBehaviour
{
    public enum direction {Up, Down, Left, Right}

    public direction dir;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setDirection(direction d)
    {
        dir = d;
    }

    public void increaseScore()
    {
        GameObject.FindObjectOfType<GameController>().increaseScore(1);
        Destroy(gameObject);
    }
}
