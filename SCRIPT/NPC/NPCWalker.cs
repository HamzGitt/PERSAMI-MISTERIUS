using UnityEngine;
using UnityEngine.AI; // Wajib untuk NavMesh

public class NPCWalker : MonoBehaviour
{
    public Transform destination; // Tarik objek target ke sini (misal: rumah atau waypoint)
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (destination != null)
        {
            // Memberitahu NPC untuk berjalan ke posisi target
            agent.SetDestination(destination.position);
        }
    }
}