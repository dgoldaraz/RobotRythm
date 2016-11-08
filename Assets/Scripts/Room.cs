using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : MonoBehaviour
{
    [System.Serializable]
    public struct Door
    {
        public Vector3 center; //
        public float size; // width/height
        public char position; // Sets wich position is the door, l -left, r- right, u - up, d - down
    };

    private AssetFabric m_assetFabric;
    private GameObject m_parent;
    private GameObject m_lChild;
    private GameObject m_rChild;
    private GameObject m_brother;

    private float m_width = 100.0f;
    private float m_height = 100.0f;
    private Vector3 m_center;

    private GameObject m_quad;
    public List<Door> m_doors;
    private bool m_divideH = false; //Stores if the division is Horizontal or Vertical
    private float m_divisionPos = 0.0f; // Stores where the division occurs
    private bool m_doorsCreated = false;
	
    public void Init()
    {
        m_doors = new List<Door>();
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

    public GameObject GetBrother()
    {
        return m_brother;
    }

    public void SetBrother(GameObject brother)
    {
        m_brother = brother;
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
        BSPDungeon dungeon = GameObject.FindObjectOfType<BSPDungeon>();
        dungeon.AddLeaf(component);
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

        //We are able to create a room
        BSPDungeon dungeon = GameObject.FindObjectOfType<BSPDungeon>();
        float minSize = dungeon.GetMinSize();
        if (m_height > minSize)
        {
            float randomY = Random.Range(minSize, (int)m_height - minSize);
            m_divideH = true;
            m_divisionPos = m_height - randomY;

            //Divide and create two new rooms
            Vector3 bottom = m_center - new Vector3(0.0f, m_height * 0.5f, 0.0f);
            float lHeight = randomY;
            float rHeight = m_height - randomY;

            Vector3 upCenter = bottom + new Vector3(0.0f, rHeight + lHeight * 0.5f, 0.0f);
            Vector3 downCenter = bottom + new Vector3(0.0f, rHeight * 0.5f, 0.0f);

            CreateChild(upCenter, m_width, lHeight, true);
            CreateChild(downCenter, m_width, rHeight, false);
            m_lChild.GetComponent<Room>().SetBrother(m_rChild);
            m_rChild.GetComponent<Room>().SetBrother(m_lChild);
        }
    }

    /// <summary>
    /// Divide the room in a random Horizontal positions (random X value)
    /// </summary>
    void DivideVertically()
    {
        //We are able to create a room
        BSPDungeon dungeon = GameObject.FindObjectOfType<BSPDungeon>();
        float minSize = dungeon.GetMinSize();
        if (m_width > minSize)
        {
            //We are able to create a room
            float randomX = Random.Range(minSize, (int)m_width - minSize);
            m_divideH = false;
            m_divisionPos = randomX;

            //Divide and create two new rooms
            float lWidth = randomX;
            float rWidth = m_width - randomX;
            Vector3 leftMost = m_center - new Vector3(m_width * 0.5f, 0.0f, 0.0f);

            Vector3 lCenter = leftMost + new Vector3(lWidth * 0.5f, 0.0f, 0.0f);
            Vector3 rCenter = leftMost + new Vector3(lWidth + rWidth * 0.5f , 0.0f, 0.0f);

            CreateChild(lCenter, lWidth, m_height, true);
            CreateChild(rCenter, rWidth, m_height, false);
            m_lChild.GetComponent<Room>().SetBrother(m_rChild);
            m_rChild.GetComponent<Room>().SetBrother(m_lChild);
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

            foreach(Door d in m_doors)
            {
                //Create a door
                GameObject door;
                if(d.position == 'u' || d.position == 'd')
                {
                    door = m_assetFabric.GetDoor(d.center, d.size, false);
                }
                else
                {
                    door = m_assetFabric.GetDoor(d.center, d.size, true);
                }

                if(door != null)
                {
                    door.transform.SetParent(transform);
                }
            }
        }
        else
        {

            m_lChild.GetComponent<Room>().Create();
            m_rChild.GetComponent<Room>().Create();
        }
    }

    public void CreateChildrenDoors()
    {
        if(!m_doorsCreated && !IsLeaf())
        {
            m_doorsCreated = true;
            //Create a door randomly in the line defined by division pos
            if(m_divideH)
            {
                float randomX = Random.Range(4.0f, m_width - 4.0f);
                //Calculate center using randomX and m_divPos
                Vector3 letfBottomMost = m_center - new Vector3(m_width * 0.5f, m_height * 0.5f, 0.0f);
                Vector3 center = letfBottomMost + new Vector3(randomX, m_divisionPos, 0.0f);
                center.z = -0.2f;
                //Create up/down door
                Door upDoor = CreateDoor('u', center, 4.0f);
                Door downDoor = CreateDoor('d', center, 4.0f);
                m_lChild.GetComponent<Room>().AddDoor(downDoor);
                m_rChild.GetComponent<Room>().AddDoor(upDoor);
            }
            else
            {
                //Create left/right door
                float randomY = Random.Range(4.0f, m_height - 4.0f);
                //Calculate center using randomX and m_divPos
                Vector3 topBottomMost = m_center - new Vector3(m_width * 0.5f, m_height * 0.5f, 0.0f);
                Vector3 center = topBottomMost + new Vector3(m_divisionPos, randomY, 0.0f);
                center.z = -0.2f;
                //Create up/down door
                Door rightDoor = CreateDoor('r', center, 4.0f);
                Door leftDoor = CreateDoor('l', center, 4.0f);
                m_rChild.GetComponent<Room>().AddDoor(leftDoor);
                m_lChild.GetComponent<Room>().AddDoor(rightDoor);

            }
        }
    }

    Door CreateDoor(char pos, Vector3 center, float size)
    {
        Door newDoor;
        newDoor.position = pos;
        newDoor.center = center;
        newDoor.size = size;

        return newDoor;
    }

    public void AddDoor(Door door)
    {
        m_doors.Add(door);
        if(m_lChild != null)
        {
            m_lChild.GetComponent<Room>().AddDoor(door);
        }
        if(m_rChild != null)
        {
            m_rChild.GetComponent<Room>().AddDoor(door);
        }
    }

}
