using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 normal { get; private set; }
    public Vector3 PointA;
    public Vector3 PointB;
    public Vector3 PointC;

    public float D;
    void Start()
    {
        normal = FindNormal(PointA, PointB, PointC);
        Debug.Log(gameObject.name + " normal is " + normal);
        //GameObject sphere = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere)) as GameObject;
        //Vector3 position = this.gameObject.transform.position + normal*5;
        //sphere.transform.position = position;
        //sphere.gameObject.name = this.name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //public float DistanceToPlane(Vector3 normal, float D,Vector3 point)
    //{
    //    float output = Mathf.Abs(Vector3.Dot(normal, point) + D) / n.magnitude);
    //    return output;
    //}
    public float ScalarMultiplication(Vector3 a, Vector3 b)
    {
        return a.x * b.x + a.y * b.y + a.z * b.z;
    }
    private float Determinant(float a, float b, float c, float d)
    {
        return a * c - b * d;
    }
    private Vector3 FindNormal(Vector3 a, Vector3 b, Vector3 c)
    {
        Vector3 output = -Vector3.Normalize(Vector3.Cross(b - a, c - a));
        //Debug.Log(gameObject.name + "(" + (b - a).x + "," + (b - a).y + "," + (b - a).z + ")*(" + (c - a).x + "," + (c - a).y + "," + (c - a).z + ")=(" + output.x + "," + output.y + "," + output.z + ")");
        return output;
    }
}

