using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : MonoBehaviour
{

    /// <summary>
    ///     Wolf follow boid rules + rule of killing sheep
    ///     If grass is under the sheep, it eats the grass. Otherwise do nothing, this tick.
    ///     Every few rounds a new wolf is spawned, which receives half of the energy  
    /// </summary>

    [SerializeField] private GameObject prefab;
    [SerializeField] public int number;
    public float radius;
    public Vector3 velocity;
    public float maxVelocity;

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


    public void Tick(List<Sheep> Vision)
    {
        EnergyLoss();
        Spawn(WolfReproduce);

        foreach (var sheep in Vision)
        {
            if (this != sheep) // checking tag??
            {

                Vector3 sheepPosition = sheep.transform.position;
                float targetDistance = (transform.position - sheepPosition).magnitude;

                if (targetDistance <= 2)
                {
                    Rule = "R3 - Eat Sheep";
                    EatSheep(sheep);
                }
                else if (targetDistance < 10)
                {
                    Rule = $"R4 - Move towards sheep: {targetDistance} tiles away";
                    MoveTowardsTarget(sheep);
                }
                else
                {
                    Rule = "R5 - No sheep near by - random move";
                    RandomMove();
                }
            }
            else
            {
                Rule = "R6 - No more sheep exist";
                RandomMove();
            }
        }
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

    private void EatSheep(Sheep sheep)
    {
        Energy += WolfGainFromFood;
        sheep.Die();
    }

    public void Spawn(int percent)
    {
        var number = percent / 100;

        for (int i = 0; i < number; i++)
        {

            Vector3 pos = this.transform.position + Random.insideUnitSphere * radius;
            Quaternion rot = Random.rotation;

            Wolf newBoid = Instantiate(prefab, pos, rot).GetComponent<Wolf>();
            newBoid.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
            wolvesList.Add(newBoid);
        }
    }


    //-----------------------------------------------------------------------------------------
    /// <summary>
    /// BoidRules 
    /// </summary>
    /// 
    void calculateVelocity(List<Wolf> Vision)
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

    // Update is called once per frame
    void Update()
    {
       // Tick();
    }
}
