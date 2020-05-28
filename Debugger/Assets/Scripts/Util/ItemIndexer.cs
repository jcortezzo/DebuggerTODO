using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemIndexer : MonoBehaviour
{
    public static ItemIndexer Instance;
    public const int BASE_ITEMS = 5;
    private const string RESOURCES_PATH = "Prefabs/Weapons/";
    private Dictionary<int, string> WeaponIndex = new Dictionary<int, string>
    {
        {-1 , "Empty" },
        { 0 , "Cutlass" },
        { 1 , "Flintlock" },
        { 2 , "Keyboard" },
        { 3 , "Step" },
        
    };
    private Dictionary<string, int> NameToIndex = new Dictionary<string, int>();

    private List<string> BaseWeapons = new List<string>
    {
        "Cutlass",
        "Flintlock",
        "Keyboard",
        "Step",
    };

    private Dictionary<(string, string), string> Combinations = new Dictionary<(string, string), string>
    {

    };

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public int GetRandomBaseWeaponID()
    {
        string weapon = BaseWeapons[Random.Range(0, BaseWeapons.Count)];
        return NameToIndex[weapon];
    }

    public int GetRandomWeaponID()
    {
        return Random.Range(0, WeaponIndex.Count - 1);
    }

    public GameObject InstantiateBaseWeapon()
    {
        string weapon = BaseWeapons[Random.Range(0, BaseWeapons.Count)];
        GameObject wp = Instantiate(Resources.Load(RESOURCES_PATH + weapon, typeof(GameObject))) as GameObject;
        return wp;
    }

    public GameObject InstantiateWeapon(int id)
    {
        if (id == -1) return null;
        GameObject wp = Instantiate(Resources.Load(RESOURCES_PATH + WeaponIndex[id], typeof(GameObject))) as GameObject;
        return wp;
    }

    public GameObject InstantiateCombination(int currID, int nextID)
    {
        string curr = WeaponIndex[currID];
        string next = WeaponIndex[nextID];
        GameObject wp;
        if (curr == null || curr.Equals("Empty") || !Combinable(currID, nextID))
        {
            wp = InstantiateWeapon(nextID);
        }
        else
        {
            wp = Instantiate(Resources.Load(RESOURCES_PATH + Combinations[(curr, next)], typeof(GameObject))) as GameObject;
        }
        return wp;
    }

    public bool IsBaseWeapon(int id)
    {
        return BaseWeapons.Contains(GetName(id));
    }

    public bool IsBaseWeapon(string id)
    {
        return BaseWeapons.Contains(id);
    }

    public bool Combinable(int id1, int id2)
    {
        return Combinations.ContainsKey((GetName(id1), GetName(id2)));
    }

    public bool Combinable(string id1, string id2)
    {
        return Combinations.ContainsKey((id1, id2));
    }

    public string GetName(int i)
    {
        if (!WeaponIndex.ContainsKey(i))
        {
            return "Empty";
        }
        return WeaponIndex[i];
    }

    public int GetIndex(string s)
    {
        if (!NameToIndex.ContainsKey(s))
        {
            return -1;
        }
        return NameToIndex[s];
    }

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        foreach (int i in WeaponIndex.Keys)
        {
            NameToIndex.Add(WeaponIndex[i], i);
        }
    }
}