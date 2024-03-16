using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleView : MonoBehaviour
{
    public List<GameObject> banners;
    public GameObject bannerBase;
    public GameObject leftPanel;
    public int watch;
    public BattleRender br;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    void Open(int battle)
    {
        watch = battle;
        gameObject.SetActive(true);
        br.Clear();
    }

    public void Populate(SimpleJSON.JSONNode gameState)
    {
        foreach(var b in banners)
        {
            Destroy(b);
        }
        banners.Clear();

        int cnt = 0;
        foreach (var v in gameState["battles"].AsArray)
        {
            var i = Instantiate(bannerBase);
            i.transform.SetParent(leftPanel.transform);
            i.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(-50 + (-100 * cnt), -50);
            i.transform.GetComponentInChildren<Text>().text = v.Value[4]["info"];
            int tmp = cnt;
            i.transform.GetComponent<Button>().onClick.AddListener(delegate () { Open(tmp); });
            cnt++;
            banners.Add(i);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
