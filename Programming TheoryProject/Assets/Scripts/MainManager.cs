using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public List<GameObject> bigShapes = new List<GameObject>();
    public List<GameObject> mediumShapes = new List<GameObject>();
    public List<GameObject> smallShapes = new List<GameObject>();

    private List<GameObject> bigShapesObj = new List<GameObject>();
    private List<GameObject> mediumShapesObj = new List<GameObject>();
    private List<GameObject> smallShapesObj = new List<GameObject>();

    private int nPillars = 3;

    Vector3 LeftPosition = new Vector3(-4.5f, 0, 0);
    Vector3 CenterPosition = Vector3.zero; 
    Vector3 RightPosition = new Vector3(4.5f, 0, 0);

    void Start()
    {
        int nShapes = bigShapes.Count;
        for(int i = 0; i < 3; i++)
        {
            List<GameObject> shapes = bigShapes;

            if (i == 0)
            {
                LeftPosition.y = 0;
                CenterPosition.y = 0;
                RightPosition.y = 0;
            }
            else if (i == 1)
            {
                shapes = mediumShapes;
                LeftPosition.y = 0.001f;
                CenterPosition.y = 0.001f;
                RightPosition.y = 0.001f;
            }
            else if (i == 2)
            {
                shapes = smallShapes;
                LeftPosition.y = 0.02f;
                CenterPosition.y = 0.02f;
                RightPosition.y = 0.02f;
            }

            int indexLeft = 0;
            if(i==1)
            {
                indexLeft = 1;
            }
//            int indexLeft = Random.Range(0, nShapes);
            GameObject gameObjectLeft = Instantiate(shapes[indexLeft], LeftPosition, shapes[indexLeft].transform.rotation);
            int indexCenter = 1;
            if (i == 1)
            {
                indexCenter = 2;
            }
            //            int indexCenter = Random.Range(0, nShapes);
            GameObject gameObjectCenter = Instantiate(shapes[indexCenter], CenterPosition, shapes[indexCenter].transform.rotation);
            int indexRight = 2;
            if (i == 1)
            {
                indexRight = 0;
            }
            //            int indexRight = Random.Range(0, nShapes);
            GameObject gameObjectRight = Instantiate(shapes[indexRight], RightPosition, shapes[indexRight].transform.rotation);

        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
