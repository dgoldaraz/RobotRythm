using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Class that generates a BSP Dungeon following http://www.roguebasin.com/index.php?title=Basic_BSP_Dungeon_generation
/// </summary>

public class BSPDungeon : MonoBehaviour
{
    private GameObject m_root;
    private List<Room> leafNodes;

    public float initialW = 100;
    public float initialH = 100;
    public int divisionTimes = 1;

	// Use this for initialization
	void Start ()
    {
        leafNodes = new List<Room>();
        //Create mainRoom
        m_root = new GameObject("Root");
        m_root.transform.SetParent(transform);
        Room roomComponent = m_root.AddComponent<Room>();
        roomComponent.Init();
        //Instantiate(roomTemplate, transform.position, Quaternion.identity) as Room;
        roomComponent.SetWidth(initialW);
        roomComponent.SetHeight(initialH);
        roomComponent.SetCenter(transform.position);
        DivideRooms();
        
        roomComponent.Create();
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void AddLeaf(Room leaf)
    {
        //Add new leafs to the list
        //Remove parent if it's contained
        if (leaf.IsLeaf())
        {
            leafNodes.Remove(leaf.GetParent().GetComponent<Room>());
            leafNodes.Add(leaf);
        }
    }

    void DivideRooms()
    {
        for(int i = 0; i < divisionTimes; i++)
        {
            m_root.GetComponent<Room>().Divide();
        }
    }

}
