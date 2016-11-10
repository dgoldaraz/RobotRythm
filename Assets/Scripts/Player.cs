using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {


    private Vector3 m_direction = Vector3.up;
    public float amountOfMovement = 1.0f;
    public AudioClip[] movementClips;

    private AudioSource m_audioSource;
    private int m_currentSoundIndex;
    private int m_numOfPartitions;
    private List<AudioClip> m_selectedSounds;

	// Use this for initialization
	void Start ()
    {
        m_audioSource = GetComponent<AudioSource>();
        m_selectedSounds = new List<AudioClip>();
        SetRythmValue(1, 0.0f);
        SetUpDir(true);
        RythmGenerator.onTick += Move;
        RythmGenerator.onRythmCreated += SetRythmValue;

	}

    void OnDestroy()
    {
        RythmGenerator.onTick -= Move;
        RythmGenerator.onRythmCreated -= SetRythmValue;
    }
	
	// Update is called once per frame
	void Update ()
    {
        ManageInput();
	}

    void Move()
    {
        transform.position += m_direction * amountOfMovement;
        PlayMoveSound();
    }

    /// <summary>
    /// Manage the input from the user
    /// </summary>
    void ManageInput()
    {
        //Set dir Up
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            SetUpDir(false);
        }

        //Set dir Down
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            SetDownDir(false);
        }

        //Set dir right
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            SetRightDir(false);
        }

        //Set dir left
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            SetLeftDir(false);
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            RythmGenerator rythm = GameObject.FindObjectOfType<RythmGenerator>();
            if (rythm)
            {
                rythm.CreateNewRythm();
            }
        }
    }


    /// <summary>
    /// Set the direction of the ant row heading Up
    /// </summary>
    /// <param name="force">flag that forces to set the direction</param>
    public void SetUpDir(bool force)
    {
        //Avoid change movement on the contrary direction
        if (m_direction != Vector3.down || force)
        {
            m_direction = Vector3.up;
            transform.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
        }
    }

    /// <summary>
    /// Set the direction of the ant row heading Down
    /// </summary>
    /// <param name="force">flag that forces to set the direction</param>
    public void SetDownDir(bool force)
    {
        //Avoid change movement on the contrary direction
        if (m_direction != Vector3.up || force)
        {
            m_direction = Vector3.down;
            transform.localEulerAngles = new Vector3(0.0f, 0.0f, 180.0f);
        }
    }

    /// <summary>
    /// Set the direction of the ant row heading Right
    /// </summary>
    /// <param name="force">flag that forces to set the direction</param>
    public void SetRightDir(bool force)
    {
        //Avoid change movement on the contrary direction
        if (m_direction != Vector3.left || force)
        {
            m_direction = Vector3.right;
            transform.localEulerAngles = new Vector3(0.0f, 0.0f, -90.0f);
        }
    }

    /// <summary>
    /// Set the direction of the ant row heading Left
    /// </summary>
    /// <param name="force">flag that forces to set the direction</param>
    public void SetLeftDir(bool force)
    {
        //Avoid change movement on the contrary direction
        if (m_direction != Vector3.right || force)
        {
            m_direction = Vector3.left;
            transform.localEulerAngles = new Vector3(0.0f, 0.0f, 90.0f);
        }
    }


    void PlayMoveSound()
    {
        m_currentSoundIndex = m_currentSoundIndex % m_selectedSounds.Count;
        PlaySound(m_selectedSounds[m_currentSoundIndex]);
        m_currentSoundIndex++;
    }

    void PlaySound(AudioClip clip)
    {
        
        m_audioSource.PlayOneShot(clip);
    }

    void SetRythmValue(int rythmPartition, float maxTime)
    {
        m_numOfPartitions = rythmPartition;
        m_currentSoundIndex = 0;
        m_selectedSounds.Clear();
        for(int i = 0; i < m_numOfPartitions; i++)
        {
            //Add random Sounds
            m_selectedSounds.Add(movementClips[Random.Range(0, movementClips.Length)]);
        }
    }
}
