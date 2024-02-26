using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class GameCursor : MonoBehaviour
{
    public Camera gameCamera;
    public GameObject heroInfoPopup;
    public GameObject cityInfoPopup;
    public RectTransform canvas;
    public WorldRenderer worldRenderer;
    
    public SimpleJSON.JSONNode selectedUnit;
    public SimpleJSON.JSONNode selectedCity;


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
        moveCamera();
        // see what mouse is over
        heroInfoPopup.SetActive(false);
        RaycastHit hit;
        Ray ray = gameCamera.ScreenPointToRay(Input.mousePosition);

        GameObject heroUnderCursor = null;
        if (Physics.Raycast(ray, out hit))
        {
            // convert collision point to grid.
            Vector3 point = hit.point / WorldRenderer.TILE_SIZE;
            // Do something with the object that was hit by the raycast.
            showHeroPopup((int)(point.x),(int)(point.z));
        }

        // check for mouse click on hero
        if(selectedUnit != null)
        {
            worldRenderer.setGridTiles(worldRenderer.tilesWithinRangeOfHero(selectedUnit[0], selectedUnit[1], selectedUnit[2]["move_points"]));
        }

    }

    public void showHeroPopup(int x, int y)
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
                    selectedUnit = hero.Item2;
                }
                return;
            }
        }
    }


}
