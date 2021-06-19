using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassGrowthAgent : MonoBehaviour
{
    //public static GrasslandLayer Grassland;
    public static GrassGrowthAgent Instance;

    public int GrassRegrowthPerStep { get; set; }

    public int Width { get; set; }
    public int Height { get; set; }

    /*
    public void Init(GrasslandLayer layer)
    {
        Grassland = layer;
    }
    */



    public void Die()
    {
        //  _grassland.SheepEnvironment.Remove(this);
        //  UnregisterHandle.Invoke(_grassland, this);
    }

    public void Tick()
    {
        /*
        for (var x = 0; x < Grassland.Width; x++)
        {
            for (var y = 0; y < Grassland.Height; y++)
            {
                var position = Position.CreatePosition(x, y);
                Grassland[position] = Grassland[position] + GrassRegrowthPerStep;
            }
        }
        */
    }

    //  public Guid ID { get; set; }

    void Awake()
    {
        Instance = this;
        Width = 10;
        Height = 10;
       // wolvesList.Clear();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Tick();
    }
}
