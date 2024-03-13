using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CityInfoPanel : MonoBehaviour
{
    public GameObject heroPanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void setBuildingData(JSONObject city, int i, string buyButton, string unitButton)
    {
        // set unit_unlocks, units_available, units_garrisoned first
        transform.Find("Building").Find(buyButton).gameObject.SetActive(!city["unit_unlocks"][i]);
        transform.Find("Garrison").Find(unitButton).gameObject.SetActive(city["unit_unlocks"][i]);
    }

    public void setCityData(SimpleJSON.JSONNode cityNode, SimpleJSON.JSONNode visitingHero)
    {
        Debug.Log(cityNode);
        JSONObject city = cityNode[2].AsObject;
        setBuildingData(city, 0, "BuyBarrack", "BuyInf");
        setBuildingData(city, 1, "BuyRange", "BuyArc");
        setBuildingData(city, 2, "BuyStables", "BuyCav");
        setBuildingData(city, 3, "BuyWorkshop", "BuyBal");
        setBuildingData(city, 4, "BuySchool", "BuyWiz");

        // set amount of each in garrison
        var garrison = "";
        garrison += city["units_garrisoned"][0] + "(" + city["units_available"][0] + " avail)\n";
        garrison += city["units_garrisoned"][1] + "(" + city["units_available"][1] + " avail)\n";
        garrison += city["units_garrisoned"][2] + "(" + city["units_available"][2] + " avail)\n";
        garrison += city["units_garrisoned"][3] + "(" + city["units_available"][3] + " avail)\n";
        garrison += city["units_garrisoned"][4] + "(" + city["units_available"][4] + " avail)\n";
        transform.Find("Garrison").Find("Desc").GetComponent<Text>().text = garrison;

        // if there's a visiting hero, set that
        heroPanel.SetActive(visitingHero != null);
        if (visitingHero != null)
        {
            heroPanel.transform.Find("InfStack").Find("Count").GetComponent<Text>().text = visitingHero[2]["unit_stacks"][0].ToString();
            heroPanel.transform.Find("ArcStack").Find("Count").GetComponent<Text>().text = visitingHero[2]["unit_stacks"][1].ToString();
            heroPanel.transform.Find("CavStack").Find("Count").GetComponent<Text>().text = visitingHero[2]["unit_stacks"][2].ToString();
            heroPanel.transform.Find("BalStack").Find("Count").GetComponent<Text>().text = visitingHero[2]["unit_stacks"][3].ToString();
            heroPanel.transform.Find("WizStack").Find("Count").GetComponent<Text>().text = visitingHero[2]["unit_stacks"][4].ToString();
        }
    }
}
