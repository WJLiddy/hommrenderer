using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using static UnityEngine.GraphicsBuffer;

public class GameClient : MonoBehaviour
{
    public float timer = 1f;
    public SimpleJSON.JSONArray commandQueue;
    public WorldRenderer worldRenderer;

    void Start()
    {
        commandQueue = SimpleJSON.JSONObject.Parse("[]").AsArray; // fuck
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            if (commandQueue.Count > 0)
            {
                Debug.Log("sending command " + commandQueue.ToString());
                StartCoroutine(postRequest("localhost:7775", commandQueue.ToString()));
                // dispatched the command, clear queue
                commandQueue = SimpleJSON.JSONObject.Parse("[]").AsArray; // fuck
            }
            else
            {
                StartCoroutine(postRequest("localhost:7775", ""));
            }
            timer = 1f;
        }
    }

    IEnumerator postRequest(string url, string json)
    {
        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError)
            {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
        }

        worldRenderer.UpdateMap(SimpleJSON.JSON.Parse(uwr.downloadHandler.text), 0);
    }

    private JSONArray makeVector2(Vector2Int vector)
    {
        JSONArray jsonArray = new JSONArray();
        jsonArray.Add(vector.x);
        jsonArray.Add(vector.y);
        return jsonArray;
    }

    public void addMoveCommandToQueue(Vector2Int target, Vector2Int delta)
    {
        SimpleJSON.JSONNode command = SimpleJSON.JSON.Parse("{}");
        command["command"] = "move";
        command["player"] = 0;
        command["target"] = makeVector2(target);
        command["delta"] = makeVector2(delta);
        commandQueue.Add(command);
    }

    public void addEndTurnCommandToQueue()
    {
        SimpleJSON.JSONNode command = SimpleJSON.JSON.Parse("{}");
        command["command"] = "endturn";
        command["player"] = 0;
        commandQueue.Add(command);
    }

    public void addBuildCommandToQueue(Vector2Int target, string build)
    {
        SimpleJSON.JSONNode command = SimpleJSON.JSON.Parse("{}");
        command["command"] = "build";
        command["player"] = 0;
        command["target"] = makeVector2(target);
        command["build"] = build;
        commandQueue.Add(command);
    }

    // cut param since we just spam click button..
    public void addBuyCommandToQueue(Vector2Int target, string buy)
    {
        SimpleJSON.JSONNode command = SimpleJSON.JSON.Parse("{}");
        command["command"] = "buy";
        command["player"] = 0;
        command["target"] = makeVector2(target);
        command["buy"] = buy;
        commandQueue.Add(command);
    }

    public void addTransferCommandFromCityToHero(Vector2Int target, int typeID)
    {
        SimpleJSON.JSONNode command = SimpleJSON.JSON.Parse("{}");
        command["command"] = "transfer";
        command["player"] = 0;
        command["src"] = makeVector2(target);
        command["dest"] = makeVector2(target);
        command["srctype"] = "city";
        command["desttype"] = "hero";
        command["type"] = typeID;
        commandQueue.Add(command);
    }

    public void addTransferCommandFromHeroToCity(Vector2Int target, int typeID)
    {
        SimpleJSON.JSONNode command = SimpleJSON.JSON.Parse("{}");
        command["command"] = "transfer";
        command["player"] = 0;
        command["src"] = makeVector2(target);
        command["dest"] = makeVector2(target);
        command["srctype"] = "hero";
        command["desttype"] = "city";
        command["type"] = typeID;
        commandQueue.Add(command);
    }

}
