using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] TMP_Text targetText;
    [SerializeField] TMP_Text nameText;
    [SerializeField] Image portrait;

    DialogueContainer currentDialogue;
    int currentTextLine;

    // Reveal Dialogue Text parameters
    [Range(0f, 1f)]
    [SerializeField] float visibleTextPercent;
    [SerializeField] float timePerLetter = 0.05f;
    float totalTimeToType, currentTime;
    string lineToShow;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            PushText();
        }
        TypeOutText();
    }

    public void Initialize(DialogueContainer dialogueContainer)
    {
        // Activate the Dialogue System
        ShowDialogue(true);

        // Set the dialogue container
        currentDialogue = dialogueContainer;

        // Set the counter on the first line
        currentTextLine = 0;

        UpdatePortrait();

        // Start the dialogue
        CycleLine();
    }

    private void UpdatePortrait()
    {
        portrait.sprite = currentDialogue.actor.portrait;
        nameText.text = currentDialogue.actor.Name;
    }

    private void CycleLine()
    {
        // prepare the dialogue line and reveal length
        lineToShow = currentDialogue.lines[currentTextLine];
        totalTimeToType = lineToShow.Length * timePerLetter;

        // reset the time, visible index and current revealed text
        currentTime = 0f;
        visibleTextPercent = 0f;
        targetText.text = "";

        currentTextLine += 1;
    }

    private void PushText()
    {
        // if press continue while text loading, reveal entire text
        if (visibleTextPercent < 1f)
        {
            visibleTextPercent = 1f;
            UpdateText();
            return;
        }

        // if last line, conclude, else cycle for the next line
        if (currentTextLine >= currentDialogue.lines.Count)
        {
            Conclude();
        }
        else
        {
            CycleLine();
        }
    }

    private void TypeOutText()
    {
        if (visibleTextPercent >= 1f) { return; }

        // calculate the 'progress' for the dialogue line
        currentTime += Time.deltaTime;
        visibleTextPercent = currentTime / totalTimeToType;
        visibleTextPercent = Mathf.Clamp(visibleTextPercent, 0f, 1f);

        UpdateText();
    }

    private void UpdateText()
    {
        // calculate the amount of letters, then reveal
        int letterCount = (int)(lineToShow.Length * visibleTextPercent);
        targetText.text = lineToShow.Substring(0, letterCount);
    }

    private void Conclude()
    {
        // Finish the dialogue
        ShowDialogue(false);
    }

    private void ShowDialogue(bool state)
    {
        gameObject.SetActive(state);
    }
}
