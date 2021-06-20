using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


[System.Serializable]
public class Sheep : Boid
{

    /// <summary>
    ///     Sheep move according to Boid Rules (see Boid.cs)
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
       
        List<Boid> SheepICanSee = new List<Boid>();
        // filter Vision
        foreach (var go in Vision)
        {
            if (go.GetComponent<Sheep>())
            {
                SheepICanSee.Add(go.GetComponent<Boid>());
            }
            if (go.GetComponent<GrassGrowthAgent>())
            {
                // Destroy(go); // works
                EatGrass(go);
            }
        }

        // TODO: add rule: seek food
        // TODO: add rule: flee
        this.GetComponent<Boid>().BoidMovement(SheepICanSee);





        EnergyLoss();
       // SheepReproduce = 2;
       // Spawn(SheepReproduce);

    }

    private void EatGrass(GameObject grass)
    {
        double delta = 0.5;
        if ((transform.position - grass.transform.position).magnitude <  delta)
        {
            Energy += SheepGainFromFood;
            Destroy(grass);
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

}
