using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; // to use .Where

/// <summary>
///     MICROLEVEL(what can the agent see and do)
///     Parentclass to Sheep and Wolf
/// </summary>

[System.Serializable]
public class Boid : MonoBehaviour
{
    public Vector3 velocity;
    public float maxVelocity;

    public float separationRadius;
    public float radius;

    private Boid boid;


    // Init -------------------------------------------------------------------------
    // Start is called before the first frame update
    void Start()
    {
        velocity = this.transform.forward * maxVelocity; 
    }


    // Update -----------------------------------------------------------------------
    public void BoidMovement(List<Boid> Vision)
    {
        followBoidRules(Vision);
        movePosition();
    }


    // Methods ----------------------------------------------------------------------
    public void followBoidRules(List<Boid> Vision)
    {
        // init
        Vector3 average_separation = Vector3.zero;
        Vector3 average_cohesion = Vector3.zero;
        Vector3 average_alignment = Vector3.zero;

        Vector3 boundaryForce = Vector3.zero;

        int boidsNearby = 0;

        foreach (var boid in Vision)
        {
            if (this != boid) // selfcheck
            {

                Vector3 otherBoidPosition = boid.transform.position;
                float distToOtherBoid = (transform.position - otherBoidPosition).magnitude;

                // draw rays 
                Debug.DrawRay(transform.position, boid.transform.position - transform.position, Color.white);  // works!!!


                // rules
                if (distToOtherBoid < radius)
                {
                    average_separation += -(otherBoidPosition - transform.position) * (1f / Mathf.Max(distToOtherBoid, .0001f));  // other radius?
                    average_cohesion += otherBoidPosition; // diff?
                    average_alignment += boid.transform.forward; 

                    boidsNearby++;
                }

                if (boidsNearby > 0)
                {
                    average_separation /= boidsNearby;
                    average_cohesion = (average_cohesion / boidsNearby) - transform.position;
                    average_alignment /= boidsNearby;
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

        // TODO: make weights a public var
        velocity += (average_separation*50) + average_cohesion + average_alignment + boundaryForce;

    }


    public void movePosition()
    {
        maxVelocity = 2;
        // limit velocity 
        Debug.Log("velocity.magnitude");
        Debug.Log(velocity.magnitude);
        if (velocity.magnitude > maxVelocity)
        {
            Debug.Log("fuck.magnitude");
            velocity = velocity.normalized * maxVelocity;
        }
        Debug.Log("velocity.magnitude");
        Debug.Log(velocity.magnitude);

        // dont move along z axis
        Vector3 pos = this.transform.position;
        pos.z = 0;
        this.transform.position = pos;

       // Debug.Log(this.transform.position);
       // Debug.Log(velocity);
        this.transform.position += velocity * Time.deltaTime; // move 10 units every sec
        this.transform.up = velocity; // boid looks into direction its heading
    }

}

