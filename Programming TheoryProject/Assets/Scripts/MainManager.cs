using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.ProBuilder.Shapes;

public class MainManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> bigShapes = new List<GameObject>();

    public List<GameObject> mediumShapes = new List<GameObject>();
    public List<GameObject> smallShapes = new List<GameObject>();


    private const int numFloors = 3;

    private List<Vector3> pilePositions = new List<Vector3>();
    private List<List<GameObject>> pileObjs = new List<List<GameObject>>();

    private Vector3 CenterPosition = new Vector3(0, 0, 1.8f);
    private Vector3 RightPosition = new Vector3(3.3f, 0, -1.87f);
    private Ray ray;
    private RaycastHit hit;

    private Material goldMaterial;
    private Material frameMaterial;

    private List<GameObject> selectedPile = null;

    void SetPositionHeight(int index)
    {
        for(int i = 0;i<pilePositions.Count;i++)
        {
            float x = pilePositions[i].x;
            float z = pilePositions[i].z;
            pilePositions[i] = new Vector3(x, (float)(index) * 0.01f, z);
        }
    }

    void SetPilePositions()
    {
        pilePositions.Add(new Vector3(-2f, 0, 1.87f));
        pilePositions.Add(new Vector3(3.3f, 0, 1.87f));
        pilePositions.Add(new Vector3(-2f, 0, -1.87f));
        pilePositions.Add(new Vector3(3.3f, 0, -1.87f));
        pileObjs = new List<List<GameObject>>();
        for (int j = 0; j < pilePositions.Count; j++)
        {
            pileObjs.Add(new List<GameObject>());
        }
    }

    void AddShapes()
    {
        int nShapes = bigShapes.Count;
        for (int i = 0; i < numFloors; i++)
        {
            List<GameObject> shapes = GetShapesBySize(i);
            SetPositionHeight(i);

            for (int j = 0; j < pilePositions.Count; j++)
            {
                int index = Random.Range(0, nShapes);
                GameObject gameObject = Instantiate(shapes[index], pilePositions[j], shapes[index].transform.rotation);
                pileObjs[j].Add(gameObject);
            }
        }


    }

    void LoadResources()
    {
        goldMaterial = Resources.Load("Materials/MatGold", typeof(Material)) as Material;
        frameMaterial = Resources.Load("Materials/MatFrame", typeof(Material)) as Material;
    }

    void Start()
    {
        SetPilePositions();
        AddShapes();
        LoadResources();    
    }

    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (hit.collider.CompareTag("Shape"))
                {
                    if (selectedPile == null)
                    {
                        HighlightPile(hit.collider.transform.position);
                    }
                    else
                    { 
                        MatchPiles(hit.collider.transform.position);
                    }
                }
                else
                {
                    ResetSelection();
                }
            }
            else if (Input.GetMouseButtonDown(1))
            {
                ResetSelection();
            }
        }
    }


    void MatchPiles(Vector3 position)
    {

        List<GameObject> newSelectedPile = FindSelectedPile(position);
        if (newSelectedPile != selectedPile)
        {
            for (int i = 0; i < numFloors; ++i)
            {
                int number1 = selectedPile[i].GetComponent<ShapeScript>().MyNumber();
                int number2 = newSelectedPile[i].GetComponent<ShapeScript>().MyNumber();
                int newIndex = (number1 + number2) % 3;

                List<GameObject> shapes = GetShapesBySize(i);

                Destroy(newSelectedPile[i]);
                newSelectedPile[i] = Instantiate(shapes[newIndex], newSelectedPile[i].transform.position, shapes[newIndex].transform.rotation);
            }
        }
        ResetSelection();
    }

    List<GameObject> GetShapesBySize(int index)
    {
        switch (index)
        {
            case 0:
                return bigShapes;
            case 1:
                return mediumShapes;
            case 2:
                return smallShapes;
            default:
                return bigShapes;
        }
    }

    List<GameObject> FindSelectedPile(Vector3 position)
    {
        List<GameObject> pile = pileObjs[0];
        float minDistance = (position - pilePositions[0]).magnitude;
        for (int i = 1; i<pilePositions.Count; ++i)
        { 
            float distance = (position - pilePositions[i]).magnitude;
            if (minDistance > distance)
            {
                minDistance = distance;
                pile = pileObjs[i];
            }
        }
        return pile;
    }

    void HighlightPile(Vector3 position) 
    {
        selectedPile = FindSelectedPile(position);
        int numFloors = 3;
        for (int i = 0; i < numFloors; i++)
        {
            GameObject frameObject = selectedPile[i].transform.Find("Frame").gameObject;
            frameObject.GetComponent<Renderer>().material = goldMaterial;
        }

    }

    void ResetSelection()
    {
        for(int i = 0;i<pileObjs.Count; ++i) 
        {
            ResetPile(pileObjs[i]);
        }
        selectedPile = null;
    }

    void ResetPile(List<GameObject> pileList)
    {
        int numFloors = 3;
        for (int i = 0; i < numFloors; i++)
        {
            GameObject frameObject = pileList[i].transform.Find("Frame").gameObject;
            frameObject.GetComponent<Renderer>().material = frameMaterial;
        }

    }

}
