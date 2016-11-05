using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Class that generates a BSP Dungeon following http://www.roguebasin.com/index.php?title=Basic_BSP_Dungeon_generation
/// </summary>

public class BSPDungeon : MonoBehaviour
{
    private GameObject m_root;
   // private List<Room> leafNodes;

    public float initialW = 100;
    public float initialH = 100;
    public int divisionTimes = 1;

	// Use this for initialization
	void Start ()
    {
        //Create mainRoom
        m_root = new GameObject("Root");
        m_root.transform.SetParent(transform);
        Room roomComponent = m_root.AddComponent<Room>();
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

    public void AddLeafs(List<Room> leafs)
    {
        //Add new leafs to the list
        //Remove parent if it's contained
        foreach(Room r in leafs)
        {
            if (r.IsLeaf())
            {
                leafs.Remove(r.GetParent().GetComponent<Room>());
                leafs.Add(r);
            }
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
