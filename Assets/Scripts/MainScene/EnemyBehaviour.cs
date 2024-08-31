using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    public Transform patrolRoute;
    public List<Transform> locations;

    private int locationIndex = 0;
    private NavMeshAgent agent;

    public Transform player;

    private int _lives = 1;


    public int EnemyLives
    {
        get { return _lives; }

        private set
        {
            _lives = value;
            if (_lives <= 0)
            {
                Destroy(this.gameObject);
                Debug.Log("Enemy Down");
            }
        }
    }



    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        InitiolizePatrolRoute();
        MoveToNextPatrolLocation();
        player = GameObject.Find("Player").transform;
    }

    void Update()
    {
        if (agent.remainingDistance < 0.5f && !agent.pathPending)
        {
            MoveToNextPatrolLocation();
        }
    }


    void InitiolizePatrolRoute()
    {
        foreach (Transform child in patrolRoute)
        {
            locations.Add(child);
        }
    }

    void MoveToNextPatrolLocation()
    {
        if (locations.Count == 0)
            return;

        agent.destination = locations[locationIndex].position;

        locationIndex = (locationIndex + 1) % locations.Count;

    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.name == "Player")
    //    {
    //        agent.destination = player.position;
    //        Debug.Log("Detected - Attack !");
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.name == "Player")
    //    {
    //        Debug.Log("Where are you disapear !?");
    //    }
    //}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "45ACP Bullet_Head(Clone)")
        {
            EnemyLives -= 1;
            Debug.Log("Critical Hit");
        }
    }
}

