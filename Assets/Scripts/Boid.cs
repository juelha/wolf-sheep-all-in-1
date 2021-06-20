using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; // to use .Where


[System.Serializable]
public class Boid : MonoBehaviour
{
    // private BoidSpawner spawner;
    public static Boid BoidInstance;

    // public Vector3 pos; // prob dont need
    public Vector3 velocity;
    public float maxVelocity;

    public float separationRadius;
    public float radius;


    private Boid boid;
    // public List<Boid> Vision;

    void Awake()
    {
        BoidInstance = this;
        boid = Boid.BoidInstance;
        // boids.Clear();
    }

    // Start is called before the first frame update
    void Start()
    {
        velocity = this.transform.forward * maxVelocity; 
    }

    // Update is called once per frame
    public void BoidMovement(List<Boid> Vision)
    {
        Debug.Log("TEST");
        calculateVelocity(Vision);
        movePosition();

    }


    void calculateVelocity(List<Boid> Vision)
    {
        // init
        var average_alignment = Vector3.zero;
        var average_cohesion = Vector3.zero;
        var average_separation = Vector3.zero;

        Vector3 seperationSum = Vector3.zero;
        Vector3 positionSum = Vector3.zero;
        Vector3 headingSum = Vector3.zero;

        Vector3 separationForce = Vector3.zero;
        Vector3 cohesionForce = Vector3.zero;
        Vector3 alignmentForce = Vector3.zero;
        Vector3 boundaryForce = Vector3.zero;

        int boidsNearby = 0;

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
        // limit velocity 
        if (velocity.magnitude > maxVelocity)
        {
            velocity = velocity.normalized * maxVelocity;
        }

        // dont move along z axis
        Vector3 pos = this.transform.position;
        pos.z = 0;
        this.transform.position = pos;

        this.transform.position += velocity * Time.deltaTime; // move 10 units every sec
        this.transform.up = velocity;
        //this.transform.rotation = Quaternion.LookRotation(velocity); // look in direction its going 



    }



}

