using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    [SerializeField]
    private Tile floorTile, wallTile;

    [SerializeField]
    Transform cam;

    [SerializeField]
    Spawner spawner;

    [SerializeField]
    WeaponManager weaponManager;

    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    // Start is called before the first frame update
    public void generateGrid(int width, int height, float tileWidth, float tileHeight, Spawner.weapon w)
    {
        weaponManager.setDimensions(width, height);
        for (int x = 0; x <= width+1; x++)
        {
            for (int y = 0; y <= height+1; y++)
            {
                if(x == 0 || x == width + 1 || y == 0 || y == height + 1)
                {
                    Tile newTile = Instantiate(wallTile, new Vector2(x*tileWidth,y*tileHeight), Quaternion.identity);
                    newTile.name = $"Wall {x} {y}";
                    newTile.transform.SetParent(transform);
                   if(x == 0 && y != 0 && y!= height + 1)
                    {
                        Spawner newSpawner = Instantiate(spawner, new Vector3(x * tileWidth, y * tileHeight, -1f), Quaternion.identity);
                        newSpawner.setDirection(Danger.direction.Right);
                        weaponManager.addLeft(newSpawner, y - 1);
                    }
                    else if (x == width + 1 && y != 0 && y != height + 1)
                    {
                        Spawner newSpawner = Instantiate(spawner, new Vector3(x * tileWidth, y * tileHeight, -1f), Quaternion.identity);
                        newSpawner.setDirection(Danger.direction.Left);
                        weaponManager.addRight(newSpawner, y - 1);
                    }
                    else if (y == 0 && x != 0 && x != width + 1)
                    {
                        Spawner newSpawner = Instantiate(spawner, new Vector3(x * tileWidth, y * tileHeight, -1f), Quaternion.identity);
                        newSpawner.setDirection(Danger.direction.Up);
                        weaponManager.addBottom(newSpawner, x - 1);
                    }
                    else if (y == height + 1 && x != 0 && x != width + 1)
                    {
                        Spawner newSpawner = Instantiate(spawner, new Vector3(x * tileWidth, y * tileHeight, -1f), Quaternion.identity);
                        newSpawner.setDirection(Danger.direction.Down);
                        weaponManager.addTop(newSpawner, x - 1 );
                    }
                }
                else
                {
                    Tile newTile = Instantiate(floorTile, new Vector2(x*tileWidth, y*tileHeight), Quaternion.identity);
                    newTile.name = $"Floor {x} {y}";
                    newTile.transform.SetParent(transform);
                }
            }
        }

        cam.transform.localPosition = new Vector3((float)((width + 1) *1.3 / 2), (float)((height + 1)*1.2 / 2), -3f); 
    }
}
