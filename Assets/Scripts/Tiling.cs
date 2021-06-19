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
    private const int tileYMultiplier = 1000;
    private const int tileSize = 5;
    [SerializeField] private GameObject prefab;

    // public List<Boid> boidList;
    // public Boid boid;

    private Wolf wolf;
    private Sheep sheepInst;
    private GrassGrowthAgent grass;

    private static int GetPosHashMapKey(float3 position)
    {
        return (int)(math.floor(position.x / tileSize) + (tileYMultiplier * math.floor(position.y / tileSize)));
    }

    private static void DebugDrawTiles(float3 position)
    {
        Vector3 lowerLeft = new Vector3(math.floor(position.x / tileSize) * tileSize, math.floor(position.y / tileSize) * tileSize);

        Debug.DrawLine(lowerLeft, lowerLeft + new Vector3(+1, +0) * tileSize);
        Debug.DrawLine(lowerLeft, lowerLeft + new Vector3(+0, +1) * tileSize);

        Debug.DrawLine(lowerLeft + new Vector3(+1, +0) * tileSize, lowerLeft + new Vector3(+1, +1) * tileSize);
        Debug.DrawLine(lowerLeft + new Vector3(+0, +1) * tileSize, lowerLeft + new Vector3(+1, +1) * tileSize);

        // Debug.Log(GetPosHashMapKey(position) + " " + position);
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

    // Start is called before the first frame update
    void Start()
    {
        wolf = Wolf.Instance;
        sheepInst = Sheep.Instance;
        grass = GrassGrowthAgent.Instance;

        //  boid = new Sheep();
        // boidList = new List<Sheep>();
        //List<Boid> boidTemp = Spawn(prefab, 60);
        // var sheepL = (Spawn(prefab, 60))
        //  sheep.SetSheepList(Spawn(prefab,60));

        // var boids = SpawnBoids(prefab, 60);

        var sheep = SpawnSheep(prefab, 60);
        // var sheepList = Converter.ConvertToSheep(boids);
        sheepInst.SetSheepList(sheep);
        //  sheep.SetSheepList(Converter.ConvertToSheep(SpawnBoids(prefab, 60)));


    }

    /*
    private int GetBoidsperTile(Dictionary<int, HashSet<Boid>> tileDict, int tileKey)
    {

        return tileDict[tileKey].Count;
    }

   */
    // Update is called Aonce per frame
    void Update()
    {
        // wolf.Spawn(40);
        // sheep.Spawn(60);
       // /*

        // debug stuff
        // Debug.Log(spawner.boids.Count);

        //HashSet<Boid> boidsSet = new HashSet<Boid>();
        //Dictionary<int, HashSet<Boid>> tileDict = new Dictionary<int, HashSet<Boid>>();

        HashSet<Boid> boidsSet = new HashSet<Boid>();
        Dictionary<int, HashSet<Boid>> tileDict = new Dictionary<int, HashSet<Boid>>();

        // sorting ALL boids to tile
        foreach (var sheep in sheepInst.GetSheepList())
        {
            var boid = sheep.GetComponent<Boid>();

            // create key
            int tileKey = GetPosHashMapKey(boid.transform.position);

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

            //  Debug.Log(GetBoidsperTile(tileDict, tileKey));

            foreach (var boid in tileDict[tileKey])
            {
                List<Boid> Vision = new List<Boid>();
                Vision = tileDict[tileKey].ToList();  // works // if (this != boid) // selfcheck
                //var eh = boid.GetComponent<Boid>();
                boid.oldUpdate(Vision);

                //    Debug.Log("BOID IN VISION");
                // Debug.Log(Vision);
            }



        }


       // */


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