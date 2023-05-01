using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Danger
{
    float _age;

    // Start is called before the first frame update
    void Start()
    {
        _age = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        _age += Time.deltaTime;
        if (_age >= 1f)
        {
            increaseScore();
        }
    }
}
