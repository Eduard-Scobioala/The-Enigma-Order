using UnityEngine;

public class CharacterInteractController : MonoBehaviour
{
    CharacterController characterController;
    Rigidbody2D rgbody2d;
    [SerializeField] float offsetDistance = 1.0f;
    [SerializeField] float sizeOfInteractableArea = 1.2f;
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

        if (Input.GetMouseButtonDown(0))
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
