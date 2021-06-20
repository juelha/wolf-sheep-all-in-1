using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// MICROLEVEL
// what can the agent see and do

[System.Serializable]
public class Wolf : Boid
{

    /// <summary>
    ///     Wolf follow boid rules + rule of killing sheep
    ///     If grass is under the sheep, it eats the grass. Otherwise do nothing, this tick.
    ///     Every few rounds a new wolf is spawned, which receives half of the energy  
    /// </summary>

    public static Wolf Instance;
    public List<Wolf> wolvesList;
    public List<Wolf> GetWolves() { return wolvesList; }
   // public List<Wolf> List { get; set; }

    public int WolfGainFromFood { get; set; }

    public int WolfReproduce { get; set; }

    //private GrasslandLayer _grassland;

   // public Position Position { get; set; }

    public string Type => "Wolf";
    public string Rule { get; private set; }
    public int Energy { get; private set; }


    public void Tick(List<GameObject> Vision)
    {
        EnergyLoss();
       // Spawn(WolfReproduce);

        List<Boid> WolvesICanSee = new List<Boid>();
        List<Sheep> SheepICanSee = new List<Sheep>();
        // filter Vision
        foreach (var go in Vision)
        {
            // wolves
            if (go.GetComponent<Wolf>())
            {
                WolvesICanSee.Add(go.GetComponent<Boid>());
            }
            // sheep
            if (go.GetComponent<Sheep>())
            {
                SheepICanSee.Add(go.GetComponent<Sheep>());
            }
            
        }

        // TODO: add rule: seek food
        // TODO: add rule: flee
        this.GetComponent<Boid>().BoidMovement(WolvesICanSee);



    }

    private void MoveTowardsTarget(Sheep target)
    {
        var directionToEnemy = (transform.position - target.transform.position);
        velocity += directionToEnemy;
    }

    private void EnergyLoss()
    {
        Energy -= 2;
        if (Energy <= 0)
        {
            Die();
           // _grassland.WolfEnvironment.Remove(this);
        }
    }

    public void Die()
    {
        //  _grassland.SheepEnvironment.Remove(this);
        //  UnregisterHandle.Invoke(_grassland, this);
    }


    private void RandomMove()
    {
      //  var bearing = RandomHelper.Random.Next(360);
     //   Position = _grassland.WolfEnvironment.MoveTowards(this, bearing, 3);
    }

    private void Hunt(Sheep sheep)
    {
        Vector3 sheepPosition = sheep.transform.position;
        float targetDistance = (transform.position - sheepPosition).magnitude;



    }

    private void EatSheep(Sheep sheep)
    {
        Energy += WolfGainFromFood;
        sheep.Die();
    }



   // public Guid ID { get; set; }

    void Awake()
    {
        Instance = this;
        wolvesList.Clear();
    }


    // Start is called before the first frame update <- init
    void Start()
    {
        velocity = this.transform.forward * maxVelocity;
        Energy = Energy / 2;
        WolfReproduce = 1;
        //_grassland.WolfEnvironment.Insert(this);
    }

}
