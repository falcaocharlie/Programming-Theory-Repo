using UnityEngine;

public class TriangleScript : ShapeScript
{
    public override int MyNumber()
    {
        return 1;
    }

    public override Vector3 CenterForNextFloor(Vector3 center)
    {
        Debug.Log("Floor " + Floor.ToString());
        Debug.Log("Triangle in " + center.ToString());
        if (lowerScript != null)
        {
            center = lowerScript.CenterForNextFloor(center);
        }

        Vector3 result = new Vector3();
        switch (floor)
        {
            case 0:
                Debug.Log("cambiato");
                result = new Vector3(center.x, center.y, center.z - 0.4f);
                break;
            case 1:
                result = new Vector3(center.x, center.y, center.z - 0.1f);
                break;
            case 2:
                result = new Vector3(center.x, center.y, center.z - 0.0f);
                break;
        }
        Debug.Log("Triangle out " + result.ToString());
        return result;
    }

}
