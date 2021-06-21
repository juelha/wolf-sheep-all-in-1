using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using System.Linq;

// for casting parent class to child class:
using System.Reflection;
//using AutoMapper;



public class Tiling : MonoBehaviour
{
    public static Tiling TilingInstance;
    public Dictionary<int, HashSet<GameObject>> tileDict;


    private const int tileYMultiplier = 1000;
    private const int tileSize = 5;
    [SerializeField] private GameObject prefab;
    public GameObject prefabgrass;
    public GameObject prefabwolf;

    // public List<Boid> boidList;
    // public Boid boid;

    private Wolf wolfInst;
    private Sheep sheepInst;
    private GrassGrowthAgent grassInst;

    private static int GetTile(float3 position)
    {
        return (int)(math.floor(position.x / tileSize) + (tileYMultiplier * math.floor(position.y / tileSize)));
    }

    public static List<Vector3> LoopOverPos()
    {
        List<Vector3> TilesList = new List<Vector3>();
        float3 position;
        float3 position2;
        float3 position3;
        float3 position4;
        // var ble = grassInst.Width;
        for (var x = 0; x < 10; x++)
        {
            position.x = x;
            position2.x = -x;
            position3.x = x;
            position4.x = -x;
            for (var y = 0; y < 10; y++)
            {

                position.y = y;
                position2.y = y;
                position3.y = -y;
                position4.y = -y;

                position.z = 0;
                position2.z = 0;
                position3.z = 0;
                position4.z = 0;

                TilesList.Add(position);
                TilesList.Add(position2);
                TilesList.Add(position3);
                TilesList.Add(position4);
            }
        }
        return TilesList;
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

    private void Awake()
    {

        TilingInstance = this;
    }



    public List<Sheep> SpawnSheep(GameObject prefab, int percent)
    {
       // percent = 10;
        
        var radius = 5;
        List<Sheep> boidsTemp = new List<Sheep>();

        var number = 5;
        for (int i = 0; i < number; i++)
        {

            Vector3 pos = this.transform.position + UnityEngine.Random.insideUnitSphere * radius;
            Quaternion rot = UnityEngine.Random.rotation;

            Sheep newBoid = Instantiate(prefab, pos, rot).GetComponent<Sheep>();
            newBoid.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
            boidsTemp.Add(newBoid);
        }
        Debug.Log(boidsTemp);

        return boidsTemp;

    }



    public List<Wolf> SpawnWolves(GameObject prefab, int percent)
    {
        // percent = 10;

        var radius = 5;
        List<Wolf> boidsTemp = new List<Wolf>();

        var number = 5;
        for (int i = 0; i < number; i++)
        {

            Vector3 pos = this.transform.position + UnityEngine.Random.insideUnitSphere * radius;
            Quaternion rot = UnityEngine.Random.rotation;

            Wolf newBoid = Instantiate(prefab, pos, rot).GetComponent<Wolf>();
            newBoid.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
            boidsTemp.Add(newBoid);
        }
        Debug.Log(boidsTemp);

        return boidsTemp;

    }

    public List<GrassGrowthAgent> Grow(GameObject prefab) // Spaw grass quadrant
    {
        List<GrassGrowthAgent> boidsTemp = new List<GrassGrowthAgent>();
        var posList = Tiling.LoopOverPos();
        foreach (var posL in posList)
        {
            GrassGrowthAgent newBoid = Instantiate(prefab, posL, Quaternion.identity).GetComponent<GrassGrowthAgent>();
            newBoid.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
            boidsTemp.Add(newBoid);
        }
        return boidsTemp;
    }

    // Start is called before the first frame update
    void Start()
    {
        wolfInst = Wolf.Instance;
        sheepInst = Sheep.Instance;
        grassInst = GrassGrowthAgent.Instance;

        sheepInst.SetSheepList(SpawnSheep(prefab, 60));
        wolfInst.SetWolfList(SpawnWolves(prefabwolf, 60));
        grassInst.SetGrassList(Grow(prefabgrass));

    }

    
    private int GetBoidsperTile(Dictionary<int, HashSet<Boid>> tileDict, int tileKey)
    {
        return tileDict[tileKey].Count;
    }



    public void sortGOtoTiles()
    {
        Debug.Log(tileDict);
        tileDict = new Dictionary<int, HashSet<GameObject>>();


        // find all GameObjects in scene an add to list 
        List<GameObject> goAllList = new List<GameObject>();
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (var go in allObjects)
        {
            goAllList.Add(go);
        }

        // sorting ALL boids to tile
        foreach (var go in goAllList)
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
            // Debug.Log(GetBoidsperTile(tileDict, tileKey));
        }

    }

    // looping through tiles and activate update of agents
    public void loopOverTiles()
    {
        foreach (int tileKey in tileDict.Keys)
        {
            foreach (var go in tileDict[tileKey])
            {
                // sheep
                // get List of what Sheep can see and call update (hier -> tick()) manually
                if (go.GetComponent<Sheep>())
                {        
                    go.GetComponent<Sheep>().Tick(tileDict[tileKey].ToList()); 
                }
                // wolf
                // get List of what Wolf can see and call update (hier -> tick()) manually
                if (go.GetComponent<Wolf>())
                {
                    go.GetComponent<Wolf>().Tick(tileDict[tileKey].ToList());
                }
            }
        }
    }



    // Update is called Aonce per frame
    void Update()
    {
        sortGOtoTiles();
        loopOverTiles();
    }
}


