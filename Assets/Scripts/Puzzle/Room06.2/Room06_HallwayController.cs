// Developer: Jhon Jellar Z. Miranda
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Room06_HallwayController : MonoBehaviour
{
    public static Room06_HallwayController Instance { get; private set; }

    [Header("Intro Cutscene")]
    [TextArea] public string[] introLines = {
        "...This place.",
        "...It's getting worse.",
        "Stay on..."
    };
    public string introPrefsKey = "R06_IntroPlayed";

    [Header("Emily AI Setup")]
    public bool isEmilyHunting = false;
    public GameObject emilyEnemy;
    public Transform emilySpawnPoint;
    public Vector2 spawnFacing = Vector2.left; 

    [Header("Audio")]
    public AudioSource hallwayAudioSource;
    public AudioClip lullabyChaseMusic;

    private NavMeshAgent emilyAgent;
    private Transform playerTransform;
    private GameObject playerObject;
    private bool isChaseSequencePlaying = false; // Panangga para di mag-doble trigger

    private void Awake()
    {
        if (Instance == null) Instance = this;
        if (emilyEnemy != null) emilyAgent = emilyEnemy.GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        if (emilyEnemy != null) emilyEnemy.SetActive(false);

        playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null) playerTransform = playerObject.transform;

        if (PlayerPrefs.GetInt(introPrefsKey, 0) == 0)
        {
            StartCoroutine(PlayIntroSequence());
        }
    }

    private void Update()
    {
        // Gagalaw lang si Emily kung hunting na AT naka-enable na yung NavMeshAgent niya
        if (isEmilyHunting && emilyEnemy != null && emilyEnemy.activeInHierarchy && playerTransform != null)
        {
            if (emilyAgent != null && emilyAgent.isActiveAndEnabled && emilyAgent.isOnNavMesh)
            {
                emilyAgent.SetDestination(playerTransform.position);
            }
        }
    }

    private IEnumerator PlayIntroSequence()
    {
        PlayerPrefs.SetInt(introPrefsKey, 1);
        PlayerPrefs.Save();

        yield return new WaitForSeconds(0.5f);

        MonoBehaviour playerController = null;
        Rigidbody2D rb = null;

        if (playerObject != null)
        {
            playerController = playerObject.GetComponent("JoystickPlayerController") as MonoBehaviour;
            rb = playerObject.GetComponent<Rigidbody2D>();
            
            if (playerController != null) playerController.enabled = false;
            if (rb != null) rb.linearVelocity = Vector2.zero; 
        }

        foreach (string line in introLines)
        {
            if (DialogueSystemV2.Instance != null)
            {
                DialogueSystemV2.Instance.StartDialogue(line, "Lisa");
                yield return new WaitForSeconds(0.1f);
                yield return new WaitUntil(() => DialogueSystemV2.Instance == null || !DialogueSystemV2.Instance.IsDialogueActive());
            }
        }

        if (playerController != null) playerController.enabled = true;
    }

    // Pinalitan natin ito para maging Coroutine!
    public void TriggerEmilyChase()
    {
        if (isChaseSequencePlaying || isEmilyHunting) return; 
        StartCoroutine(ChaseSequence());
    }

    private IEnumerator ChaseSequence()
    {
        isChaseSequencePlaying = true;
        
        // 1. I-play ang Lullaby Music
        if (hallwayAudioSource != null && lullabyChaseMusic != null)
        {
            hallwayAudioSource.clip = lullabyChaseMusic;
            hallwayAudioSource.loop = true;
            hallwayAudioSource.Play();
        }

        // 2. Palitawin si Emily pero NAKA-FREEZE pa
        if (emilyEnemy != null)
        {
            if (emilySpawnPoint != null) emilyEnemy.transform.position = emilySpawnPoint.position;
            emilyEnemy.SetActive(true);

            EmilyGhost emilyScript = emilyEnemy.GetComponent<EmilyGhost>();
            if (emilyScript != null)
            {
                emilyScript.ForceFacing(spawnFacing);
                emilyScript.enabled = false; // Pigilan munang mag-isip
            }
            if (emilyAgent != null)
            {
                emilyAgent.enabled = false; // Pigilan munang maglakad
            }
        }

        // 3. I-freeze si Lisa
        MonoBehaviour playerController = null;
        Rigidbody2D playerRb = null;

        if (playerObject != null)
        {
            playerController = playerObject.GetComponent("JoystickPlayerController") as MonoBehaviour;
            playerRb = playerObject.GetComponent<Rigidbody2D>();
            
            if (playerController != null) playerController.enabled = false;
            if (playerRb != null) playerRb.linearVelocity = Vector2.zero; 
        }

        // 4. I-play ang Dialogue ni Lisa
        if (DialogueSystemV2.Instance != null)
        {
            DialogueSystemV2.Instance.StartDialogue("She's here! I need to get to my room NOW!", "Lisa");
            yield return new WaitForSeconds(0.1f);
            
            // Hihintayin ng laro na i-click ng player yung "Next" bago ituloy
            yield return new WaitUntil(() => DialogueSystemV2.Instance == null || !DialogueSystemV2.Instance.IsDialogueActive());
        }

        // 5. I-unfreeze si Lisa
        if (playerController != null) playerController.enabled = true;

        // 6. ITULOY ANG HUNTING!
        isEmilyHunting = true;

        if (emilyEnemy != null)
        {
            EmilyGhost emilyScript = emilyEnemy.GetComponent<EmilyGhost>();
            if (emilyScript != null)
            {
                if (emilyEnemy.GetComponent<EmilyMovement>() != null)
                    emilyEnemy.GetComponent<EmilyMovement>().enabled = true;

                emilyScript.enabled = true;
                emilyScript.SetStateExternal(EmilyGhost.State.Hunt);
            }
            
            if (emilyAgent != null)
            {
                emilyAgent.enabled = true; // Buhayin ang NavMesh
            }
        }
    }

    [ContextMenu("Reset Hallway Data")]
    public void ResetHallwayData()
    {
        PlayerPrefs.DeleteKey(introPrefsKey);
        PlayerPrefs.DeleteKey("R06_PhotoInteracted");
        PlayerPrefs.Save();
        Debug.Log("Hallway Progress Reset!");
    }
}