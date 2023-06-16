using Ink.Runtime;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class PrologueController : MonoBehaviour
{
    [Header("Params")]
    [SerializeField] private TextAsset inkJSON;
    [SerializeField] private float bufferTime = 3f;
    [SerializeField] private float typingSpeed = 0.04f;

    [Header("UI")]
    [SerializeField] GameObject prologueScreen;
    [SerializeField] private GameObject pressEToContinue;
    [SerializeField] private TMP_Text prologueText;

    [Header("Sound")]
    [SerializeField] private AudioClip typingSound;
    [SerializeField] private int frequencyLevel = 2;
    [SerializeField] private bool stopAudioSource;
    private AudioSource audioSource;

    private Story currentStory;

    // Variable for checking if the last coroutine has ended
    private Coroutine displayLineCoroutine;
    private bool canContinueToNextLine = false;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void Start()
    {
        StartCoroutine(StartPrologue(inkJSON));
    }

    void Update()
    {
        if (canContinueToNextLine
            && Input.GetKeyDown(KeyCode.E))
        {
            ContinueStory();
        }
    }

    public IEnumerator StartPrologue(TextAsset inkJSON)
    {
        yield return new WaitForSeconds(bufferTime);

        // Create the story object from the json file
        currentStory = new Story(inkJSON.text);

        prologueScreen.SetActive(true);

        ContinueStory();
    }

    private void ContinueStory()
    {
        // Hide the press to continue label
        pressEToContinue.SetActive(false);

        if (currentStory.canContinue)
        {
            // Check if the last coroutine has ended
            if (displayLineCoroutine != null)
            {
                StopCoroutine(displayLineCoroutine);
            }

            // Type the text for the current prologue line, letter by letter
            displayLineCoroutine = StartCoroutine(DisplayLine(currentStory.Continue()));
        }
        else
        {
            StartCoroutine(ExitPrologue());
        }
    }

    private IEnumerator DisplayLine(string line)
    {
        // Empty the dialogue text
        prologueText.text = "";

        // Buffer the input system
        yield return new WaitForSeconds(0.001f);


        canContinueToNextLine = false;

        // Display each letter one at a time
        foreach (char letter in line.ToCharArray())
        {
            // If the next line button was pressed, show the entire line
            if (Input.GetKeyDown(KeyCode.E))
            {
                prologueText.text = line;
                break;
            }

            PlayTypingSound(prologueText.text.Count());
            prologueText.text += letter;

            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitForSeconds(0.2f);
        canContinueToNextLine = true;
        pressEToContinue.SetActive(true);
    }

    private void PlayTypingSound(int characterCount)
    {
        if (characterCount % frequencyLevel == 0)
        {
            if (stopAudioSource)
            {
                audioSource.Stop();
            }
            audioSource.PlayOneShot(typingSound);
        }
    }

    private IEnumerator ExitPrologue()
    {
        // Wait for the end of the frame and exit dialogue mode
        yield return new WaitForEndOfFrame();

        GameSceneManager.Instance.InitSwitchScene("LevelOne", Vector3.zero, GameSceneManager.SwitchMode.LoadGame);
        Debug.Log("Prologue Finished");
    }
}
