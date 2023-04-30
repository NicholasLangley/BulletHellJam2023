using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public enum weapon {boulder, laser, none}

    [SerializeField]
    Boulder boulder;
    [SerializeField]
    Laser laser;

    [SerializeField]
    Danger.direction dir;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void spawnAttack(weapon attack)
    {
        Danger newDanger;
        Vector2 newPos = transform.localPosition;
        switch (attack)
        {
         case weapon.laser:
                newDanger = Instantiate(laser);
                switch(dir)
                {
                    case Danger.direction.Up:
                        newPos.y += 10;
                        newDanger.transform.rotation = Quaternion.Euler(0, 0, 90);
                        break;
                    case Danger.direction.Down:
                        newPos.y -= 10;
                        newDanger.transform.rotation = Quaternion.Euler(0, 0, 90);
                        break;
                    case Danger.direction.Right:
                        newPos.x += 10;
                        break;
                    case Danger.direction.Left:
                        newPos.x -= 10;
                        break;
                }
                break;
        case weapon.boulder:
                newDanger = Instantiate(boulder);
                break;
        default:
                return;
        }
        newDanger.transform.localPosition = newPos;
        newDanger.setDirection(dir);
    }

    public void setDirection(Danger.direction d)
    {
        dir = d;
    }
}
