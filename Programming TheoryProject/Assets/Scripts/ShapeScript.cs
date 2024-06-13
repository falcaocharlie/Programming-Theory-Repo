using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public abstract class ShapeScript : MonoBehaviour
{
    // Start is called before the first frame update
    public abstract int MyNumber();
    protected ShapeScript lowerScript = null;

    public ShapeScript LowerScript   // property
    {
        get { return lowerScript; }   // get method
        set { lowerScript = value; }  // set method
    }

    [SerializeField] 
    protected int floor = -1;

    public int Floor // property
    {
        get { return floor; }   // get method
        set { floor = value; }  // set method
    }

    [SerializeField]
    protected int pileNumber = -1;
    public int PileNumber // property
    {
        get { return pileNumber; }   // get method
        set { pileNumber = value; }  // set method
    }

    public abstract Vector3 CenterForNextFloor(Vector3 pileCenter);
    
    public void Update()
    {
        
    }
}
