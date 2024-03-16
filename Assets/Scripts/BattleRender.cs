using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRender : MonoBehaviour
{
    public GameObject[] units;
    public WorldRenderer wr; //get team unit colors

    public Dictionary<int,GameObject> rendered;
    public Dictionary<int, Vector3> moveTargets;

    ///!!! MUST MATCH the server 
    public readonly int WIDTH = 25;
    public readonly int HEIGHT = 10;

    public void Start()
    {
        rendered = new Dictionary<int, GameObject>();
        moveTargets = new Dictionary<int, Vector3>();

    }

    public void Update()
    {
        // move
        foreach(var v in rendered.Keys)
        {
            rendered[v].transform.localPosition = Vector3.MoveTowards(rendered[v].transform.localPosition, moveTargets[v],  2 * Time.deltaTime);
        }
    }

    public void Render(SimpleJSON.JSONNode node)
    {
        HashSet<int> seen = new HashSet<int>();
        for(int x = 0; x < WIDTH; x++)
        {
            for(int y = 0; y < HEIGHT; y++)
            {
                if(node["arena"][x][y].IsObject)
                {

                    int id = node["arena"][x][y]["id"];
                    seen.Add(id);
                    int team = node["arena"][x][y]["team"];
                    int tier = node["arena"][x][y]["tier"];
                    int hp = node["arena"][x][y]["hp"];

                    if (!rendered.ContainsKey(id))
                    {
                        GameObject go = Instantiate(units[tier], Vector2.zero, Quaternion.identity);
                        go.transform.SetParent(this.transform);
                        go.transform.localPosition = new Vector3(x * 1.5f, 0, y * 1.5f);
                        go.transform.eulerAngles = new Vector3(0, team == 0 ? 90 : -90, 0);
                        foreach (var v in go.GetComponentsInChildren<SkinnedMeshRenderer>())
                        {
                            v.material = wr.teamUnitColors[team];
                        }
                        rendered[id] = (go);
                    }

                    rendered[id].GetComponent<Animator>().SetBool("walk", false);
                    if (moveTargets.ContainsKey(id) && moveTargets[id] != new Vector3(x * 1.5f, 0, y * 1.5f))
                    {
                        rendered[id].GetComponent<Animator>().SetBool("walk",true);
                    }
                    moveTargets[id] = new Vector3(x * 1.5f, 0, y * 1.5f);
                }
            }
        }

        HashSet<int> toRemove = new HashSet<int>();

        // clear this character, it died..
        foreach(var v in rendered)
        {
            if(!seen.Contains(v.Key))
            {
                Destroy(v.Value);
                moveTargets.Remove(v.Key);
                toRemove.Add(v.Key);
            }
        }

        foreach(var v in toRemove)
        {
            rendered.Remove(v);
        }

        foreach (var v in node["attacks"])
        {
            // fire attack anim.
            rendered[v.Value[0]].GetComponent<Animator>().SetTrigger("attack");
        }
    }
}
