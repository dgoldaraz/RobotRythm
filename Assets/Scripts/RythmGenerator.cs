using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RythmGenerator : MonoBehaviour {


    public delegate void TickMovement();
    public static event TickMovement onTick;

    public delegate void RythmCreated(int partition, float maxTime);
    public static event RythmCreated onRythmCreated;

    public List<float> m_times;
    public float maxTime = 5.0f;


    private int m_cPosition;
    private float m_amountOfTime = 0.0f;
    private float m_cMaxTime = 0.0f;
    private int m_cPartitions = 0;


	// Use this for initialization
	void Start ()
    {
        m_times = new List<float>();
        CreateNewRythm();
	}
	
	// Update is called once per frame
	void Update ()
    {
        CheckTime();
	}

    void CheckTime()
    {
        if (m_amountOfTime >= m_times[m_cPosition])
        {
            Tick();
            m_amountOfTime = 0;
            m_cPosition++;
            m_cPosition %= m_times.Count;
        }
        else
        {
            m_amountOfTime += Time.deltaTime;
        }
    }

    void Tick()
    {
        if(onTick != null)
        {
            onTick();
        }
    }

    public void CreateNewRythm()
    {
        //Clean times list
        m_times.Clear();
        //For now just add one
        m_cMaxTime = Random.Range(1.0f, maxTime);
        m_cPartitions = Random.Range((int)(m_cMaxTime * 0.5f) + 2, (int)m_cMaxTime * 3 + 1);
        float minSpace = 0.0f;
        float minSize = m_cMaxTime * 0.1f;
        for(int i = 0; i < m_cPartitions; i++)
        {
            float space = m_cMaxTime / (m_cPartitions - i);
            float value = Random.Range(minSpace, space);
            if(value < minSize)
            {
                value = minSize;
            }
            m_times.Add(value - minSpace);
            minSpace = value;
        }
        m_amountOfTime = 0.0f;
        m_cPosition = 0;

        if(onRythmCreated != null)
        {
            onRythmCreated(m_cPartitions, m_cMaxTime);
        }
    }
}
