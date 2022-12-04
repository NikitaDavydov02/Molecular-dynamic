using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomWall : Wall
{
    public float k;
    [SerializeField]
    public Vector3 speed;
    public float mass;
    public float forgetTime = 0.05f;
    public List<GameObject> lastCollisionedObjects = new List<GameObject>();
    public float lowCoeficient = 1f;
    private Vector3 startPosition;
    float middleDeltaY;
    int nOfCounts = 0;
    // Start is called before the first frame update
    void Start()
    {
        speed = Vector3.zero;
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        UseForce(k * (startPosition - transform.position));
        Translate();

        nOfCounts++;
        middleDeltaY = (middleDeltaY * (nOfCounts - 1) + (startPosition.y - transform.position.y)) / nOfCounts;
        Debug.Log("MiddleDeltaY = " + middleDeltaY);
    }

    public void ChangeImpuls(Vector3 deltaP)
    {
        Vector3 deltaV = (1 / mass) * deltaP;
        speed = speed + deltaV;
    }
    public void UseForce(Vector3 force)
    {
        Vector3 dv = ((force / mass) * Time.deltaTime) / lowCoeficient;
        speed = speed + dv;
    }
    public void Translate()
    {
        transform.position += (speed * Time.deltaTime) / lowCoeficient;
    }
}
