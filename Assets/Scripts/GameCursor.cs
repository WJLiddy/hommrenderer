using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class GameCursor : MonoBehaviour
{
    public Camera gameCamera;
    public GameObject heroInfoPopup;
    public RectTransform canvas;
    public WorldRenderer worldRenderer;
    public GameClient gameClient;
    public CityInfoPanel cityInfoPanel;

    public int selectedHero;
    public Vector2Int selectedCity;
    public enum CursorState { NONE, HERO, CITY };
    CursorState cursorState;

    public Vector2 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
    }

    private void moveCamera()
    {
        var mp = Input.mousePosition;

        if (Vector3.Distance(gameCamera.transform.position, new Vector3(targetPosition.x, gameCamera.transform.position.y, targetPosition.y)) < 0.1f)
        {
            targetPosition = Vector2.zero;
        }

        if (targetPosition != Vector2.zero)
        {
            gameCamera.transform.position = Vector3.Lerp(gameCamera.transform.position, new Vector3(targetPosition.x, gameCamera.transform.position.y, targetPosition.y), 10 * Time.deltaTime);
        }
        else
        {

            // camera scroll
            if (mp.x < Screen.width * 0.02f)
            {
                gameCamera.transform.position -= Vector3.right * 10 * Time.deltaTime;
            }
            if (mp.x > Screen.width * 0.98f)
            {
                gameCamera.transform.position -= Vector3.left * 10 * Time.deltaTime;
            }

            if (mp.y < Screen.height * 0.02f)
            {
                gameCamera.transform.position -= Vector3.forward * 10 * Time.deltaTime;
            }

            if (mp.y > Screen.height * 0.98f)
            {
                gameCamera.transform.position -= Vector3.back * 10 * Time.deltaTime;
            }
        }

    }
    // Update is called once per frame
    void Update()
    {
        // reset stuff
        moveCamera();
        heroInfoPopup.SetActive(false);
        if (cursorState != CursorState.CITY)
        {
            cityInfoPanel.gameObject.SetActive(false);
        }
        // check for raycast hit
        RaycastHit hit;
        Ray ray = gameCamera.ScreenPointToRay(Input.mousePosition);
        Vector3 point = Vector3.zero;
        bool pointSet = false;
        if (Physics.Raycast(ray, out hit))
        {
            // convert collision point to grid.
            point = hit.point / WorldRenderer.TILE_SIZE;
            pointSet = true;
        }

        // condition 0: nothing selected.
        if (cursorState == CursorState.NONE)
        {
            // check if we clicked a hero.
            if (pointSet)
            {
                checkForHero((int)(point.x), (int)(point.z));
            }

            // check if we clicked a city.
            if (pointSet && cursorState != CursorState.HERO)
            {
                checkForCity((int)(point.x), (int)(point.z));
            }

        }
        // condition 1: city selected
        else if (cursorState == CursorState.CITY)
        {
            cityInfoPanel.gameObject.SetActive(true);
            cityInfoPanel.setCityData(worldRenderer.cities[selectedCity].Item2);
        }

        // condition 2 : hero selected
        else if (cursorState == CursorState.HERO)
        {
            // show the grid tiles.
            worldRenderer.setGridTiles(worldRenderer.tilesWithinRangeOfHero(worldRenderer.heroes[selectedHero].Item2[0], worldRenderer.heroes[selectedHero].Item2[1], worldRenderer.heroes[selectedHero].Item2[2]["move_points"]).Keys.ToList());
            
            // check for a move command.
            if (Input.GetMouseButtonDown(0) && pointSet)
            {
                Vector2Int diff = new Vector2Int((int)point.x - (int)worldRenderer.heroes[selectedHero].Item2[0], (int)point.z - (int)worldRenderer.heroes[selectedHero].Item2[1]);

                // try to do a move
                if (diff.magnitude > 0)
                {
                    doMove(point);
                }
                // special case - if magnitude is 0, do city transaction instead
                if (diff.magnitude == 0 && cityAt((int)(point.x), (int)(point.z)))
                {
                    // city
                    checkForCity((int)(point.x), (int)(point.z));
                    selectedHero = -1;
                    cursorState = CursorState.NONE;
                }

            }
        }
    }

    public void doMove(Vector3 point)
    {
        // walk back from the selected coordinate
        var search = worldRenderer.tilesWithinRangeOfHero(worldRenderer.heroes[selectedHero].Item2[0], worldRenderer.heroes[selectedHero].Item2[1], worldRenderer.heroes[selectedHero].Item2[2]["move_points"]);

        var ptr = new Vector2Int((int)point.x, (int)point.z);

        // if ptr not in search list -> not a move...
        if (!search.ContainsKey(ptr))
        {
            // user just wanted to click away
            selectedHero = -1;
            cursorState =CursorState.NONE;
            return;
        }

        List<Vector2Int> moves = new List<Vector2Int>();
        while (ptr.x != -999)
        {
            var prev = search[ptr];
            moves.Add(ptr - prev);
            ptr = prev;
        }
        moves.Reverse();
        // first move is the -999 move
        moves.RemoveAt(0);

        // send all commands
        Vector2Int cumMove = Vector2Int.zero;
        foreach (var move in moves)
        {
            Debug.Log(move);
            gameClient.addMoveCommandToQueue(new Vector2Int(worldRenderer.heroes[selectedHero].Item2[0], worldRenderer.heroes[selectedHero].Item2[1]) + cumMove, move);
            cumMove += move;
        }
        selectedHero = -1;
        cursorState = CursorState.NONE;
    }

    public bool cityAt(int x, int y)
    {
        foreach (var city in worldRenderer.cities.Values)
        {
            if (city.Item2[0] == x && city.Item2[1] == y)
            {
                return true;
            }
        }
        return false;
    }
    public void checkForCity(int x, int y)
    {
        foreach (var city in worldRenderer.cities.Values)
        {
            if (city.Item2[0] == x && city.Item2[1] == y && Input.GetMouseButtonDown(0))
            {

                cursorState = CursorState.CITY;
                selectedCity = new Vector2Int(city.Item2[0], city.Item2[1]);
                // zoom to city
                targetPosition = new Vector2(WorldRenderer.TILE_SIZE * x, WorldRenderer.TILE_SIZE * (y - 5));
            }
        }
    }

    public void checkForHero(int x, int y)
    {
        // check if any hero is at this tile.
        foreach(var hero in worldRenderer.heroes.Values)
        {
            if (hero.Item2[0] == x && hero.Item2[1] == y)
            {
                heroInfoPopup.SetActive(true);
                heroInfoPopup.transform.position = Input.mousePosition;
                heroInfoPopup.transform.Find("Panel").Find("Hero Name").GetComponent<Text>().text = HeroModel.allNames[hero.Item2[2]["id"]];
                heroInfoPopup.transform.Find("Panel").Find("InfStack").Find("Text").GetComponent<Text>().text = hero.Item2[2]["unit_stacks"][0].ToString();
                heroInfoPopup.transform.Find("Panel").Find("ArcStack").Find("Text").GetComponent<Text>().text = hero.Item2[2]["unit_stacks"][1].ToString();
                heroInfoPopup.transform.Find("Panel").Find("CavStack").Find("Text").GetComponent<Text>().text = hero.Item2[2]["unit_stacks"][2].ToString();
                heroInfoPopup.transform.Find("Panel").Find("BalStack").Find("Text").GetComponent<Text>().text = hero.Item2[2]["unit_stacks"][3].ToString();
                heroInfoPopup.transform.Find("Panel").Find("WizStack").Find("Text").GetComponent<Text>().text = hero.Item2[2]["unit_stacks"][4].ToString();
                heroInfoPopup.transform.Find("Panel").Find("MoveAmt").Find("Text").GetComponent<Text>().text = hero.Item2[2]["move_points"].ToString();

                if (Input.GetMouseButtonDown(0))
                {
                    selectedHero = hero.Item2[2]["id"].AsInt;
                    cursorState = CursorState.HERO;
                    // zoom to hero
                    targetPosition = new Vector2(WorldRenderer.TILE_SIZE * x, WorldRenderer.TILE_SIZE * (y - 5));
                }
                return;
            }
        }
    }

    public void closeCityMenu()
    {
        cursorState = CursorState.NONE;
    }

    public void sendBuildCommand(string buildString)
    {
        gameClient.addBuildCommandToQueue(new Vector2Int(selectedCity[0], selectedCity[1]), buildString);
    }

    public void sendBuyCommand(string buyString)
    {
        gameClient.addBuyCommandToQueue(new Vector2Int(selectedCity[0], selectedCity[1]),buyString);
    }


}
