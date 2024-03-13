using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class WorldRenderer : MonoBehaviour
{
    public static readonly float TILE_SIZE = (5 * 0.64f);

    // Tile junk
    public Tilemap tilemap;
    public Tilemap grid;
    public Tile grass;
    public Tile water;
    public Tile gridTile;
    public Tile inRangeTile;

    // Prefabs
    public GameObject townBase;
    public GameObject cerealBase;
    public GameObject weedBase;
    public GameObject bitcoinBase;
    public GameObject mountainBase;
    public GameObject heroBase;

    // UI Junk
    public Text BTCVal;
    public Text WeedVal;
    public Text CerealVal;
    public Text Turn;
    public Text Turnover;
    public GameMenu gameMenu;

    // Mats
    public Material[] teamBuildingColors;
    public Material neutralBuildingColor;
    public Material[] teamUnitColors;

    // Terrain junk
    private List<GameObject> mountains = new List<GameObject>();

    // structure and hero info
    public Dictionary<int, Tuple<GameObject, SimpleJSON.JSONNode>> heroes = new Dictionary<int, Tuple<GameObject, SimpleJSON.JSONNode>>();
    public Dictionary<Vector2Int, Tuple<GameObject, SimpleJSON.JSONNode>> farms = new Dictionary<Vector2Int, Tuple<GameObject, SimpleJSON.JSONNode>>();
    public Dictionary<Vector2Int, Tuple<GameObject, SimpleJSON.JSONNode>> cities = new Dictionary<Vector2Int, Tuple<GameObject, SimpleJSON.JSONNode>>();

    // state junk
    private SimpleJSON.JSONNode lastState = null;
    private int MAPSIZE = 0;


    // Update is called once per frame
    void Update()
    {
        // just make sure mapsize was set
        if (MAPSIZE != 0)
        {
            clearGridTiles();
        }
    }

    private Vector3 gridToWorldPosition(int x, int y)
    {
        return new Vector3((TILE_SIZE * 0.5f) + x * TILE_SIZE, 0, (TILE_SIZE * 0.5f) + y * TILE_SIZE);
    }

    public void clearGridTiles()
    {
        grid.ClearAllTiles();
        for (int x = 0; x < MAPSIZE; x++)
        {
            for (int y = 0; y < MAPSIZE; y++)
            {
                grid.SetTile(new Vector3Int(x, y, 0), gridTile);
            }
        }
    }

    public void renderTiles(SimpleJSON.JSONNode node, int playerID)
    {
        MAPSIZE = node["tiles"].Count;
        if (lastState == null)
        {
            lastState = node;
            tilemap.ClearAllTiles();
            for (int x = 0; x < MAPSIZE; x++)
            {
                for (int y = 0; y < MAPSIZE; y++)
                {
                    string tile = node["tiles"][x][y];
                    if (tile == "open")
                    {
                        tilemap.SetTile(new Vector3Int(x, y, 0), grass);
                        //Instantiate open tile
                    }
                    else if (tile == "water")
                    {
                        tilemap.SetTile(new Vector3Int(x, y, 0), water);
                        //Instantiate water tile
                    }
                    else if (tile == "mountain")
                    {
                        tilemap.SetTile(new Vector3Int(x, y, 0), grass);
                        var v = Instantiate(mountainBase);
                        mountains.Add(v);
                        v.transform.position = gridToWorldPosition(x, y);
                        //Instantiate mountain tile (later)
                    }
                }
            }
        }
    }

    public void renderCities(SimpleJSON.JSONNode node, int playerID)
    {
        int cityUIIdx = 0;
        int cityidx = 0;
        foreach (SimpleJSON.JSONNode city in node["cities"].AsArray)
        {
            if (!cities.ContainsKey(new Vector2Int(city[0], city[1])))
            {
                // struct hasn't been init'd yet
                var v = Instantiate(townBase);
                v.transform.position = gridToWorldPosition(city[0], city[1]);
                cities[new Vector2Int(city[0], city[1])] = new Tuple<GameObject, SimpleJSON.JSONNode>(v, city);
            }
            // update city data every frame
            // wow this is filthy wow!
            cities[new Vector2Int(city[0], city[1])] = new Tuple<GameObject, JSONNode>(cities[new Vector2Int(city[0], city[1])].Item1,city);
            // set color
            var colorMat = (city[3] == -1) ? neutralBuildingColor : teamBuildingColors[city[3]];
            cities[new Vector2Int(city[0], city[1])].Item1.GetComponentInChildren<MeshRenderer>().material = colorMat;

            if (city[3] == playerID)
            {
                //show it on UI
                gameMenu.cityButtons[cityUIIdx].gameObject.SetActive(true);
                string cityName = CityMenu.allNames[cityidx];
                gameMenu.cityButtons[cityUIIdx].GetComponentInChildren<Text>().text = cityName;
                cityUIIdx++;
            }
            cityidx++;
        }
        // clear rest of UI
        for (int i = cityUIIdx; i < 5; ++i)
        {
            gameMenu.cityButtons[i].gameObject.SetActive(false);
        }
    }

    public void renderFarms(SimpleJSON.JSONNode node, int playerID)
    {
        foreach (SimpleJSON.JSONNode farm in node["farms"].AsArray)
        {
            // cereal, pot, bitcoin.
            if (!farms.ContainsKey(new Vector2Int(farm[0], farm[1])))
            {
                GameObject v = null;
                switch (farm[2].Value)
                {
                    case "cereal":
                        v = Instantiate(cerealBase);
                        break;
                    case "pot":
                        v = Instantiate(weedBase);
                        break;
                    case "bitcoin":
                        v = Instantiate(bitcoinBase);
                        break;
                }
                v.transform.position = gridToWorldPosition(farm[0], farm[1]);
                farms[new Vector2Int(farm[0], farm[1])] = new Tuple<GameObject, SimpleJSON.JSONNode>(v, farm);
            }
            //set color
            var colorMat = (farm[3] == -1) ? neutralBuildingColor : teamBuildingColors[farm[3]];
            farms[new Vector2Int(farm[0], farm[1])].Item1.GetComponentInChildren<MeshRenderer>().material = colorMat;
        }
    }

    public void renderPlayerUI(SimpleJSON.JSONNode node, int playerID)
    {
        int ctr = 0;
        Turn.text = "Week " + (1+((int)(node["day"].AsInt / 7))) + ", Day " + (1+(node["day"].AsInt % 7));
        Debug.Log(node["players"][playerID]["team"] != node["team"]);
        
        // fuck!
        if (node["team"])
        {
            Turnover.text = " (Turn Over)";
        }
        else
        {
            Turnover.text = "";
        }

        foreach (SimpleJSON.JSONNode player in node["players"].AsArray)
        {
            if (ctr == playerID)
            {
                BTCVal.text = player["bitcoin"].Value;
                WeedVal.text = player["pot"].Value;
                CerealVal.text = player["cereal"].Value;

            }
            ctr++;
        }
    }

    public void renderHeroes(SimpleJSON.JSONNode node, int playerID)
    {
        int heroUIIdx = 0;
        foreach (SimpleJSON.JSONNode hero in node["heroes"].AsArray)
        {
            int x = hero[0];
            int y = hero[1];
            int id = hero[2]["id"];
            if (!heroes.ContainsKey(id))
            {
                var heroModel = Instantiate(heroBase);
                heroModel.GetComponent<HeroModel>().makeHeroFromID(id);
                // need this.
                foreach (var v in heroModel.GetComponentInChildren<HeroModel>().unitMaterials)
                {
                    if (v.GetComponent<SkinnedMeshRenderer>() != null)
                    {
                        v.GetComponent<SkinnedMeshRenderer>().material = teamUnitColors[hero[3]];
                    }
                    if (v.GetComponent<MeshRenderer>() != null)
                    {
                        v.GetComponent<MeshRenderer>().material = teamUnitColors[hero[3]];
                    }
                }
                heroes[id] = new Tuple<GameObject, SimpleJSON.JSONNode>(heroModel, hero);
            }
            heroes[id] = new Tuple<GameObject, SimpleJSON.JSONNode>(heroes[id].Item1, hero);

            heroes[id].Item1.transform.position = gridToWorldPosition(x, y);
            // boost hero up if in a town
            if (cities.ContainsKey(new Vector2Int(x, y)))
            {
                heroes[id].Item1.transform.position += new Vector3(0, 0.5f, -1.5f);
            }
            // if hero's on the team...
            if (hero[3] == playerID)
            {
                //show it on UI
                gameMenu.heroButtons[heroUIIdx].gameObject.SetActive(true);
                gameMenu.heroButtons[heroUIIdx].gameObject.SetActive(true);
                string heroName = HeroModel.allNames[hero[2]["id"]];
                gameMenu.heroButtons[heroUIIdx].GetComponentInChildren<Text>().text = heroName;
                heroUIIdx++;
            }
        }
        // clear rest of UI
        for(int i = heroUIIdx; i < 3; ++i)
        {
            gameMenu.heroButtons[i].gameObject.SetActive(false);
        }
    }
    public void UpdateMap(SimpleJSON.JSONNode node, int playerID)
    {
        renderTiles(node, playerID);
        renderCities(node, playerID);
        renderFarms(node, playerID);
        renderPlayerUI(node, playerID);
        renderHeroes(node, playerID);
    }

   // next -> prev
   public Dictionary<Vector2Int,Vector2Int> tilesWithinRangeOfHero(int x, int y, int dist)
   {
        Vector2Int[] directions = new Vector2Int[4] { new Vector2Int(-1, 0), new Vector2Int(1, 0), new Vector2Int(0, 1), new Vector2Int(0, -1) };
        

        List<Tuple<Vector2Int,int>> queue = new List<Tuple<Vector2Int,int>>();

        // magic num since vector2 non nullable
        queue.Add(new Tuple<Vector2Int,int>(new Vector2Int(x, y), dist));

        // All nodes we've visited.
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
        visited.Add(new Vector2Int(x, y));
        // The node that leads to each node we've visited.

        Dictionary<Vector2Int, Vector2Int> prev = new Dictionary<Vector2Int, Vector2Int>();
        // magic number because we can't have a union type..
        prev[new Vector2Int(x, y)] = new Vector2Int(-999, -999);

        // bfs
        while (queue.Count > 0)
        {
            var v = queue[0];
            queue.RemoveAt(0);

            // cutoff if dist = 0.
            if (v.Item2 == 0)
            {
                continue;
            }

            foreach (var dir in directions)
            {
                var newV = v.Item1 + dir;

                // in bounds
                if (newV.x < 0 || newV.x >= MAPSIZE || newV.y < 0 || newV.y >= MAPSIZE)
                {
                    continue;
                }

                // no terrain
                if (lastState["tiles"][newV.x][newV.y] == "water" || lastState["tiles"][newV.x][newV.y] == "mountain")
                {
                    continue;
                }

                // not visited already
                if (visited.Contains(v.Item1 + dir))
                {
                    continue;
                }

                // this node has been visited. Add to queue.
                prev[newV] = v.Item1;
                visited.Add(newV);
                // it will be added to the queue.
                queue.Add(new Tuple<Vector2Int, int>(newV, v.Item2 - 1));
            }
        }

        // okay, explored it all
        // assemble list with prev's so we can chain commands to server
        Dictionary<Vector2Int, Vector2Int> ret = new Dictionary<Vector2Int, Vector2Int>();

        foreach (var v in visited)
        {
            ret[v] = prev[v];
        }
        return ret;
   }

   // renderGrid
   public void setGridTiles(List<Vector2Int> tiles)
   {
        foreach (var v in tiles)
        {
            grid.SetTile(new Vector3Int(v.x, v.y, 0), inRangeTile);
        }
   }

    // this might break if called when lastState changed.
    public Vector2 heroAtIndex(int index)
    {
        foreach (SimpleJSON.JSONNode hero in lastState["heroes"].AsArray)
        {
            // if hero's on the team...
            if (hero[3] == 0)
            {
                if (index == 0)
                {
                    return new Vector2(hero[0], hero[1]);
                }
                index--;
            }
        }
        return Vector2.zero;
    }

    public Vector2 cityAtIndex(int index)
    {
        foreach (SimpleJSON.JSONNode city in lastState["cities"].AsArray)
        {
            if (city[3] == 0)
            {
                if (index == 0)
                {
                    //show it on UI
                    return new Vector2(city[0], city[1]);
                }
                index--;
            }
        }
        return Vector2.zero;
    }
}
