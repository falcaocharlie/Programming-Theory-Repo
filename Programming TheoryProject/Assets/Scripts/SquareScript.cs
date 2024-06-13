using UnityEngine;

public class SquareScript : ShapeScript
{
    public override int MyNumber()
    {
        return 2;
    }

    public override Vector3 CenterForNextFloor(Vector3 center)
    {
        Debug.Log("Floor " + Floor.ToString());
        Debug.Log("Square in " + center.ToString());
        if ( lowerScript !=null )
        {
            center = lowerScript.CenterForNextFloor(center);
        }
        Debug.Log("Square out " + center.ToString());
        return center;
    }


}
