using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class GameClient : MonoBehaviour
{
    public float timer = 1f;
    public SimpleJSON.JSONArray commandQueue;
    public WorldRenderer worldRenderer;

    public int PLAYER_ID = -1;
    public string pname = "";
    public int buttonCol = 0;
    public InputField t;
    public Text buttonName;

    public GameObject[] showOnLaunch;
    public GameObject[] showOnSetup;

    void Start()
    {
        commandQueue = SimpleJSON.JSONObject.Parse("[]").AsArray; // fuck
        foreach(var v in showOnLaunch)
        {
            v.SetActive(false);
        }
        foreach(var v in showOnSetup)
        {
            v.SetActive(true);
        }
    }

    public void Join()
    {
        if(t.text == "")
        {
            return;
        }

        pname = t.text;
        PLAYER_ID = buttonCol;

        foreach (var v in showOnLaunch)
        {
            v.SetActive(true);
        }
        foreach (var v in showOnSetup)
        {
            v.SetActive(false);
        }
    }

    public void incrCol()
    {
        buttonCol += 1;
        if(buttonCol == 6)
        {
            buttonCol = 0;
        }
        switch(buttonCol)
        {
            case 0: buttonName.text = "BLUE"; break;
            case 1: buttonName.text = "RED"; break;
            case 2: buttonName.text = "GREEN"; break;
            case 3: buttonName.text = "YELLOW"; break;
            case 4: buttonName.text = "PURPLE"; break;
            case 5: buttonName.text = "ORANGE"; break;
        }

    }

    void Update()
    {
        if(PLAYER_ID == -1)
        {
            // do nothing until set
            return;
        }

        timer -= Time.deltaTime;

        if (timer < 0)
        {
            if (commandQueue.Count > 0)
            {
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

        worldRenderer.UpdateMap(SimpleJSON.JSON.Parse(uwr.downloadHandler.text), PLAYER_ID);
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
        command["player"] = PLAYER_ID;
        command["target"] = makeVector2(target);
        command["delta"] = makeVector2(delta);
        // move command comes with name FIXME
        command["name"] = pname;
        commandQueue.Add(command);
    }

    public void addEndTurnCommandToQueue()
    {
        SimpleJSON.JSONNode command = SimpleJSON.JSON.Parse("{}");
        command["command"] = "endturn";
        command["player"] = PLAYER_ID;
        commandQueue.Add(command);
    }

    public void addBuildCommandToQueue(Vector2Int target, string build)
    {
        SimpleJSON.JSONNode command = SimpleJSON.JSON.Parse("{}");
        command["command"] = "build";
        command["player"] = PLAYER_ID;
        command["target"] = makeVector2(target);
        command["build"] = build;
        commandQueue.Add(command);
    }

    // cut param since we just spam click button..
    public void addBuyCommandToQueue(Vector2Int target, string buy)
    {
        SimpleJSON.JSONNode command = SimpleJSON.JSON.Parse("{}");
        command["command"] = "buy";
        command["player"] = PLAYER_ID;
        command["target"] = makeVector2(target);
        command["buy"] = buy;
        commandQueue.Add(command);
    }

    public void addTransferCommandFromCityToHero(Vector2Int target, int typeID)
    {
        SimpleJSON.JSONNode command = SimpleJSON.JSON.Parse("{}");
        command["command"] = "transfer";
        command["player"] = PLAYER_ID;
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
        command["player"] = PLAYER_ID;
        command["src"] = makeVector2(target);
        command["dest"] = makeVector2(target);
        command["srctype"] = "hero";
        command["desttype"] = "city";
        command["type"] = typeID;
        commandQueue.Add(command);
    }

}
