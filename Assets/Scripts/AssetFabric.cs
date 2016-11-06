using UnityEngine;
using System.Collections;

public class AssetFabric : MonoBehaviour
{

    public GameObject quadTemplate;
    public Material bricksMaterial;


    public GameObject GetRandomRoom(float w, float h, Vector3 center)
    {
        if(quadTemplate)
        {
            GameObject newQuad = Instantiate(quadTemplate, center, Quaternion.identity) as GameObject;
            newQuad.transform.localScale = new Vector3(w, h, 1.0f);
            Material newMat = newQuad.GetComponent<MeshRenderer>().material;
            newMat.color = new Color(Random.value, Random.value, Random.value);
            newMat.SetTextureScale("_MainTex", new Vector2(w, h));
            newQuad.GetComponent<MeshRenderer>().material = newMat;
            CreateWalls(w, h, center, newQuad);
            return newQuad;
        }
        else
        {
            Debug.LogError("No QUad Error");
            return null;
        }
    }

    void CreateWalls( float w, float h, Vector3 center, GameObject floor)
    {
        //Create for Quads for each wall
        float mHeight = h * 0.5f;
        float mWidth = w * 0.5f;
        Vector3 hVector = new Vector3(0.0f, mHeight, 0.0f);
        Vector3 wVector = new Vector3(mWidth, 0.0f, 0.0f);
        //Top/Bottom
        Vector3 topVector = center + hVector;
        Vector3 bottomVector = center - hVector;

        //Left Right
        Vector3 leftVector = center - wVector;
        Vector3 rightVector = center + wVector;

        //Set Z
        topVector.z = -0.1f;
        bottomVector.z = -0.1f;
        leftVector.z = -0.1f;
        rightVector.z = -0.1f;

        GameObject topQuad = CreateHorizontalWall(w, topVector);
        GameObject bottomQuad = CreateHorizontalWall(w, bottomVector);

        GameObject leftQuad = CreateVerticalWall(h, leftVector);
        GameObject rightQuad = CreateVerticalWall(h, rightVector);

        topQuad.transform.SetParent(floor.transform);
        bottomQuad.transform.SetParent(floor.transform);
        leftQuad.transform.SetParent(floor.transform);
        rightQuad.transform.SetParent(floor.transform);
    }

    GameObject CreateHorizontalWall(float width, Vector3 center)
    {
        GameObject wall = Instantiate(quadTemplate, center, Quaternion.identity) as GameObject;
        wall.transform.localScale = new Vector3(width, 1.0f, 1.0f);
        Material vMaterial = Instantiate(bricksMaterial) as Material;
        vMaterial.SetTextureScale("_MainTex", new Vector2(width, 1.0f));
        wall.GetComponent<MeshRenderer>().material = vMaterial;

        return wall;
    }

    GameObject CreateVerticalWall( float height, Vector3 center)
    {
        GameObject wall = Instantiate(quadTemplate, center, Quaternion.identity) as GameObject;
        wall.transform.localScale = new Vector3(1.0f, height, 1.0f);
        Material hMaterial = Instantiate(bricksMaterial) as Material;
        hMaterial.SetTextureScale("_MainTex", new Vector2(1.0f, height));
        wall.GetComponent<MeshRenderer>().material = hMaterial;

        return wall;
    }
}
