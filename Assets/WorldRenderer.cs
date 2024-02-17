using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldRenderer : MonoBehaviour
{
    string json = "{\"tiles\":[[\"open\",\"open\",\"open\",\"open\",\"water\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"water\",\"water\",\"open\",\"open\",\"open\",\"open\"],[\"open\",\"open\",\"open\",\"open\",\"water\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\"],[\"open\",\"open\",\"open\",\"water\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"water\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\"],[\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\"],[\"open\",\"open\",\"open\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"water\",\"open\",\"open\",\"mountain\",\"open\",\"open\",\"open\"],[\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"mountain\",\"open\",\"open\",\"mountain\"],[\"open\",\"mountain\",\"open\",\"water\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"water\",\"mountain\",\"open\",\"open\",\"open\",\"open\",\"open\"],[\"open\",\"open\",\"open\",\"open\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\"],[\"open\",\"open\",\"open\",\"open\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\",\"mountain\"],[\"open\",\"open\",\"open\",\"open\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\",\"mountain\"],[\"open\",\"open\",\"open\",\"open\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\"],[\"open\",\"mountain\",\"open\",\"water\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"water\",\"mountain\",\"open\",\"open\",\"open\",\"open\",\"open\"],[\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"mountain\",\"open\",\"open\",\"mountain\"],[\"open\",\"open\",\"open\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"water\",\"open\",\"open\",\"mountain\",\"open\",\"open\",\"open\"],[\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\"],[\"open\",\"open\",\"open\",\"water\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"water\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\"],[\"open\",\"open\",\"open\",\"open\",\"water\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\"],[\"open\",\"open\",\"open\",\"open\",\"water\",\"water\",\"open\",\"open\",\"open\",\"open\",\"open\",\"open\",\"water\",\"water\",\"open\",\"open\",\"open\",\"open\"]],\"cities\":[[1,3,{\"built\":true,\"bitcoin_level\":3,\"defense_level\":3,\"meme_level\":0,\"unit_unlocks\":[true,false,false,false,false],\"units_available\":[0,0,0,0,0],\"units_garrisoned\":[0,0,0,0,0]}],[6,10,{\"built\":false,\"bitcoin_level\":1,\"defense_level\":0,\"meme_level\":0,\"unit_unlocks\":[true,false,false,false,false],\"units_available\":[0,0,0,0,0],\"units_garrisoned\":[0,0,0,0,0]}],[16,3,{\"built\":false,\"bitcoin_level\":1,\"defense_level\":0,\"meme_level\":0,\"unit_unlocks\":[true,false,false,false,false],\"units_available\":[0,0,0,0,0],\"units_garrisoned\":[0,0,0,0,0]}],[11,10,{\"built\":false,\"bitcoin_level\":1,\"defense_level\":0,\"meme_level\":0,\"unit_unlocks\":[true,false,false,false,false],\"units_available\":[0,0,0,0,0],\"units_garrisoned\":[0,0,0,0,0]}]]}";
    public Tilemap tilemap;
    public Tilemap grid;
    public Tile grass;
    public Tile water;
    public Tile gridTile;

    public GameObject townBase;
    public GameObject cerealBase;
    public GameObject weedBase;
    public GameObject bitcoinBase;

    // keep the city data here
    private Dictionary<Vector2Int, Tuple<GameObject, string>> structureData = new Dictionary<Vector2Int, Tuple<GameObject, string>>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMap(SimpleJSON.JSON.Parse(json));
    }

    void UpdateMap(SimpleJSON.JSONNode node)
    {
        grid.ClearAllTiles();
        tilemap.ClearAllTiles();
        foreach(var v in structureData.Values)
        {
            Destroy(v.Item1);
        }
        structureData.Clear();

        for(int x = 0; x < node["tiles"].Count; x++)
        {
            for(int y = 0; y < node["tiles"][x].Count; y++)
            {
                string tile = node["tiles"][x][y];
                if(tile == "open")
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), grass);
                    //Instantiate open tile
                }
                else if(tile == "water")
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), water);
                    //Instantiate water tile
                }
                else if(tile == "mountain")
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), null);
                    //Instantiate mountain tile (later)
                }
            }
        }
        foreach(SimpleJSON.JSONNode city in node["cities"].AsArray)
        {
            var v = Instantiate(townBase);
            v.transform.position = new Vector3(city[0], 0, city[1]);

            structureData[new Vector2Int(city[0], city[1])] = new Tuple<GameObject, string>(v,city[2].ToString());
        }
    }
}
