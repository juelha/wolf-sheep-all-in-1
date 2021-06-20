using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class Sheep : Boid
{

    /// <summary>
    ///     Sheep walk around by chance.
    ///     If grass is under the sheep, it eats the grass. Otherwise do nothing, this tick.
    ///     Every few rounds a new sheep is spawned, which receives half of the energy  
    /// </summary>



    public static Sheep Instance;
    public List<Sheep> sheepList;
    public List<Sheep> GetSheepList() { return sheepList; }
    public void SetSheepList(List<Sheep> sheepTemp) { sheepList = sheepTemp; }
    //public List<Sheep> sheepList { get; set; }

    public int SheepGainFromFood { get; set; }

    public int SheepReproduce { get; set; }


  // private GrasslandLayer _grassland;

   // public Position Position { get; set; }

    public string Type => "Sheep";
    public string Rule { get; private set; }
    public int Energy { get; private set; }



    public void Tick(List<GameObject> Vision)
    {
       
        List<Boid> SheepIcanSee = new List<Boid>();
        // filter Vision
        foreach (var go in Vision)
        {
            if (go.GetType() == typeof(Sheep))
            {
                // var boid = go.GetComponent<Boid>();
                SheepIcanSee.Add(go.GetComponent<Boid>());
            }
        }

        var boid = this.GetComponent<Boid>();
        boid.oldUpdate(SheepIcanSee);
        EnergyLoss();
       // SheepReproduce = 2;
       // Spawn(SheepReproduce);

    }

    private void EatGrass(GrassGrowthAgent grass)
    {
      //  if(this.GetTile() is Key in grassdict then)
        foreach(var grassobj in grass.GetGrassList())
        {
            if (this.transform.position.x == grassobj.transform.position.x)
            {
                Energy += SheepGainFromFood;
                grass.Die();
            }
                
        }
        
    }

    private void EnergyLoss()
    {
        Energy -= 2;
        if (Energy <= 0)
        {
            Die();
        }
    }



    public void Die()
    {
        
      //  _grassland.SheepEnvironment.Remove(this);
      //  UnregisterHandle.Invoke(_grassland, this);
    }

   // public Guid ID { get; set; }

    //-----------------------------------------------------------------------------------------
    /// <summary>
    /// BoidRules 
    /// </summary>
    /// 
  



    //------------------------------------------------------------------------------------------

    void Awake()
    {
        Instance = this;
        sheepList.Clear();

        // boids.Clear();
    }


    // Start is called before the first frame update
    void Start()
    {
        //Spawn(60);
        //velocity = this.transform.forward * maxVelocity;
        Energy /= 2;

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("fsociety");
        //Tick();
       // calculateVelocity(Vision);
       // movePosition();
    }
}
