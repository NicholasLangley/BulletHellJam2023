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

    bool currentlyFiring;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Renderer>().enabled = false;
        currentlyFiring = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void fireWeapon(weapon attack, float chargeTime)
    {
        StartCoroutine(chargeAttack(attack, chargeTime));
    }

    public IEnumerator chargeAttack(weapon attack, float delay)
    {
        if (attack != weapon.none && !currentlyFiring)
        {
            currentlyFiring = true;
            gameObject.GetComponent<Renderer>().enabled = true;
            yield return new WaitForSeconds(delay);
            yield return StartCoroutine(spawnAttack(attack));
            gameObject.GetComponent<Renderer>().enabled = false;
            currentlyFiring = false;
        }
    }

    public IEnumerator spawnAttack(weapon attack)
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
                newDanger = Instantiate(boulder);
                break;
        }
        newDanger.transform.localPosition = newPos;
        newDanger.setDirection(dir);
        yield return null;
    }

    public void setDirection(Danger.direction d)
    {
        dir = d;
        switch (d)
        {
            case Danger.direction.Up:
                transform.rotation = Quaternion.Euler(0, 0, 270);
                break;
            case Danger.direction.Down:
                transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
            case Danger.direction.Right:
                transform.rotation = Quaternion.Euler(0, 0, 180);
                break;
            default:
                break;
        }
    }
}
