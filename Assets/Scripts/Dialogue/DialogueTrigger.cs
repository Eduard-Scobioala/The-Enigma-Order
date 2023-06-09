using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Character's Container")]
    [SerializeField] private Actor NPCCharacter;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    private bool playerInRange;

    private void Awake()
    {
        playerInRange = false;
        visualCue.SetActive(false);
    }

    private void Update()
    {
        // if player in range and dialogue not playing
        if (playerInRange && !DialogueManager.instance.DialogueIsPlaying)
        {
            visualCue.SetActive(true);
            if (!GameManager.instance.player.GetComponent<CharacterController>().moving && Input.GetKeyDown(KeyCode.E))
            {
                DialogueManager.instance.EnterDialogueMode(inkJSON, NPCCharacter);
            }
        }
        else
        {
            visualCue.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
