using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Press : MonoBehaviour
{
    // Start is called before the first frame update
    public bool move = false;
    public float mass=10f;
    public float pressure = 1f;
    public float lowCoeficient = 1f;
    public Vector3 speed = Vector3.zero;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!move)
            return;
        UseForce(Vector3.down*pressure) ;
        Translate();

    }
    public void ChangeImpuls(Vector3 deltaP)
    {
        if (!move)
            return;
        Vector3 deltaV = (1 / mass) * deltaP;
        speed = speed + deltaV;
    }
    public void UseForce(Vector3 force)
    {
        if (!move)
            return;
        Vector3 dv = ((force / mass) * Time.deltaTime) / lowCoeficient;
        speed = speed + dv;
    }
    public void Translate()
    {
        if (!move)
            return;
        transform.position += (speed * Time.deltaTime) / lowCoeficient;
    }
}
