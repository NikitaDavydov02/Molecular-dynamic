using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Molecule : MonoBehaviour
{
    // Start is called before the first frame update
    public float CollisionRadius { get; private set; } = 0.5f;
    public float Mass { get; private set; } = 1;
    public Vector3 speed { get; private set; } = Vector3.zero;
    //public float forgetTime = 0.05f;
   // public List<GameObject> lastCollisionedObjects = new List<GameObject>();
    //public float lowCoeficient = 1f;
    
    public float Temperature { get { return speed.magnitude* speed.magnitude; }}
    [SerializeField]
    List<Material> temperatureTextures;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Translate();
        //if (MoleculeManager.UseGravity)
        //    UseForce(Mass * MoleculeManager.g);
        Collider[] nearColliders = Physics.OverlapSphere(transform.position, CollisionRadius);
        
        foreach (Collider collider in nearColliders)
        {
            if (collider.gameObject.GetComponent<Wall>())
            {
                Wall wallScript = collider.gameObject.GetComponent<Wall>();
                //Debug.Log(wallScript.normal);
                if (Vector3.Dot(wallScript.normal, speed) <= 0)
                {
                    //Столкновение со стеной
                    Vector3 n = wallScript.normal;
                    Vector3 VnaN = new Vector3(Mathf.Abs(speed.x * n.x) * (-Mathf.Sign(speed.x)), Mathf.Abs(speed.y * n.y) * (-Mathf.Sign(speed.y)), Mathf.Abs(speed.z * n.z) * (-Mathf.Sign(speed.z)));
                    Vector3 deltaP = -2 * n * Vector3.Dot(n, speed) * Mass;
                    ChangeImpuls(deltaP);
                    //Debug.Log(wallScript.normal);
                }
            }
            if (collider.gameObject.GetComponent<Molecule>())
            {
                //Molecule firstMolecule = molecules[i].gameObject.GetComponent<Molecule>();
                Molecule anotherMolecule = collider.gameObject.GetComponent<Molecule>();
                if (Vector3.Dot(anotherMolecule.speed-this.speed, anotherMolecule.transform.position-this.transform.position)<=0)
                {
                    //Столкновение молекул
                    Vector3 v1 = this.speed;
                    Vector3 v2 = anotherMolecule.speed;
                    float m1 = this.Mass;
                    float m2 = anotherMolecule.Mass;
                    Vector3 speedOfCenterOfMass = (m1 * v1 + m2 * v2) / (m1 + m2);
                    Vector3 v1Otn = v1 - speedOfCenterOfMass;
                    Vector3 v2Otn = v2 - speedOfCenterOfMass;
                    float u1OtnMagnitude = Mathf.Sqrt((m2 * ((m1 * v1Otn.magnitude * v1Otn.magnitude) + (m2 * v2Otn.magnitude * v2Otn.magnitude))) / (m1 * (m1 + m2)));
                    float u2OtnMagnitude = Mathf.Sqrt((m1 * ((m1 * v1Otn.magnitude * v1Otn.magnitude) + (m2 * v2Otn.magnitude * v2Otn.magnitude))) / (m2 * (m1 + m2)));
                    Vector3 u1Otn = Vector3.ClampMagnitude(v2Otn * 1000, 1) * u1OtnMagnitude;
                    Vector3 u2Otn = Vector3.ClampMagnitude(v1Otn * 1000, 1) * u2OtnMagnitude;
                    Vector3 u1 = u1Otn + speedOfCenterOfMass;
                    Vector3 u2 = u2Otn + speedOfCenterOfMass;
                    this.ChangeImpuls(m1 * (u1 - v1));
                    anotherMolecule.ChangeImpuls(m2 * (u2 - v2));
                }
            }
        }
    }

    public void ChangeImpuls(Vector3 deltaP)
    {
        Vector3 deltaV = (1 / Mass) * deltaP;
        speed = speed + deltaV;
    }

    public void Translate()
    {
        if (float.IsNaN(transform.position.x) || float.IsNaN(transform.position.y) || float.IsNaN(transform.position.z))
            Debug.LogWarning("Nan!!!!");
        transform.position += speed * Time.deltaTime;
        Debug.DrawRay(transform.position, speed, Color.blue, Time.deltaTime);
    }

    //public void UpdateLastCollisionedObjects(Collider[] nearObjects)
    //{
    //    List<int> indexesOfObjectsToRemove = new List<int>();
    //    List<GameObject> nearObjectsList = new List<GameObject>();
    //    foreach(Collider collider in nearObjects)
    //        nearObjectsList.Add(collider.gameObject);
    //    for(int i = lastCollisionedObjects.Count - 1; i >= 0; i--)
    //    {
    //        if (!nearObjectsList.Contains(lastCollisionedObjects[i]))
    //            indexesOfObjectsToRemove.Add(i);
    //    }
    //    foreach (int i in indexesOfObjectsToRemove)
    //        lastCollisionedObjects.RemoveAt(i);
    //}

    public void UseForce(Vector3 force)
    {
        Vector3 dv = (force / Mass) * Time.deltaTime;
        speed = speed + dv;
    }

    public void UpdateTemperatureTexture(float minTemperature, float maxTemperature)
    {
        float deltaTemperaturesBetweemTextures = (maxTemperature - minTemperature) / (temperatureTextures.Count - 1);
        List<float> deltasProcentage = new List<float>();
        for(float t = minTemperature; t <= maxTemperature; t += deltaTemperaturesBetweemTextures)
            deltasProcentage.Add(Mathf.Abs(t - Temperature));
        int minIndex = 0;
        //Debug.Log(Temperature);
        for(int i = 0; i < deltasProcentage.Count; i++)
            if (deltasProcentage[minIndex] > deltasProcentage[i])
                minIndex = i;
        GetComponent<Renderer>().material = temperatureTextures[minIndex];
    }
}
