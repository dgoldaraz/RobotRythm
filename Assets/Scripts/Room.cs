using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : MonoBehaviour
{

    struct Door
    {
        Vector2 center; //
        float size; // width/height
        char position; // Sets wich position is the door, l -left, r- right, u - up, d - down

    };
    private AssetFabric m_assetFabric;
    private GameObject m_parent;
    private GameObject m_lChild;
    private GameObject m_rChild;

    private float m_width = 100.0f;
    private float m_height = 100.0f;
    private Vector3 m_center;

    private GameObject m_quad;

    private List<Door> m_doors;
	
    public void Init()
    {

        m_assetFabric = GameObject.FindObjectOfType<AssetFabric>();
        if(m_assetFabric == null)
        {
            Debug.LogError("No asset Fabric");
        }
    }

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

    public void SetWidth(float w)
    {
        m_width = w;
    }

    public void SetHeight(float h)
    {
        m_height = h;
    }

    public Vector3 GetCenter()
    {
        return m_center;
    }

    public void SetCenter(Vector3 c)
    {
        m_center = c;
    }

    public GameObject GetParent()
    {
        return m_parent;
    }

    public GameObject GetLChild()
    {
        return m_lChild;
    }

    public GameObject GetRChild()
    {
        return m_rChild;
    }

    void CreateChild(Vector3 center,float w, float h, bool left)
    {
        Room component = null;
        if (left)
        {
            if (m_lChild == null)
            {
                m_lChild = new GameObject("LChild");
                m_lChild.transform.SetParent(transform);
                component = m_lChild.AddComponent<Room>();
            }
        }
        else
        {
            if (m_rChild == null)
            {
                m_rChild = new GameObject("RChild");
                m_rChild.transform.SetParent(transform);
                component = m_rChild.AddComponent<Room>();
            }
        }
        if(component)
        {
            component.Init();
            component.SetParent(gameObject);
            component.SetCenter(center);
            component.SetWidth(w);
            component.SetHeight(h);
        }
    }

    public void SetParent(GameObject p)
    {
        m_parent = p;
        transform.SetParent(p.transform);
    }

    public void SetLChild(GameObject c)
    {
        m_lChild = c;
        m_lChild.transform.SetParent(transform);
    }

    public void SetRChild(GameObject c)
    {
        m_rChild = c;
        m_rChild.transform.SetParent(transform);
    }

    public void Divide()
    {
        //Divide the room in another two rooms
        if(IsLeaf())
        {
            //Divide in two new rooms
            //Get a Random Direction
            if(Random.value > 0.5f)
            {
                //Horizontal
                DivideHorizontally();
            }
            else
            {
                //Vertical
                DivideVertically();
            }
        }
        else
        {
            m_lChild.GetComponent<Room>().Divide();
            m_rChild.GetComponent<Room>().Divide();
        }
    }

    /// <summary>
    /// Divide the room in a random Horizontal positions (random Y value)
    /// </summary>
    void DivideHorizontally()
    {
        //Calculate offset
        int offset = (int)(m_height * 0.1);
        //if(m_height - offset * 2 > 5)
        {
            //We are able to create a room
            float randomY = Random.Range(offset, (int)m_height - offset);
            //Divide and create two new rooms

            Vector3 bottom = m_center - new Vector3(0.0f, m_height * 0.5f, 0.0f);
            float lHeight = randomY;
            float rHeight = m_height - randomY;

            Vector3 upCenter = bottom + new Vector3(0.0f, rHeight + lHeight * 0.5f, 0.0f);
            Vector3 downCenter = bottom + new Vector3(0.0f, rHeight * 0.5f, 0.0f);

            CreateChild(upCenter, m_width, lHeight, true);
            CreateChild(downCenter, m_width, rHeight, false);
        }
    }

    /// <summary>
    /// Divide the room in a random Horizontal positions (random X value)
    /// </summary>
    void DivideVertically()
    {
        //Calculate offset
        int offset = (int)(m_width * 0.1);
        //if (m_width - offset * 2 > 20)
        {
            //We are able to create a room
            float randomX = Random.Range(offset, (int)m_width - offset);
            //Divide and create two new rooms
            float lWidth = randomX;
            float rWidth = m_width - randomX;
            Vector3 leftMost = m_center - new Vector3(m_width * 0.5f, 0.0f, 0.0f);

            Vector3 lCenter = leftMost + new Vector3(lWidth * 0.5f, 0.0f, 0.0f);
            Vector3 rCenter = leftMost + new Vector3(lWidth + rWidth * 0.5f , 0.0f, 0.0f);

            CreateChild(lCenter, lWidth, m_height, true);
            CreateChild(rCenter, rWidth, m_height, false);
        }
    }

    public void Create()
    {
        //Create a quad and give the position and w/h
        if(IsLeaf())
        {
            m_quad = m_assetFabric.GetRandomRoom(m_width, m_height, m_center);
            m_quad.transform.SetParent(transform);
            BSPDungeon dungeon = GameObject.FindObjectOfType<BSPDungeon>();
            dungeon.AddLeaf(this);
        }
        else
        {
            m_lChild.GetComponent<Room>().Create();
            m_rChild.GetComponent<Room>().Create();
        }
    }


}
