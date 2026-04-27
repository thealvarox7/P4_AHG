using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float detectionRange = 15f;
    [SerializeField] private float stopDistance = 2f;

    private NavMeshAgent agent;
    private Health health;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();
    }

    private void Start()
    {
        FindPlayer();
    }

    private void Update()
    {
        if (health != null && health.IsDead)
        {
            Destroy(gameObject);
            return;
        }

        if (player == null)
        {
            FindPlayer();
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > detectionRange)
        {
            StopEnemy();
            return;
        }

        if (distanceToPlayer <= stopDistance)
        {
            StopEnemy();
            return;
        }

        agent.isStopped = false;
        agent.SetDestination(player.position);
    }

    private void FindPlayer()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    private void StopEnemy()
    {
        agent.isStopped = true;
        agent.ResetPath();
    }
}