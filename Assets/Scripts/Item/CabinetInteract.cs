using UnityEngine;

public class CabinetInteract : Interactable
{
    Animator animator;
    bool open = false;
    public override void Interact(Character character)
    {
        animator = GetComponent<Animator>();
        if (!open)
        {
            open = true;
            animator.SetTrigger("open");
        }
        else
        {
            open = false;
            animator.SetTrigger("close");
        }
    }
}
