using UnityEngine;

public class CharacterInteractController : MonoBehaviour
{
    CharacterController characterController;
    Rigidbody2D rgbody2d;
    [SerializeField] float offsetDistance = 0.7f;
    [SerializeField] float sizeOfInteractableArea = 0.7f;
    Character character;
    [SerializeField] HighlightController highlightController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        character = GetComponent<Character>();
        rgbody2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        CheckForInteractable();

        if (!characterController.moving && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    void CheckForInteractable()
    {
        Vector2 position = rgbody2d.position + characterController.lastMotionVector * offsetDistance;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, sizeOfInteractableArea);

        foreach (Collider2D collider in colliders)
        {
            Interactable interactedWithObject = collider.GetComponent<Interactable>();
            if (interactedWithObject != null)
            {
                highlightController.Highlight(interactedWithObject.gameObject);
                return;
            }
        }

        highlightController.Hide();
    }

    void OnDrawGizmosSelected()
    {
        Vector2 position = rgbody2d.position + characterController.lastMotionVector * offsetDistance;

        // Draw a wire circle with the given radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(position, sizeOfInteractableArea);
    }

    private void Interact()
    {
        Vector2 position = rgbody2d.position + characterController.lastMotionVector * offsetDistance;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, sizeOfInteractableArea);

        foreach(Collider2D collider in colliders)
        {
            Interactable interactedWithObject = collider.GetComponent<Interactable>();
            if (interactedWithObject != null)
            {
                interactedWithObject.Interact(character);
                break;
            }
        }
    }
}
