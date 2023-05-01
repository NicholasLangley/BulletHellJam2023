using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    [SerializeField]
    Player player;
    [SerializeField]
    GridManager gm;
    [SerializeField]
    WeaponManager wm;

    int score;
    [SerializeField]
    TextMeshProUGUI scoreText;

    [SerializeField]
    int mapWidth, mapHeight;

    [SerializeField]
    float tileWidth, tileHeight;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        scoreText.SetText("{0}", score);
        gm.generateGrid(mapWidth, mapHeight, tileWidth, tileHeight, Spawner.weapon.laser);
        player.setLocation(3f*tileWidth, 3f*tileHeight);
        player.setTileSize(tileWidth, tileHeight);
        wm.startFiring();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void increaseScore(int s)
    {
        score += s;
        scoreText.SetText("{0}", score);
    }
}
