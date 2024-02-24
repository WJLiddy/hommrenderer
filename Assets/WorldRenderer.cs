using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class WorldRenderer : MonoBehaviour
{
    string json = "{\"tiles\":[[\"open\",\"open\",\"open\",\"open\",\"water\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"water\",\"water\",\"open\",\"open\",\"open\",\"open\"],[\"open\",\"open\",\"open\",\"open\",\"water\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\"],[\"open\",\"open\",\"open\",\"water\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"water\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\"],[\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\"],[\"open\",\"open\",\"open\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"water\",\"open\",\"open\",\"mountain\",\"open\",\"open\",\"open\"],[\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"mountain\",\"open\",\"open\",\"mountain\"],[\"open\",\"mountain\",\"open\",\"water\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"water\",\"mountain\",\"open\",\"open\",\"open\",\"open\",\"open\"],[\"open\",\"open\",\"open\",\"open\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\"],[\"open\",\"open\",\"open\",\"open\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\",\"mountain\"],[\"open\",\"open\",\"open\",\"open\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\",\"mountain\"],[\"open\",\"open\",\"open\",\"open\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\"],[\"open\",\"mountain\",\"open\",\"water\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"water\",\"mountain\",\"open\",\"open\",\"open\",\"open\",\"open\"],[\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"mountain\",\"open\",\"open\",\"mountain\"],[\"open\",\"open\",\"open\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"water\",\"open\",\"open\",\"mountain\",\"open\",\"open\",\"open\"],[\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\"],[\"open\",\"open\",\"open\",\"water\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"water\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\"],[\"open\",\"open\",\"open\",\"open\",\"water\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\"],[\"open\",\"open\",\"open\",\"open\",\"water\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"water\",\"water\",\"open\",\"open\",\"open\",\"open\"]],\"cities\":[[1,3,{\"built\":true,\"bitcoin_level\":3,\"defense_level\":3,\"meme_level\":0,\"unit_unlocks\":[true,false,false,false,false],\"units_available\":[0,0,0,0,0],\"units_garrisoned\":[0,0,0,0,0]},0],[6,10,{\"built\":false,\"bitcoin_level\":1,\"defense_level\":0,\"meme_level\":0,\"unit_unlocks\":[true,false,false,false,false],\"units_available\":[0,0,0,0,0],\"units_garrisoned\":[0,0,0,0,0]},1],[16,3,{\"built\":false,\"bitcoin_level\":1,\"defense_level\":0,\"meme_level\":0,\"unit_unlocks\":[true,false,false,false,false],\"units_available\":[0,0,0,0,0],\"units_garrisoned\":[0,0,0,0,0]},-1],[11,10,{\"built\":false,\"bitcoin_level\":1,\"defense_level\":0,\"meme_level\":0,\"unit_unlocks\":[true,false,false,false,false],\"units_available\":[0,0,0,0,0],\"units_garrisoned\":[0,0,0,0,0]},-1]],\"farms\":[[6,16,\"bitcoin\",-1],[6,14,\"bitcoin\",-1],[8,5,\"bitcoin\",0],[0,7,\"pot\",-1],[2,10,\"pot\",-1],[8,12,\"cereal\",-1],[7,7,\"cereal\",0],[11,16,\"bitcoin\",-1],[11,14,\"bitcoin\",-1],[9,5,\"bitcoin\",0],[17,7,\"pot\",-1],[15,10,\"pot\",-1],[9,12,\"cereal\",-1],[10,7,\"cereal\",-1]],\"players\":[{\"bitcoin\":420,\"pot\":0,\"cereal\":2,\"team\":0,\"name\":\"p0\"},{\"bitcoin\":120,\"pot\":0,\"cereal\":0,\"team\":1,\"name\":\"p1\"}],\"heroes\":[[7,5,{\"unit_stacks\":[5,0,0,0,0],\"move_points\":7,\"move_stat\":1,\"health_stat\":2,\"attack_stat\":3,\"memes\":[false,false,false,false,false],\"id\":0},0],[6,10,{\"unit_stacks\":[5,0,0,0,0],\"move_points\":7,\"move_stat\":1,\"health_stat\":2,\"attack_stat\":3,\"memes\":[false,false,false,false,false],\"id\":1},1]]}";
    public Tilemap tilemap;
    public Tilemap grid;
    public Tile grass;
    public Tile water;
    public Tile gridTile;

    public GameObject townBase;
    public GameObject cerealBase;
    public GameObject weedBase;
    public GameObject bitcoinBase;
    public GameObject mountainBase;
    public GameObject heroBase;

    public Text BTCVal;
    public Text WeedVal;
    public Text CerealVal;

    private float TILE_SIZE = (5 * 0.64f);

    public Material[] teamBuildingColors;
    public Material neutralBuildingColor;

    public Material[] teamUnitColors;

    // keep the city data here
    private Dictionary<Vector2Int, Tuple<GameObject, string>> structureData = new Dictionary<Vector2Int, Tuple<GameObject, string>>();
    private List<GameObject> tiles = new List<GameObject>();
    private Dictionary<int, GameObject> heroes = new Dictionary<int, GameObject>();

    public GameMenu gameMenu;

    // update tiles
    private bool tilesDirty = true;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateMap(SimpleJSON.JSON.Parse(json),0);
    }

    private Vector3 gridToWorldPosition(int x, int y)
    {
        return new Vector3((TILE_SIZE * 0.5f) + x * TILE_SIZE, 0, (TILE_SIZE * 0.5f) + y * TILE_SIZE);
    }

    public void renderTiles(SimpleJSON.JSONNode node, int playerID)
    {
        if (tilesDirty)
        {
            tilemap.ClearAllTiles();
            grid.ClearAllTiles();

            for (int x = 0; x < node["tiles"].Count; x++)
            {
                for (int y = 0; y < node["tiles"][x].Count; y++)
                {
                    grid.SetTile(new Vector3Int(x, (node["tiles"].Count - 1) - y, 0), gridTile);
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
                        tiles.Add(v);
                        v.transform.position = gridToWorldPosition(x, y);
                        //Instantiate mountain tile (later)
                    }
                }
            }
            tilesDirty = false;
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
                gameMenu.heroButtons[cityUIIdx].GetComponentInChildren<Text>().text = cityName;
                cityUIIdx++;
            }
            cityidx++;
        }
        // clear rest of UI
        for (int i = cityUIIdx; i < 3; ++i)
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
                heroes[id] = heroModel;
            }
            heroes[id].transform.position = gridToWorldPosition(x, y);
            // boost hero up if in a town
            if (structureData.ContainsKey(new Vector2Int(x, y)))
            {
                heroes[id].transform.position += new Vector3(0, 0.5f, -1.5f);
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
}
