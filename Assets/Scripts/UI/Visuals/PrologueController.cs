using FMOD.Studio;
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

    //[Header("Sound")]
    //[SerializeField] private AudioClip typingSound;
    //[SerializeField] private int frequencyLevel = 2;
    //[SerializeField] private bool stopAudioSource;
    //private AudioSource audioSource;

    // Dialogue Object
    private Story currentStory;
    // Audio
    private EventInstance typingSound;

    // Variable for checking if the last coroutine has ended
    private Coroutine displayLineCoroutine;
    private bool canContinueToNextLine = false;

    private void Start()
    {
        typingSound = AudioManager.Instance.CreateInstance(FMODEvents.Instance.typingEffect);
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
        // Start typing effect
        UpdateSound(true);

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
                // Start typing effect
                UpdateSound(false);
                break;
            }

            //PlayTypingSound(prologueText.text.Count());
            prologueText.text += letter;

            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitForSeconds(0.2f);
        // Start typing effect
        UpdateSound(false);
        canContinueToNextLine = true;
        pressEToContinue.SetActive(true);
    }

    //private void PlayTypingSound(int characterCount)
    //{
    //    if (characterCount % frequencyLevel == 0)
    //    {
    //        if (stopAudioSource)
    //        {
    //            audioSource.Stop();
    //        }
    //        audioSource.PlayOneShot(typingSound);
    //    }
    //}

    private IEnumerator ExitPrologue()
    {
        // Wait for the end of the frame and exit dialogue mode
        yield return new WaitForEndOfFrame();

        GameSceneManager.Instance.InitSwitchScene("LevelOne", new Vector3(-0.5f, -5f, 0f), GameSceneManager.SwitchMode.LoadGame);
        Debug.Log("Prologue Finished");
    }

    private void UpdateSound(bool inDialogue)
    {
        // Start footstep event if the player moves
        if (inDialogue)
        {
            // get the playback state
            PLAYBACK_STATE playbackState;
            typingSound.getPlaybackState(out playbackState);
            if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
            {
                typingSound.start();
            }
        }
        // Stop the footsteps event
        else
        {
            typingSound.stop(STOP_MODE.ALLOWFADEOUT);
        }
    }
}
