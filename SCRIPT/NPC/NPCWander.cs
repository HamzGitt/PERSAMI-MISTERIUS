using UnityEngine;
using UnityEngine.AI;

public class NPCWander : MonoBehaviour
{
    public float wanderRadius = 10f;
    public float wanderTimer = 5f;

    private NavMeshAgent agent;
    private Animator anim; // Tambahkan ini
    private float timer;

    void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>(); // Ambil komponen Animator
        timer = wanderTimer;
    }

    void Update()
    {
        timer += Time.deltaTime;

        // Mengirimkan kecepatan agent ke parameter "Speed" di Animator
        // .magnitude mengukur seberapa cepat agent bergerak saat itu
        if (anim != null)
        {
            anim.SetFloat("Speed", agent.velocity.magnitude);
        }

        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavMeshLocation(wanderRadius);
            agent.SetDestination(newPos);
            timer = 0;
        }
    }

    public Vector3 RandomNavMeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }
}