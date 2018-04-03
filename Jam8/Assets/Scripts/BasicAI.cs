using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

// Adapted from Ash's IGME-202 Final Project.
public class BasicAI : MonoBehaviour
{

    public enum AIType { ATTACK, DEFEND, RECON }

    public enum AITeam { ONE, TWO }

    public AIType UnitAI = AIType.ATTACK;

    public AITeam Team = AITeam.ONE;
    public GameObject HomeBase;

    //Motion variables
    public float maxSpeed = 6.0f;
    public float maxForce = 12.0f;
    public float mass = 1.0f;
    public float radius = 1.0f;

    //Position is in the transform
    protected Vector3 acceleration;
    protected Vector3 velocity;
    protected Vector3 desired;

    //We'll need access to this
    CharacterController charControl;

    //The target of the Seeker
    public GameObject seekerTarget;
    //The calculated summing force
    private Vector3 ultimateForce;

    //Weighting
    public float seekeWeight = 1.75f;
    public float avoidWeight = 1.95f;

    virtual public void Start()
    {
        acceleration = Vector3.zero;
        velocity = transform.forward;
        charControl = GetComponent<CharacterController>();
        ultimateForce = Vector3.zero;
    }
    protected void Update()
    {
        CalcSteeringForces();

        //Standard motion
        velocity += acceleration * Time.deltaTime;
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        transform.forward = velocity.normalized;
        Debug.DrawLine(transform.position, transform.position + velocity);
        charControl.Move(velocity * Time.deltaTime);

        acceleration = Vector3.zero;
    }

    //Steering methods

    protected void CalcSteeringForces()
    {
        switch (UnitAI)
        {
            case AIType.ATTACK:
                {
                    if (seekerTarget != null)
                    {
                        ultimateForce += this.Arrive(seekerTarget.transform.position, 5.0f) * seekeWeight;
                        ultimateForce += this.AvoidObstacle(seekerTarget, 5.0f) * avoidWeight;
                    }
                    break;
                }
            case AIType.DEFEND:
                {
                    if (seekerTarget == null)
                    {
                        Collider[] colliders = Physics.OverlapSphere(transform.position, radius * 5.0f);
                        foreach (Collider c in colliders)
                        {
                            BasicAI ai = c.gameObject.GetComponent<BasicAI>();
                            if (ai != null)
                            {
                                if (ai.Team != Team && ai.UnitAI == AIType.ATTACK)
                                    seekerTarget = c.gameObject;
                            }
                        }
                        ultimateForce += this.Arrive(HomeBase.transform.position, 5.0f) * seekeWeight;
                    }
                    else
                    {
                        ultimateForce += this.Arrive(seekerTarget.transform.position, 5.0f) * seekeWeight;
                    }
                    break;
                }
            case AIType.RECON:
                {
                    if (seekerTarget == null)
                    {
                        Transform[] list = Physics.OverlapSphere(transform.position, 550.0f)
                            .Where(e => e.tag == "Resource")
                            .Select(e => e.transform)
                            .ToArray();
                        Transform nearest = GetClosest(list);
                        seekerTarget = nearest.gameObject;
                        ultimateForce += this.Arrive(HomeBase.transform.position, 5.0f) * seekeWeight;
                    } else
                    {
                        ultimateForce += this.Arrive(seekerTarget.transform.position, 5.0f) * seekeWeight;
                    }
                    break;
                }
        }
        ultimateForce = Vector3.ClampMagnitude(ultimateForce, maxForce);
        ApplyForce(ultimateForce);
    }

    Transform GetClosest(Transform[] enemies)
    {
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (Transform t in enemies)
        {
            float dist = Vector3.Distance(t.position, currentPos);
            if (dist < minDist)
            {
                tMin = t;
                minDist = dist;
            }
        }
        return tMin;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == seekerTarget)
        {
            Destroy(other.gameObject);
            seekerTarget = null;
            ultimateForce = Vector3.zero;
        }
    }

    protected void ApplyForce(Vector3 steeringForce)
    {
        acceleration += steeringForce / mass;
    }


    //Types of Steering Forces

    protected Vector3 Seek(Vector3 targetPosition)
    { //Standard seek method
        Vector3 seek = Vector3.zero;
        desired = Vector3.zero;

        desired = (targetPosition - transform.position);

        desired.y = 0;
        desired.Normalize();
        desired *= maxSpeed;
        seek = desired - velocity;

        return seek;
    }

    public Vector3 Arrive(Vector3 targetPosition, float slowDistance)
    { //Slow down as you get there
        Vector3 seek = Vector3.zero;
        desired = Vector3.zero;

        if (slowDistance < 0)
            return Vector3.zero;

        desired = (targetPosition - transform.position);

        float distance = desired.magnitude;
        float rSpeed = maxSpeed * (distance / slowDistance); //Reduced speed
        float cSpeed = Mathf.Min(rSpeed, maxSpeed); //Limited speed

        desired.y = 0;
        desired *= (cSpeed / distance);
        seek = desired - velocity;

        return seek;
    }

    protected Vector3 AvoidObstacle(GameObject obst, float safeDistance)
    { //Dodge obstacles
        Vector3 point = obst.transform.position - transform.position;
        desired = Vector3.zero;

        if (point.magnitude > safeDistance)
            return Vector3.zero;
        if (Vector3.Dot(point, transform.forward) < 0)
            return Vector3.zero;

        float dot = Vector3.Dot(point, transform.right);

        if (Mathf.Abs(dot) > 5.0f + radius)
            return Vector3.zero;
        if (dot > 0)
            desired = transform.right * -maxSpeed;
        else if (dot < 0)
            desired = transform.right * maxSpeed;

        Debug.DrawLine(transform.position, transform.position + desired, Color.yellow);
        return desired;
    }
}
