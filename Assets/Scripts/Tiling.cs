using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using System.Linq;

/// <summary>
/// Managing the world (MACROLEVEL)
/// GameObjects are stored in a Dictionary 'tileDict'
/// Updates of Agents are called manually 
/// </summary>

public class Tiling : MonoBehaviour
{

    public Dictionary<int, HashSet<GameObject>> tileDict;

    private const int tileYMultiplier = 1000;
    private const int tileSize = 5;

    [SerializeField] private GameObject prefab;
    public GameObject prefabgrass;
    public GameObject prefabwolf;

    private Wolf wolfInst;
    private Sheep sheepInst;
    private GrassGrowthAgent grassInst;

    // Init -------------------------------------------------------------------------
    // Start is called before the first frame update (automatically)
    void Start()
    {
        wolfInst = Wolf.Instance;
        sheepInst = Sheep.Instance;
        grassInst = GrassGrowthAgent.Instance;

        SpawnSheep(prefab, 90);
        SpawnWolves(prefabwolf, 10);
        Grow(prefabgrass,90);

    }

    // Update------------------------------------------------------------------------
    // Update is called once per frame (automatically)
    void Update()
    {
        sortGOtoTiles();
        loopOverTiles();
    }

    // Methods-----------------------------------------------------------------------

    private static int GetTile(float3 position)
    {
        return (int)(math.floor(position.x / tileSize) + (tileYMultiplier * math.floor(position.y / tileSize)));
    }


    private int GetBoidsperTile(Dictionary<int, HashSet<Boid>> tileDict, int tileKey)
    {
        return tileDict[tileKey].Count;
    }


    // sorting all gameobjects in scene to its tile -> creating new tileDict in each scene
    public void sortGOtoTiles()
    {
        tileDict = new Dictionary<int, HashSet<GameObject>>();

        // find all GameObjects in scene 
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

        // sorting ALL GO to tile
        foreach (var go in allObjects)
        {
            // create key
            int tileKey = GetTile(go.transform.position);

            // add go to key in tileDict
            if (!tileDict.ContainsKey(tileKey))
            {
                tileDict.Add(tileKey, new HashSet<GameObject>());
            }
            tileDict[tileKey].Add(go);

            DebugDrawTiles(go.transform.position);
        }
    }

    // looping through tiles and activating update of agents
    public void loopOverTiles()
    {
        foreach (int tileKey in tileDict.Keys)
        {
            foreach (var go in tileDict[tileKey])
            {
                List<GameObject> StuffICanSee = new List<GameObject>();

                // sheep
                // get List of what Sheep can see and call update (hier -> tick()) 
                if (go.GetComponent<Sheep>())
                {
                    // get own tile and adjacent tiles -> cross style (diagonal adjacent tiles are not covered) <- TODO!!
                    StuffICanSee.AddRange(tileDict[tileKey].ToList()); // own 
                    // if adjacent tiles exist at all:
                    if (tileDict.ContainsKey(tileKey + 1))
                    {
                        StuffICanSee.AddRange(tileDict[tileKey + 1].ToList()); // tile to the right
                    }
                    if (tileDict.ContainsKey(tileKey - 1))
                    {
                        StuffICanSee.AddRange(tileDict[tileKey - 1].ToList()); // tile to the left
                    }
                    if (tileDict.ContainsKey(tileKey + tileYMultiplier))
                    {
                        StuffICanSee.AddRange(tileDict[tileKey + tileYMultiplier].ToList()); // tile above
                    }
                    if (tileDict.ContainsKey(tileKey - tileYMultiplier))
                    {
                        StuffICanSee.AddRange(tileDict[tileKey - tileYMultiplier].ToList()); // tile below
                    }

                    if (tileDict.ContainsKey(tileKey + 1 + tileYMultiplier))
                    {
                        StuffICanSee.AddRange(tileDict[tileKey + 1 + tileYMultiplier].ToList()); // tile top right
                    }
                    if (tileDict.ContainsKey(tileKey - 1 + tileYMultiplier))
                    {
                        StuffICanSee.AddRange(tileDict[tileKey - 1 + tileYMultiplier].ToList()); // tile top left
                    }

                    if (tileDict.ContainsKey(tileKey + 1 - tileYMultiplier))
                    {
                        StuffICanSee.AddRange(tileDict[tileKey + 1 - tileYMultiplier].ToList()); // tile bot right
                    }
                    if (tileDict.ContainsKey(tileKey - 1 - tileYMultiplier))
                    {
                        StuffICanSee.AddRange(tileDict[tileKey - 1 - tileYMultiplier].ToList()); // tile below
                    }



                    go.GetComponent<Sheep>().Tick(StuffICanSee);
                }
                // wolf
                // get List of what Wolf can see and call update (hier -> tick()) MANUALLY
                if (go.GetComponent<Wolf>())
                {
                    // get own tile and adjacent tiles -> cross style (diagonal adjacent tiles are not covered) <- TODO!!
                    StuffICanSee.AddRange(tileDict[tileKey].ToList()); // own 
                    // if adjacent tiles exist at all:
                    if (tileDict.ContainsKey(tileKey + 1))
                    {
                        StuffICanSee.AddRange(tileDict[tileKey + 1].ToList()); // tile to the right
                    }
                    if (tileDict.ContainsKey(tileKey - 1))
                    {
                        StuffICanSee.AddRange(tileDict[tileKey - 1].ToList()); // tile to the left
                    }
                    if (tileDict.ContainsKey(tileKey + tileYMultiplier))
                    {
                        StuffICanSee.AddRange(tileDict[tileKey + tileYMultiplier].ToList()); // tile above
                    }
                    if (tileDict.ContainsKey(tileKey - tileYMultiplier))
                    {
                        StuffICanSee.AddRange(tileDict[tileKey - tileYMultiplier].ToList()); // tile below
                    }

                    if (tileDict.ContainsKey(tileKey + 1 + tileYMultiplier))
                    {
                        StuffICanSee.AddRange(tileDict[tileKey + 1 + tileYMultiplier].ToList()); // tile top right
                    }
                    if (tileDict.ContainsKey(tileKey - 1 + tileYMultiplier))
                    {
                        StuffICanSee.AddRange(tileDict[tileKey - 1 + tileYMultiplier].ToList()); // tile top left
                    }

                    if (tileDict.ContainsKey(tileKey + 1 - tileYMultiplier))
                    {
                        StuffICanSee.AddRange(tileDict[tileKey + 1 - tileYMultiplier].ToList()); // tile bot right
                    }
                    if (tileDict.ContainsKey(tileKey - 1 - tileYMultiplier))
                    {
                        StuffICanSee.AddRange(tileDict[tileKey - 1 - tileYMultiplier].ToList()); // tile below
                    }
                    go.GetComponent<Wolf>().Tick(StuffICanSee);
                }
            }
        }
    }

    private static void DebugDrawTiles(float3 position)
    {
        Vector3 lowerLeft = new Vector3(math.floor(position.x / tileSize) * tileSize, math.floor(position.y / tileSize) * tileSize);

        Debug.DrawLine(lowerLeft, lowerLeft + new Vector3(+1, +0) * tileSize);
        Debug.DrawLine(lowerLeft, lowerLeft + new Vector3(+0, +1) * tileSize);

        Debug.DrawLine(lowerLeft + new Vector3(+1, +0) * tileSize, lowerLeft + new Vector3(+1, +1) * tileSize);
        Debug.DrawLine(lowerLeft + new Vector3(+0, +1) * tileSize, lowerLeft + new Vector3(+1, +1) * tileSize);

        // Debug.Log(GetTile(position) + " " + position);
    }

    // Methods-----------------------------------------------------------------------

    public List<Sheep> SpawnSheep(GameObject prefab, int percent)
    {
        var radius = 5;
        List<Sheep> boidsTemp = new List<Sheep>();

        for (int i = 0; i < percent/10; i++)
        {

            Vector3 pos = this.transform.position + UnityEngine.Random.insideUnitSphere * radius;
            Quaternion rot = UnityEngine.Random.rotation;

            Sheep newBoid = Instantiate(prefab, pos, rot).GetComponent<Sheep>();
            newBoid.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
            boidsTemp.Add(newBoid);
        }
       // Debug.Log(boidsTemp);

        return boidsTemp;

    }

    public List<Wolf> SpawnWolves(GameObject prefab, int percent)
    {
        var radius = 5;
        List<Wolf> boidsTemp = new List<Wolf>();
        Color newColor = new Color(114, 112, 114,1);

        for (int i = 0; i < percent / 10; i++)
        {

            Vector3 pos = this.transform.position + UnityEngine.Random.insideUnitSphere * radius;
            Quaternion rot = UnityEngine.Random.rotation;

            Wolf newBoid = Instantiate(prefab, pos, rot).GetComponent<Wolf>();
            boidsTemp.Add(newBoid);
        }
       // Debug.Log(boidsTemp);

        return boidsTemp;

    }

    public List<GrassGrowthAgent> Grow(GameObject prefab, int percent) // Spaw grass quadrant
    {
        List<GrassGrowthAgent> boidsTemp = new List<GrassGrowthAgent>();
        var radius = 20;
        Color newColor = new Color(79, 178, 134, 1);
        for (int i = 0; i < percent *20/ 2.5; i++)
        {
            Vector3 pos = this.transform.position + UnityEngine.Random.insideUnitSphere * radius;
            pos.z = 0;
            GrassGrowthAgent newBoid = Instantiate(prefab, pos, Quaternion.identity).GetComponent<GrassGrowthAgent>();
         //   newBoid.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
            boidsTemp.Add(newBoid);
        }
        return boidsTemp;
    }


}



