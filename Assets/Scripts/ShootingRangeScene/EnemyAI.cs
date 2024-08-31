using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    const string RUN_TRIGGER = "Run";
    const string CROUCH_TRIGGER = "Crouch";
    const string SHOOT_TRIGGER = "Shoot";

    [SerializeField] private float minTimeInCover;
    [SerializeField] private float maxTimeInCover;
    [SerializeField] private int minShotsToFire;
    [SerializeField] private int maxShotsToFire;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float givenDamage;

    [SerializeField, Range(0f, 100)] private float shootingAccuracy;
    [SerializeField] private Transform shootingPosition;
    //[SerializeField] private ParticleSystem bloodSplatterFX;

    private bool isShooting;
    private int currentShotsTaken;
    private int currentMaxShotsToFire;
    private NavMeshAgent agent;
    [SerializeField] private Player player;
    private Transform occupiedCoverSpot;
    private Animator animator;

    [SerializeField, Tooltip("Enemy HP")] private int enemyHP = 1;

    public int EnemyLives
    {
        get { return enemyHP; }

        private set
        {
            enemyHP = value;
            if (enemyHP <= 0)
            {
                Destroy(this.gameObject);
                Debug.Log("Enemy Down");
            }
            //ParticleSystem effect = Instantiate(bloodSplatterFX, transform.position, Quaternion.identity);
            //effect.Stop();
            //effect.Play();
        }
    }
    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        animator.SetTrigger(RUN_TRIGGER);
    }

    public void Init(Player player, Transform coverSpot)
    {
        occupiedCoverSpot = coverSpot;
        this.player = player;
        GetToCover();
    }

    private void GetToCover()
    {
        agent.isStopped = false;
        agent.SetDestination(occupiedCoverSpot.position);

    }

    private void Update()
    {
        if (agent.isStopped == false && (transform.position - occupiedCoverSpot.position).sqrMagnitude < 0.1f)
        {
            agent.isStopped = true;
            StartCoroutine(InitializeShootingCO());
        }
        if (isShooting)
        {
            RotateTowardsPlayer();
        }
    }

    private IEnumerator InitializeShootingCO()
    {
        HideBehindCover();
        yield return new WaitForSeconds(UnityEngine.Random.Range(minTimeInCover, maxTimeInCover));
        StartShooting();
    }

    private void StartShooting()
    {
        isShooting = true;
        currentMaxShotsToFire = UnityEngine.Random.Range(minShotsToFire, maxShotsToFire);
        currentShotsTaken = 0;
        animator.SetTrigger(SHOOT_TRIGGER);
    }

    public void Shoot()
    {
        bool hitPlayer = UnityEngine.Random.Range(0, 100) < shootingAccuracy;

        if (hitPlayer)
        {
            RaycastHit hit; 
            Vector3 direction = player.GetHeadPosition() - shootingPosition.position;
            if (Physics.Raycast(shootingPosition.position, direction, out hit))
            {
                player = hit.collider.GetComponentInParent<Player>();
                if (player)
                {
                    player.TakeDamage(givenDamage);
                }
            }
        }
        currentShotsTaken++;
        if (currentShotsTaken >= currentMaxShotsToFire)
        {
            StartCoroutine(InitializeShootingCO());
        }
    }

    private void HideBehindCover()
    {
        animator.SetTrigger(CROUCH_TRIGGER);
    }

    private void RotateTowardsPlayer()
    {
        Vector3 direction = player.GetHeadPosition() - transform.position;
        direction.y = 0;
        Quaternion rotation = Quaternion.LookRotation(direction);
        rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        transform.rotation = rotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "45ACP Bullet_Head(Clone)")
        {
            EnemyLives--;
            Debug.Log("Critical Hit");
        }
    }
}

