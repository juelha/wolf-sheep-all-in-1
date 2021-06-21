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
    public List<Wolf> wolfList;
    public List<Wolf> GetWolfList() { return wolfList; }
    public void SetWolfList(List<Wolf> wTemp) { wolfList = wTemp; }
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


    public void Tick(List<GameObject> Vision)
    {
        EnergyLoss();
       // Spawn(WolfReproduce);

        List<Boid> WolvesICanSee = new List<Boid>();
        List<GameObject> SheepICanSee = new List<GameObject>();
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
                SheepICanSee.Add(go);
            }
            
        }


        // TODO: add rule: seek food
        // TODO: add rule: flee
        this.GetComponent<Boid>().followBoidRules(WolvesICanSee);
        this.Hunt(SheepICanSee);
        this.GetComponent<Boid>().movePosition();


    }



    private void EnergyLoss()
    {
        Energy -= 2;
        if (Energy <= 0)
        {
           // Destroy(this.GetComponent<Boid>());
            Debug.Log("died");
        }
    }



    public void Hunt(List<GameObject> targets)
    {
        foreach (var target in targets)
        {
            // kill sheep
            double delta = 0.5;
            if ((transform.position - target.transform.position).magnitude < delta)
            {
                Energy += WolfGainFromFood;
                Destroy(target);
            }
            // MoveTowardsTarget
            var directionToEnemy = (target.transform.position - transform.position);
            velocity += directionToEnemy;
            velocity *= 100;
        }

    }




}
