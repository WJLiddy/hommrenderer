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

    // temp
    string json = "{\"tiles\":[[\"open\",\"open\",\"open\",\"open\",\"water\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"water\",\"water\",\"open\",\"open\",\"open\",\"open\"],[\"open\",\"open\",\"open\",\"open\",\"water\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\"],[\"open\",\"open\",\"open\",\"water\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"water\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\"],[\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\"],[\"open\",\"open\",\"open\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"water\",\"open\",\"open\",\"mountain\",\"open\",\"open\",\"open\"],[\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"mountain\",\"open\",\"open\",\"mountain\"],[\"open\",\"mountain\",\"open\",\"water\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"water\",\"mountain\",\"open\",\"open\",\"open\",\"open\",\"open\"],[\"open\",\"open\",\"open\",\"open\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\"],[\"open\",\"open\",\"open\",\"open\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\",\"mountain\"],[\"open\",\"open\",\"open\",\"open\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\",\"mountain\"],[\"open\",\"open\",\"open\",\"open\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\"],[\"open\",\"mountain\",\"open\",\"water\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"water\",\"mountain\",\"open\",\"open\",\"open\",\"open\",\"open\"],[\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"mountain\",\"open\",\"open\",\"mountain\"],[\"open\",\"open\",\"open\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"water\",\"open\",\"open\",\"mountain\",\"open\",\"open\",\"open\"],[\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\"],[\"open\",\"open\",\"open\",\"water\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"water\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\"],[\"open\",\"open\",\"open\",\"open\",\"water\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\"],[\"open\",\"open\",\"open\",\"open\",\"water\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"water\",\"water\",\"open\",\"open\",\"open\",\"open\"]],\"cities\":[[1,3,{\"built\":true,\"bitcoin_level\":3,\"defense_level\":3,\"meme_level\":0,\"unit_unlocks\":[true,false,false,false,false],\"units_available\":[0,0,0,0,0],\"units_garrisoned\":[0,0,0,0,0]},0],[6,10,{\"built\":false,\"bitcoin_level\":1,\"defense_level\":0,\"meme_level\":0,\"unit_unlocks\":[true,false,false,false,false],\"units_available\":[0,0,0,0,0],\"units_garrisoned\":[0,0,0,0,0]},1],[16,3,{\"built\":false,\"bitcoin_level\":1,\"defense_level\":0,\"meme_level\":0,\"unit_unlocks\":[true,false,false,false,false],\"units_available\":[0,0,0,0,0],\"units_garrisoned\":[0,0,0,0,0]},-1],[11,10,{\"built\":false,\"bitcoin_level\":1,\"defense_level\":0,\"meme_level\":0,\"unit_unlocks\":[true,false,false,false,false],\"units_available\":[0,0,0,0,0],\"units_garrisoned\":[0,0,0,0,0]},-1]],\"farms\":[[6,16,\"bitcoin\",-1],[6,14,\"bitcoin\",-1],[8,5,\"bitcoin\",0],[0,7,\"pot\",-1],[2,10,\"pot\",-1],[8,12,\"cereal\",-1],[7,7,\"cereal\",0],[11,16,\"bitcoin\",-1],[11,14,\"bitcoin\",-1],[9,5,\"bitcoin\",0],[17,7,\"pot\",-1],[15,10,\"pot\",-1],[9,12,\"cereal\",-1],[10,7,\"cereal\",-1]],\"players\":[{\"bitcoin\":420,\"pot\":0,\"cereal\":2,\"team\":0,\"name\":\"p0\"},{\"bitcoin\":120,\"pot\":0,\"cereal\":0,\"team\":1,\"name\":\"p1\"}],\"heroes\":[[7,5,{\"unit_stacks\":[5,0,0,0,0],\"move_points\":7,\"move_stat\":1,\"health_stat\":2,\"attack_stat\":3,\"memes\":[false,false,false,false,false],\"id\":0},0],[6,10,{\"unit_stacks\":[5,0,0,0,0],\"move_points\":7,\"move_stat\":1,\"health_stat\":2,\"attack_stat\":3,\"memes\":[false,false,false,false,false],\"id\":1},1]]}";
    
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
    public GameMenu gameMenu;


    // Mats
    public Material[] teamBuildingColors;
    public Material neutralBuildingColor;
    public Material[] teamUnitColors;

    // Terrain junk
    private List<GameObject> mountains = new List<GameObject>();

    // structure and hero info
    private Dictionary<Vector2Int, Tuple<GameObject, string>> structureData = new Dictionary<Vector2Int, Tuple<GameObject, string>>();
    public Dictionary<int, Tuple<GameObject, SimpleJSON.JSONNode>> heroes = new Dictionary<int, Tuple<GameObject, SimpleJSON.JSONNode>>();

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
        UpdateMap(SimpleJSON.JSON.Parse(json),0);
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
            if (!structureData.ContainsKey(new Vector2Int(city[0], city[1])))
            {
                // struct hasn't been init'd yet
                var v = Instantiate(townBase);
                v.transform.position = gridToWorldPosition(city[0], city[1]);
                structureData[new Vector2Int(city[0], city[1])] = new Tuple<GameObject, string>(v, city[2].ToString());
            }
            // set color
            var colorMat = (city[3] == -1) ? neutralBuildingColor : teamBuildingColors[city[3]];
            structureData[new Vector2Int(city[0], city[1])].Item1.GetComponentInChildren<MeshRenderer>().material = colorMat;

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
        foreach (SimpleJSON.JSONNode farms in node["farms"].AsArray)
        {
            // cereal, pot, bitcoin.
            if (!structureData.ContainsKey(new Vector2Int(farms[0], farms[1])))
            {
                GameObject v = null;
                switch (farms[2].Value)
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
                v.transform.position = gridToWorldPosition(farms[0], farms[1]);
                structureData[new Vector2Int(farms[0], farms[1])] = new Tuple<GameObject, string>(v, farms[2].ToString());
            }
            //set color
            var colorMat = (farms[3] == -1) ? neutralBuildingColor : teamBuildingColors[farms[3]];
            structureData[new Vector2Int(farms[0], farms[1])].Item1.GetComponentInChildren<MeshRenderer>().material = colorMat;
        }
    }

    public void renderPlayerUI(SimpleJSON.JSONNode node, int playerID)
    {
        int ctr = 0;
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
            if (structureData.ContainsKey(new Vector2Int(x, y)))
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
    void UpdateMap(SimpleJSON.JSONNode node, int playerID)
    {
        renderTiles(node, playerID);
        renderCities(node, playerID);
        renderFarms(node, playerID);
        renderPlayerUI(node, playerID);
        renderHeroes(node, playerID);
    }

   public List<Vector2Int> tilesWithinRangeOfHero(int x, int y, int dist)
   {
        Vector2Int[] directions = new Vector2Int[4] { new Vector2Int(-1, 0), new Vector2Int(1, 0), new Vector2Int(0, 1), new Vector2Int(0, -1) };
        // get the tiles within range of hero
        List<Tuple<Vector2Int,int>> queue = new List<Tuple<Vector2Int,int>>();
        queue.Add(new Tuple<Vector2Int,int>(new Vector2Int(x, y), dist));
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
        // bfs
        while (queue.Count > 0)
        {
            var v = queue[0];
            queue.RemoveAt(0);

            visited.Add(v.Item1);
            // cutoff if dist = 0.
            if (v.Item2 == 0)
            {
                continue;
            }
            foreach (var dir in directions)
            {
                var newV = v.Item1 + dir;

                if (lastState["tiles"][newV.x][newV.y] == "water" || lastState["tiles"][newV.x][newV.y] == "mountain")
                {
                    continue;
                }
                if (visited.Contains(v.Item1 + dir))
                {
                    continue;
                }
                queue.Add(new Tuple<Vector2Int, int>(newV, v.Item2 - 1));
            }
        }
        return visited.ToList();
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
                    //show it on UI
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
