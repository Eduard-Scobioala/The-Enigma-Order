using TMPro;
using UnityEngine;

public class NotesController : MonoBehaviour, IDataPersistance
{
    [SerializeField] GameObject NotesInterface;
    [SerializeField] GameObject NotesButton;
    [SerializeField] TMP_InputField notesText;

    public void OnNotesPressed()
    {
        // Not able to open notes while in dialogue mode
        if (GameManager.instance.dialogueManager.DialogueIsPlaying)
        {
            return;
        }

        NotesInterface.SetActive(true);
        NotesButton.SetActive(false);

        // Disable movement and inventory
        GameManager.instance.characterCanMove = false;
        GameManager.instance.canOpenInventory = false;
    }

    public void OnNotesExitPressed()
    {
        NotesInterface.SetActive(false);
        NotesButton.SetActive(true);

        // Activate movement and inventory
        GameManager.instance.characterCanMove = true;
        GameManager.instance.canOpenInventory = true;
    }

    public void LoadData(GameData data)
    {
        notesText.text = data.notesText;
    }

    public void SaveData(GameData data)
    {
        data.notesText = notesText.text;
    }
}
