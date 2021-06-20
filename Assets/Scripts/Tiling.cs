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

    // public List<Boid> boidList;
    // public Boid boid;

    private Wolf wolf;
    private Sheep sheepInst;
    private GrassGrowthAgent grassInst;

    private static int GetTile(float3 position)
    {
        return (int)(math.floor(position.x / tileSize) + (tileYMultiplier * math.floor(position.y / tileSize)));
    }

    public static List<Vector3> LoopOverTiles()
    {
        List<Vector3> TilesList = new List<Vector3>();
        float3 position;
       // var ble = grassInst.Width;
        for (var x = 0; x < 10; x++)
        {
            position.x = x;
            for (var y = 0; y < 10; y++)
            {
                position.y = y;
                position.z = 0;
                TilesList.Add(position);
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

    public List<GrassGrowthAgent> Grow(GameObject prefab) // Spaw grass quadrant
    {

        List<GrassGrowthAgent> boidsTemp = new List<GrassGrowthAgent>();
        var posList = Tiling.LoopOverTiles();
        foreach (var posL in posList)
        {
            // Vector3 pos = this.transform.position + posL;
            Quaternion rot = Quaternion.identity;

            GrassGrowthAgent newBoid = Instantiate(prefab, posL,rot).GetComponent<GrassGrowthAgent>();
            newBoid.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
            boidsTemp.Add(newBoid);
        }
        return boidsTemp;
    }

    // Start is called before the first frame update
    void Start()
    {
        wolf = Wolf.Instance;
        sheepInst = Sheep.Instance;
        grassInst = GrassGrowthAgent.Instance;

        var sheep = SpawnSheep(prefab, 60);
        // var sheepList = Converter.ConvertToSheep(boids);
        sheepInst.SetSheepList(sheep);

        var grass = Grow(prefabgrass);
        grassInst.SetGrassList(grass);
        //  sheep.SetSheepList(Converter.ConvertToSheep(SpawnBoids(prefab, 60)));

       // HashSet<GameObject> goSet = new HashSet<GameObject>();
        


    }

    
    private int GetBoidsperTile(Dictionary<int, HashSet<Boid>> tileDict, int tileKey)
    {

        return tileDict[tileKey].Count;
    }



    public void sortGotoTiles()
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
                Debug.Log("HERE");
                Debug.Log(go.GetType()); // UnityEngine.GameObject
                Debug.Log(typeof(Sheep)); // Sheep

                if (go.GetType(Sheep) == typeof(Sheep))
                {
                    Debug.Log("amihere");

                    //yaddayadda
                    List<GameObject> Vision = new List<GameObject>();
                    Vision = tileDict[tileKey].ToList();  // works // if (this != boid) // selfcheck             
                    //

                    go.GetComponent<Sheep>().Tick(Vision); 

                }
                    
            }
        }
    }

    /*
    /// <summary>
    /// get component from inst works with Boid class
    /// </summary>
    public void boidSortingStuff()
    {
        HashSet<Boid> boidsSet = new HashSet<Boid>();
        Dictionary<int, HashSet<Boid>> tileDict = new Dictionary<int, HashSet<Boid>>();

        // sorting ALL boids to tile
        foreach (var sheep in sheepInst.GetSheepList())
        {
            //sheep.Update(); -> no need to call manually 
            var boid = sheep.GetComponent<Boid>();

            // create key
            int tileKey = GetTile(boid.transform.position);

            // add boid to key in tileDict
            if (!tileDict.ContainsKey(tileKey))
            {
                tileDict.Add(tileKey, new HashSet<Boid>());
            }
            tileDict[tileKey].Add(boid);

            DebugDrawTiles(boid.transform.position);
            // Debug.Log(GetBoidsperTile(tileDict, tileKey));
        }

        // looping through tile
        foreach (var item in tileDict.Keys)
        {
            var tileKey = item;

            foreach (var boid in tileDict[tileKey])
            {
                List<Boid> Vision = new List<Boid>();
                Vision = tileDict[tileKey].ToList();  // works // if (this != boid) // selfcheck             
                boid.oldUpdate(Vision);
            }
        }
    }
    */

    // Update is called Aonce per frame
    void Update()
    {
        sortGotoTiles();
        loopOverTiles();
       // boidSortingStuff();


    }
}





/*
public class Converter
{
    /// <summary>
    ///     Converts a list of Boids (parentclass) into a List of specified type <childrenclass>
    /// </summary>

    public static List<Sheep> ConvertToSheep(List<Boid> boids)
    {
        List<Sheep> sheepTemp = new List<Sheep>();

        foreach (var boid in boids)
        {
            Debug.Log("test");

            //Sheep sheep = boid.AddComponent<Sheep>();
            // Getting properties of parent class
            PropertyInfo[] properties = typeof(Boid).GetProperties();
            
            // Copy all properties to parent class
            foreach (PropertyInfo pi in properties)
            {
                if (pi.CanWrite)
                    pi.SetValue(sheep, pi.GetValue(boid));
               // pi.SetValue(sheep, pi.GetValue(boid, null), null);
            }
            sheepTemp.Add(sheep);
        }

        return sheepTemp;

        /*
        var sheepList = new List<Sheep>();
        AutoMapper.Mapper.CreateMap<Boid, Sheep>(); // Declare that we want some automagic to happen
        foreach (var boid in boids)
        {
            var sheep = AutoMapper.Mapper.Map<Sheep>(boid);
            // At this point, the car-specific properties (Color and SteeringColumnStyle) are null, because there are no properties in the Vehicle object to map from.
            // However, car's NumWheels and HasMotor properties which exist due to inheritance, are populated by AutoMapper.
            sheepList.Add(sheep);
        }
        return sheepList;
        */
// }
//}

/*
public List<Boid> SpawnBoids(GameObject prefab, int percent)
{
    // percent = 10;

    var radius = 5;
    List<Boid> boidsTemp = new List<Boid>();

    var number = 5;
    for (int i = 0; i < number; i++)
    {

        Vector3 pos = this.transform.position + UnityEngine.Random.insideUnitSphere * radius;
        Quaternion rot = UnityEngine.Random.rotation;

        Boid newBoid = Instantiate(prefab, pos, rot).GetComponent<Boid>();
        newBoid.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
        boidsTemp.Add(newBoid);
    }
    Debug.Log(boidsTemp);

    return boidsTemp;

}

*/ 