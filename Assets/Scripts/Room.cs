using UnityEngine;
using System.Collections;

public class Room : MonoBehaviour {


    private Room m_parent;
    private Room m_lChild;
    private Room m_rChild;

    private float m_width;
    private float m_height;
	

    public bool IsLeaf()
    {
        return m_lChild == null && m_rChild == null;
    }

    public bool IsRoot()
    {
        return m_parent == null;
    }

    public float GetWidth()
    {
        return m_width;
    }

    public float GetHeight()
    {
        return m_height;
    }

    public Room GetParent()
    {
        return m_parent;
    }

    public Room GetLChildren()
    {
        return m_lChild;
    }

    public Room GetRChildren()
    {
        return m_rChild;
    }

    public void SetParent(Room p)
    {
        m_parent = p;
    }

    public void SetLChild(Room c)
    {
        m_lChild = c;
    }

    public void SetRChild(Room c)
    {
        m_rChild = c;
    }

    public void Divide()
    {
        //Divide the room in another two rooms
        if(IsLeaf())
        {
            //Divide in two new rooms
        }
        else
        {
            m_lChild.Divide();
            m_rChild.Divide();
        }
    }
}
