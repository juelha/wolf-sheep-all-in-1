using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
///     MICROLEVEL(what can the agent see and do)
///     Wolf follow boid rules & hunt sheep
///     Every few rounds a new wolf is spawned, which receives half of the energy  
/// </summary>

[System.Serializable]
public class Wolf : Boid
{

    public static Wolf Instance;

    public int WolfGainFromFood { get; set; }
    public int WolfReproduce { get; set; }

    public string Type => "Wolf";
    public string Rule { get; private set; }
    public int Energy { get; private set; }


    // Init -------------------------------------------------------------------------
    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update <- init
    void Start()
    {
        Energy = Energy / 2;
        WolfReproduce = 1;
    }


    // Update------------------------------------------------------------------------
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

       // this.GetComponent<Boid>().followBoidRules(WolvesICanSee);
        this.Hunt(SheepICanSee);
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
            Debug.DrawRay(transform.position, directionToEnemy, Color.red);
            velocity += directionToEnemy;
            velocity *= 100;
        }

    }




}
