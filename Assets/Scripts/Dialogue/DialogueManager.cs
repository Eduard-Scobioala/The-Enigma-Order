using System.Collections;
using UnityEngine;
using Ink.Runtime;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue Params")]
    [SerializeField] private float typingSpeed = 0.04f;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject continueIcon;
    [SerializeField] private TMP_Text dialogueText;

    [Header("Character's Atributtes")]
    [SerializeField] TMP_Text nameText;
    [SerializeField] Image portrait;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TMP_Text[] choicesText;

    private Story currentStory;
    public bool DialogueIsPlaying { get; private set; }

    // Variable for checking if the last coroutine has ended
    private Coroutine displayLineCoroutine;
    private bool canContinueToNextLine = false;

    public static DialogueManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of Dialogue Manager");
        }
        instance = this;
    }

    private void Start()
    {
        DialogueIsPlaying = false;
        dialoguePanel.SetActive(false);

        // get all of the choices text 
        choicesText = new TMP_Text[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TMP_Text>();
            index++;
        }
    }

    private void Update()
    {
        if (!DialogueIsPlaying)
        {
            return;
        }

        if (canContinueToNextLine
            && currentStory.currentChoices.Count == 0
            && Input.GetKeyDown(KeyCode.E))
        {
            ContinueStory();
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON, Actor npcCharacter)
    {
        // Set character's portrait and name
        portrait.sprite = npcCharacter.portrait;
        nameText.text = npcCharacter.Name;

        // Create the story object from the json file
        currentStory = new Story(inkJSON.text);
        DialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        ContinueStory();
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            // Check if the last coroutine has ended
            if (displayLineCoroutine != null)
            {
                StopCoroutine(displayLineCoroutine);
            }

            // Type the text for the current dialogue line, letter by letter
            displayLineCoroutine = StartCoroutine(DisplayLine(currentStory.Continue()));
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }
    }

    private IEnumerator DisplayLine(string line)
    {
        // Empty the dialogue text
        dialogueText.text = "";

        // Buffer the input system
        yield return new WaitForSeconds(0.001f);

        // Hide items while text typing
        continueIcon.SetActive(false);
        HideChoices();

        canContinueToNextLine = false;

        // Display each letter one at a time
        foreach (char letter in line.ToCharArray())
        {
            // If the next line button was pressed, show the entire line
            if (Input.GetKeyDown(KeyCode.E))
            {
                dialogueText.text = line;
                break;
            }

            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        // Show items after text was typed
        continueIcon.SetActive(true);
        DisplayChoices();

        canContinueToNextLine = true;
    }

    private void HideChoices()
    {
        foreach (GameObject choiceButton in choices)
        {
            choiceButton.SetActive(false);
        }
    }

    private IEnumerator ExitDialogueMode()
    {
        // Wait for the end of the frame and exit dialogue mode
        yield return new WaitForEndOfFrame();

        DialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices that UI can support. Choises given: " + currentChoices.Count);
        }

        int index = 0;
        foreach (Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }

        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }

        //StartCoroutine(SelectFirstChoice());
    }

    private IEnumerator SelectFirstChoice()
    {
        // Event System requires we clear it first, then wait
        // for at least one frame before we set the current selected object.
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }

    public void MakeChoice(int choiceIndex)
    {
        if (canContinueToNextLine)
        {
            currentStory.ChooseChoiceIndex(choiceIndex);

            //InputManager.GetInstance().RegisterSubmitPressed();
            // Continue the story from here imstead of updated when we got choices
            ContinueStory();
        }
    }
}
