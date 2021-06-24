using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


/// <summary>
///     MICROLEVEL(what can the agent see and do)
///     Sheep move according to Boid Rules (see Boid.cs)
///     If grass is under the sheep, it eats the grass. Otherwise do nothing, this tick.
///     Every few rounds a new sheep is spawned, which receives half of the energy  
/// </summary>

[System.Serializable]
public class Sheep : Boid
{

    public static Sheep Instance;

    public int SheepGainFromFood { get; set; }
    public int SheepReproduce { get; set; }

    public string Type => "Sheep";
    public string Rule { get; private set; }
    public int Energy { get; private set; }


    // Init -------------------------------------------------------------------------
    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Energy = Energy / 2;
        SheepReproduce = 1;
    }


    // Update------------------------------------------------------------------------
    public void Tick(List<GameObject> Vision)
    {
        EnergyLoss();
        // Spawn(SheepReproduce);

        List<Boid> SheepICanSee = new List<Boid>();
        List<GameObject> GrassICanSee = new List<GameObject>();
        List<GameObject> WolvesICanSee = new List<GameObject>();
        // filter Vision
        foreach (var go in Vision)
        {
            if (go.GetComponent<Wolf>())
            {
                WolvesICanSee.Add(go);
            }
            if (go.GetComponent<Sheep>())
            {
                SheepICanSee.Add(go.GetComponent<Boid>());
            }
            if (go.GetComponent<GrassGrowthAgent>())
            {
                //Destroy(go); // works
                //EatGrass(go);
                GrassICanSee.Add(go);
            }
            

        }

        // TODO: add rule: seek food
        // TODO: add rule: flee
        // this.GetComponent<Boid>().BoidMovement(SheepICanSee);
        this.GetComponent<Boid>().followBoidRules(SheepICanSee);
        this.Hunt(GrassICanSee);
        this.Flee(WolvesICanSee);
        this.GetComponent<Boid>().movePosition();


    }


    // Methods-----------------------------------------------------------------------

    private void EnergyLoss()
    {
        Energy -= 2;
        if (Energy <= 0)
        {
            // Destroy(this.GetComponent<GameObject>());
            Debug.Log("died");
        }
    }

    public void Hunt(List<GameObject> targets)
    {
        for (int i = 0; i < 1; i++)
        {
            // kill sheep
            double delta = 0.5;
            if ((transform.position - targets[i].transform.position).magnitude < delta)
            {
                Energy += SheepGainFromFood;
                Destroy(targets[i]);
            }
            // MoveTowardsTarget
            var directionToEnemy = (targets[i].transform.position - transform.position);
            velocity += directionToEnemy;
            //velocity *= 100;
        }

    }

    public void Flee(List<GameObject> badguys)
    {
        foreach (var badguy in badguys)
        {
            
            // MoveTowardsTarget negated 
            var directionToBadguy = (transform.position - badguy.transform.position);
            //Debug.DrawRay(transform.position, directionToBadguy, Color.red);
            velocity += directionToBadguy;
            velocity *= 100; 
        }

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




}
