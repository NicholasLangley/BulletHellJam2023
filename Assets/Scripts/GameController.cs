using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    Player player;
    [SerializeField]
    GridManager gm;

    [SerializeField]
    int mapWidth, mapHeight;

    [SerializeField]
    float tileWidth, tileHeight;

    // Start is called before the first frame update
    void Start()
    {
        gm.generateGrid(mapWidth, mapHeight, tileWidth, tileHeight, Spawner.weapon.laser);
        player.setLocation(3f*tileWidth, 3f*tileHeight);
        player.setTileSize(tileWidth, tileHeight);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
