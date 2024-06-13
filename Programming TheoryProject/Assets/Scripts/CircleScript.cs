using UnityEngine;

public class CircleScript : ShapeScript
{
    public override int MyNumber()
    {
        return 0;
    }

    public override Vector3 CenterForNextFloor(Vector3 center)
    {
        Debug.Log("Floor " + Floor.ToString());
        Debug.Log("Circle in " + center.ToString());
        if (lowerScript != null)
        {
            center = lowerScript.CenterForNextFloor(center);
        }

        Debug.Log("Circle out " + center.ToString());
        return center;
    }


}
