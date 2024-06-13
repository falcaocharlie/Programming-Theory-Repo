using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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

    private Ray ray;
    private RaycastHit hit;

    private Material goldMaterial;
    private Material frameMaterial;

    private List<GameObject> selectedPile = null;

    void SetPositionForFloor(int floor)
    {
        float val = (float)(floor);
        float y = val * 0.01f;
        for (int i = 0;i<pilePositions.Count;i++)
        {
            float x = pilePositions[i].x;
            float z = pilePositions[i].z;
            pilePositions[i] = new Vector3(x, y, z);
        }
    }

    void SetPilePositions()
    {
        pilePositions.Add(new Vector3(-2f, 0, 1.93f));
        pilePositions.Add(new Vector3(3.3f, 0, 1.93f));
        pilePositions.Add(new Vector3(-2f, 0, -1.93f));
        pilePositions.Add(new Vector3(3.3f, 0, -1.93f));

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
            SetPositionForFloor(i);
            for (int j = 0; j < pilePositions.Count; j++)
            {
                Vector3 pileCenter = pilePositions[j];
                int index = UnityEngine.Random.Range(0, nShapes);
                GameObject gameObject = Instantiate(shapes[index], pileCenter, shapes[index].transform.rotation);
                ShapeScript shapeScript = gameObject.GetComponent<ShapeScript>();
                shapeScript.Floor = i;
                shapeScript.PileNumber = j;
                if (i > 0)
                {
                    shapeScript.LowerScript = pileObjs[j][i - 1].GetComponent<ShapeScript>();
                    Vector3 floorCenter = shapeScript.LowerScript.CenterForNextFloor(pileCenter);
                    gameObject.transform.position = floorCenter;
                }

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
                    ShapeScript shapeScript = hit.collider.gameObject.transform.parent.GetComponent<ShapeScript>();
                    PileWasSelected(shapeScript.PileNumber);
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


    void PileWasSelected(int pileNumber) 
    {
        if (selectedPile == null)
        {
            HighlightPile(pileObjs[pileNumber]);
            selectedPile = pileObjs[pileNumber];
        }
        else
        {
            MatchPiles(pileObjs[pileNumber]);
        }
    }

    void HighlightPile(List<GameObject> newSelectedPile) 
    {
        int numFloors = 3;
        for (int i = 0; i < numFloors; i++)
        {
            GameObject frameObject = newSelectedPile[i].transform.Find("Frame").gameObject;
            frameObject.GetComponent<Renderer>().material = goldMaterial;
        }

    }

    void MatchPiles(List<GameObject> newSelectedPile)
    {
        if (newSelectedPile == null)
        {
            return;
        }
        if (newSelectedPile != selectedPile)
        {
            for (int i = 0; i < numFloors; ++i)
            {
                int number1 = selectedPile[i].GetComponent<ShapeScript>().MyNumber();
                int number2 = newSelectedPile[i].GetComponent<ShapeScript>().MyNumber();
                int newIndex = (number1 + number2) % 3;
                SetPositionForFloor(i);

                List<GameObject> shapes = GetShapesBySize(i);

                ShapeScript oldScript = newSelectedPile[i].GetComponent<ShapeScript>();
                int pileNumber = oldScript.PileNumber;
                Vector3 pileCenter = pilePositions[pileNumber];
                GameObject newShape = Instantiate(shapes[newIndex], pileCenter, shapes[newIndex].transform.rotation);
                ShapeScript newScript = newShape.GetComponent<ShapeScript>();
                newScript.Floor = i;
                newScript.PileNumber = pileNumber;
                if (i > 0)
                {
                    newScript.LowerScript = newSelectedPile[i-1].GetComponent<ShapeScript>();
                    Debug.Log("NEW FLOOR " + i.ToString());
                    Vector3 floorCenter = newScript.LowerScript.CenterForNextFloor(pileCenter);
                    newShape.transform.position = floorCenter;
                }

                Destroy(newSelectedPile[i]);
                newSelectedPile[i] = newShape;
            }
        }
        ResetSelection();
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


}
