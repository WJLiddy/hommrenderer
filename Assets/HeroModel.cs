using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroModel : MonoBehaviour
{
    // determines speed. (1-3)
    public GameObject[] horseOptions;
    // determines attack. (1-3)
    public GameObject[] weaponOptions;
    // determines defense. (1-3)
    public GameObject[] bodyOptions;
    public GameObject[] headOptions;
    public GameObject[] shieldOptions;
    public string[] names;
    public int ID;

    public void SetInArray(int index, GameObject[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (i == index)
            {
                array[i].SetActive(true);
            }
            else
            {
                array[i].SetActive(false);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        makeHeroFromID();
    }

    // get all permutations of a list
    public List<List<int>> GetPermutations(List<int> list)
    {
        List<List<int>> result = new List<List<int>>();
        if (list.Count == 1)
        {
            result.Add(list);
            return result;
        }
        for (int i = 0; i < list.Count; i++)
        {
            List<int> copy = new List<int>(list);
            copy.RemoveAt(i);
            List<List<int>> subResult = GetPermutations(copy);
            foreach (List<int> subList in subResult)
            {
                subList.Add(list[i]);
                result.Add(subList);
            }
        }
        return result;
    }

    public void makeHeroFromID()
    {
        // slow and could be done and filthy but i'm in a hurry.
        List<int> atkOffsets = new List<int>() { 0, 0, 0 };
        List<int> defOffsets = new List<int>() { 0, 0, 0 };
        int IDCTR = ID;

        while (true)
        {
            // generate every perm
            foreach (var v in GetPermutations(new List<int> { 0, 1, 2 }))
            {
                // get raw stats
                int atk = v[0];
                int spd = v[1];
                int def = v[2];

                // if this is the perm
                if (IDCTR == 0)
                {
                    Debug.Log("ID " + names[ID] + "\nPOWER " + (1 + atk) + "\nSPEED " + (1 + spd) + "\nTOUGHNESS " + (1 + def));
                    // horseset
                    SetInArray(spd, horseOptions);

                    // armorset
                    SetInArray((def * 6 + defOffsets[def]), bodyOptions);
                    SetInArray((def * 6 + defOffsets[def]), headOptions);
                    SetInArray((def * 6 + defOffsets[def]), shieldOptions);

                    // weaponset
                    SetInArray((atk * 6 + atkOffsets[atk]), weaponOptions);

                    return;
                }
                // not the perm. cycle through each speed, def, and attack position.
                defOffsets[def] += 1;
                atkOffsets[atk] += 1;
                IDCTR--;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
    }
}
