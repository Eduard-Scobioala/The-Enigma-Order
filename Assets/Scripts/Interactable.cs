using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private int value;
    public virtual void Interact(Character character)
    {

    }
}
