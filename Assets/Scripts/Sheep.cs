using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Sheep : MonoBehaviour
{

    /// <summary>
    ///     Sheep walk around by chance.
    ///     If grass is under the sheep, it eats the grass. Otherwise do nothing, this tick.
    ///     Every few rounds a new sheep is spawned, which receives half of the energy  
    /// </summary>

    [SerializeField] private GameObject prefab;
    [SerializeField] public int number;
    public float radius;
    public Vector3 velocity;
    public float maxVelocity;

    public static Sheep Instance;
    public List<Sheep> sheepList;
    public List<Sheep> GetSheepList() { return sheepList; }
   // public List<Sheep> List { get; set; }

    public int SheepGainFromFood { get; set; }

    public int SheepReproduce { get; set; }


  // private GrasslandLayer _grassland;

   // public Position Position { get; set; }

    public string Type => "Sheep";
    public string Rule { get; private set; }
    public int Energy { get; private set; }



    public void Tick()
    {
        EnergyLoss();
        Spawn(SheepReproduce);
        /*
        if (_grassland[id_key] > 0)
        {
            Rule = "R1 - Eat grass";
            EatGrass();
        }
        else
        {
            Rule = "R2 - No food found";
        }
        */
    }

    private void EatGrass(GrassGrowthAgent grass)
    {
        Energy += SheepGainFromFood;
        grass.Die();
    }

    private void EnergyLoss()
    {
        Energy -= 2;
        if (Energy <= 0)
        {
            Die();
        }
    }

    public void Spawn(int percent)
    {
        percent = 10;
        var number =  100 / percent;

        for (int i = 0; i < number; i++)
        {

            Vector3 pos = this.transform.position + Random.insideUnitSphere * radius;
            Quaternion rot = Random.rotation;

            Sheep newBoid = Instantiate(prefab, pos, rot).GetComponent<Sheep>();
            newBoid.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
            sheepList.Add(newBoid);
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
    void calculateVelocity(List<Sheep> Vision)
    {

        // init
        Vector3 seperationSum = Vector3.zero;
        Vector3 positionSum = Vector3.zero;
        Vector3 headingSum = Vector3.zero;
        Vector3 separationForce = Vector3.zero;
        Vector3 cohesionForce = Vector3.zero;
        Vector3 alignmentForce = Vector3.zero;
        Vector3 boundaryForce = Vector3.zero;


        int boidsNearby = 0;


        // BoidsList.Add(FindObjectOfType<GameObject>());
        // debug stuff
        // Debug.Log(BoidsList.Count);
        // Debug.Log(spawner.boids.Count);
        foreach (var boid in Vision)
        { 

            if (this != boid) // selfcheck
            {

                Vector3 otherBoidPosition = boid.transform.position;
                float distToOtherBoid = (transform.position - otherBoidPosition).magnitude;

                // draw rays and change color per boid
                 /*
                //  var Colornew;
                var Colorme = this.GetComponent<Renderer>().material.color;
                var Colorother = boid.GetComponent<Renderer>().material.color;

                //  Colornew = Colorme - Colorother;


                Debug.DrawRay(transform.position, boid.transform.position - transform.position, Colorme);  // works!!!
                 */

                // rules
                if (distToOtherBoid < radius)
                {

                    seperationSum += -(otherBoidPosition - transform.position) * (1f / Mathf.Max(distToOtherBoid, .0001f));  // other radius?
                    positionSum += otherBoidPosition; // diff?
                    headingSum += boid.transform.forward; //average_alignment += boid.velocity;

                    boidsNearby++;
                }

                if (boidsNearby > 0)
                {
                    separationForce = seperationSum / boidsNearby;
                    cohesionForce = (positionSum / boidsNearby) - transform.position;
                    alignmentForce = headingSum / boidsNearby;
                }
                else
                {
                    separationForce = Vector3.zero;
                    cohesionForce = Vector3.zero;
                    alignmentForce = Vector3.zero;
                }


            }
        }

        // container
        if (transform.position.magnitude > radius)
        {


            //                                            direction from where we are           increase the further u get outside           smoothing out 
            boundaryForce = transform.position.normalized * (radius - transform.position.magnitude) * Time.deltaTime;
            // boundaryForce *= boundaryWeight; 
        }


        velocity += separationForce + cohesionForce + alignmentForce + boundaryForce;


    }


    void movePosition()
    {

        // TODO add weight here?

        // limit velocity 
        if (velocity.magnitude > maxVelocity)
        {
            velocity = velocity.normalized * maxVelocity;
        }
        this.transform.position += velocity * Time.deltaTime; // move 10 units every sec
        this.transform.up = velocity;
        //this.transform.rotation = Quaternion.LookRotation(velocity); // look in direction its going 



    }





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
        Spawn(60);
        velocity = this.transform.forward * maxVelocity;
        Energy = Energy / 2;

    }

    // Update is called once per frame
    void Update()
    {
        
        Tick();
       // calculateVelocity(Vision);
        movePosition();
    }
}
