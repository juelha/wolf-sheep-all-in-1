using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using System.Linq;

public class Tiling : MonoBehaviour
{
    public static Tiling TilingInstance;
    private const int tileYMultiplier = 1000;
    private const int tileSize = 5;
    [SerializeField] private GameObject prefab;

    private Wolf wolf;
    private Sheep sheep;
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

    // Start is called before the first frame update
    void Start()
    {
        wolf = Wolf.Instance;
        sheep = Sheep.Instance;
        grass = GrassGrowthAgent.Instance;




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
        wolf.Spawn(40);
        sheep.Spawn(60);
        /*

        // debug stuff
        // Debug.Log(spawner.boids.Count);

        HashSet<Boid> boidsSet = new HashSet<Boid>();
        Dictionary<int, HashSet<Boid>> tileDict = new Dictionary<int, HashSet<Boid>>();

        //  NativeMultiHashMap<int, Boid> tileMultiHashMap = new NativeMultiHashMap<int, Boid>(spawner.boids.Count, Allocator.Temp);

        // sorting ALL boids to tile
        foreach (var boid in spawner.boids)
        {
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
                boid.oldUpdate(Vision);

                //    Debug.Log("BOID IN VISION");
                // Debug.Log(Vision);
            }



        }


        */


    }
}