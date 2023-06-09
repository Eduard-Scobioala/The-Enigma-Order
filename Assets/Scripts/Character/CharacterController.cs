using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMOD.Studio;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController : MonoBehaviour, IDataPersistance
{
    [SerializeField] float speed = 2.0f;

    Rigidbody2D rigidbody2d;
    Vector2 motionVector;
    Animator animator;

    public Vector2 lastMotionVector;
    public bool moving;

    // Audio
    private EventInstance playerFootsteps;

    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        playerFootsteps = AudioManager.Instance.CreateInstance(FMODEvents.Instance.playerFootsteps);
    }

    #region Movement

    private void Update()
    {
        // If dialogue playing, freeze the player's movement
        if (!GameManager.instance.characterCanMove || DialogueManager.instance.DialogueIsPlaying)
        {
            return;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        motionVector = new Vector2(horizontal, vertical);

        animator.SetFloat("horizontal", horizontal);
        animator.SetFloat("vertical", vertical);

        moving = horizontal != 0f || vertical != 0f;
        animator.SetBool("moving", moving);

        if (moving)
        {
            lastMotionVector = new Vector2(horizontal, vertical).normalized;

            animator.SetFloat("lastHorizontal", horizontal);
            animator.SetFloat("lastVertical", vertical);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // save the game anytime before loading a new scene
            DataPersistanceManager.Instance.SaveGame();

            // load the main menu scene
            SceneManager.LoadSceneAsync("MainMenu");
        }
    }

    void FixedUpdate()
    {
        Move();
        UpdateSound();
    }

    private void Move()
    {
        rigidbody2d.velocity = motionVector * speed;
    }

    #endregion

    #region Save&Load

    public void LoadData(GameData data)
    {
        transform.position = data.playerPosition;
    }

    public void SaveData(GameData data)
    {
        data.playerPosition = transform.position;
    }

    #endregion

    private void UpdateSound()
    {
        // Start footstep event if the player moves
        if (moving)
        {
            // get the playback state
            PLAYBACK_STATE playbackState;
            playerFootsteps.getPlaybackState(out playbackState);
            if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
            {
                playerFootsteps.start();
            }
        }
        // Stop the footsteps event
        else
        {
            playerFootsteps.stop(STOP_MODE.ALLOWFADEOUT);
        }
    }
}
