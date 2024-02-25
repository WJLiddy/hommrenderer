using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{

    public Button[] heroButtons;
    public Button[] cityButtons;
    public WorldRenderer worldRenderer;
    public GameCursor gameCursor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onHeroButtonClick(int index)
    {
        var vec = worldRenderer.heroAtIndex(index);
        gameCursor.targetPosition = new Vector2(WorldRenderer.TILE_SIZE * vec.x, WorldRenderer.TILE_SIZE * (vec.y - 5));
    }

    public void onCityButtonClick(int index)
    {
        var vec = worldRenderer.cityAtIndex(index);
        gameCursor.targetPosition = new Vector2(WorldRenderer.TILE_SIZE * vec.x, WorldRenderer.TILE_SIZE * (vec.y - 5));
    }
}
