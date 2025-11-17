using UnityEngine;
using System.Collections;

public sealed class EmilyPerception : MonoBehaviour
{
    [Header("Vision")]
    public float visionRange = 6f;
    public float visionAngle = 60f;

    [Header("References")]
    [Tooltip("Assign the AI_Forward child here. This determines where Emily is 'looking'.")]
    public Transform aiForward;

    LayerMask playerMask;
    LayerMask obstacleMask;

    [Header("Hearing")]
    public float hearingRadius = 8f;

    public bool PlayerVisible { get; private set; }
    public bool HeardNoise => Time.time < _lastNoise + 2f;

    public Vector3 LastSeenPos { get; private set; }
    public Vector3 LastNoisePos { get; private set; }

    Transform _player;
    float _lastNoise;

    void Awake()
    {
        // Auto-find aiForward
        if (aiForward == null)
        {
            aiForward = transform.Find("AI_Forward");
            if (aiForward == null)
                Debug.LogError("[EMILY PERCEPTION] Could not find AI_Forward child!!");
            else
                Debug.Log("[EMILY PERCEPTION] Auto-assigned aiForward.");
        }
    }


    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player")?.transform;

        playerMask = LayerMask.GetMask("Player");
        obstacleMask = LayerMask.GetMask("Walls");

        StartCoroutine(VisionRoutine());
    }


    IEnumerator VisionRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.1f);

        while (true)
        {
            if (_player != null)
                CheckVision();

            yield return wait;
        }
    }

    void CheckVision()
    {
        if (_player == null)
        {
            Debug.Log("[EMILY DEBUG] Player transform is NULL.");
            PlayerVisible = false;
            return;
        }

        if (aiForward == null)
        {
            Debug.Log("[EMILY DEBUG] aiForward is NULL. Cannot compute vision.");
            PlayerVisible = false;
            return;
        }

        Vector2 toP = _player.position - transform.position;
        float dist = toP.magnitude;

        // ---------------------------
        // Distance Check
        // ---------------------------
        if (dist > visionRange)
        {
            Debug.Log($"[EMILY DEBUG] Player too far. Dist={dist:F2}, Limit={visionRange}");
            PlayerVisible = false;
            return;
        }

        // ---------------------------
        // Angle Check
        // ---------------------------
        Vector2 forward = aiForward.up;
        float ang = Vector2.Angle(forward, toP);

        if (ang > visionAngle * 0.5f)
        {
           Debug.Log($"[EMILY DEBUG] Angle too wide. ang={ang:F2}, Limit={visionAngle * 0.5f}");
            PlayerVisible = false;
            return;
        }

        // ---------------------------
        // Raycast LOS Check
        // ---------------------------
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            toP.normalized,
            dist,
            playerMask | obstacleMask
        );

        if (!hit)
        {
            Debug.Log("[EMILY DEBUG] Raycast hit NOTHING.");
            PlayerVisible = false;
            return;
        }

        Debug.Log($"[EMILY DEBUG] Raycast hit: {hit.collider.name}, Layer={LayerMask.LayerToName(hit.collider.gameObject.layer)}");

        if (hit.collider.CompareTag("Player"))
        {
            Debug.Log("[EMILY DEBUG] >>> PLAYER VISIBLE <<<");
            PlayerVisible = true;
            LastSeenPos = _player.position;
        }
        else
        {
            Debug.Log("[EMILY DEBUG] Blocked by: " + hit.collider.name);
            PlayerVisible = false;
        }
    }

    // Call from noises
    public void HearNoise(Vector3 pos, float strength = 1f)
    {
        if ((pos - transform.position).sqrMagnitude >
            hearingRadius * hearingRadius * strength) return;

        _lastNoise = Time.time;
        LastNoisePos = pos;
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (aiForward == null)
            return;

        Gizmos.color = PlayerVisible ? Color.red : Color.yellow;

        Vector3 fwd = aiForward.up;
        Vector3 left = Quaternion.Euler(0, 0, -visionAngle / 2) * fwd;
        Vector3 right = Quaternion.Euler(0, 0, visionAngle / 2) * fwd;

        Gizmos.DrawRay(transform.position, left * visionRange);
        Gizmos.DrawRay(transform.position, right * visionRange);

        Gizmos.DrawWireSphere(transform.position, visionRange);
    }
#endif
}
