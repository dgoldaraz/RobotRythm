using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Class that generates a BSP Dungeon following http://www.roguebasin.com/index.php?title=Basic_BSP_Dungeon_generation
/// </summary>

public class BSPDungeon : MonoBehaviour
{

    private Room m_root;
    private List<Room> leafNodes;

    public float initialW = 100;
    public float initialH = 100;
    public int divisionTimes = 1;

	// Use this for initialization
	void Start () {
	
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
                leafs.Remove(r.GetParent());
                leafs.Add(r);
            }
        }
    }

    void DivideRooms()
    {
        for(int i = 0; i < divisionTimes; i++)
        {
            m_root.Divide();
        }
    }

}
