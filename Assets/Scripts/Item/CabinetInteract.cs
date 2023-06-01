using UnityEngine;

public class CabinetInteract : Interactable
{
    [SerializeField] AudioClip onOpenAudio;
    Animator animator;
    bool open = false;

    public override void Interact(Character character)
    {
        animator = GetComponent<Animator>();
        if (!open)
        {
            open = true;
            animator.SetTrigger("open");

            AudioManager.instance.Play(onOpenAudio);
        }
        else
        {
            open = false;
            animator.SetTrigger("close");
        }
    }
}
