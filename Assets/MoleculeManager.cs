using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleculeManager : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> molecules = new List<GameObject>();
    public List<Molecule> scripts { get; private set; } = new List<Molecule>();
    public int nOfMolecules;
    public float maxSpeed;
    [SerializeField]
    bool createMolecules = true;
    public bool HeavyAndLight = false;
    public bool WithTemperatureCubs = false;
    [SerializeField]
    public GameObject[] moleculePrefabs;
    public float spawnZone;
    public bool UseGravity = false;
    public Vector3 g = new Vector3(0, -9.8f, 0);

    public float coldSpeed;
    public float hotSpeed;
    private float[] temperatureOfTemperatureCubs = new float[10];
    private GameObject[] temperatureCubs = new GameObject[10];
    private List<GameObject>[] listOfMoleculesInTemperaturesCubs = new List<GameObject>[10];
    [SerializeField]
    public GameObject temperatureCubePrefab;

    public float intermolecularInteraction = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (createMolecules)
        {
            if (HeavyAndLight)
                CreateRandomSpeedMolecules(nOfMolecules, maxSpeed, moleculePrefabs);
            else if (WithTemperatureCubs)
            {
                for (int i = 0; i < 10; i++)
                {
                    GameObject temperatureCub = Instantiate(temperatureCubePrefab) as GameObject;
                    temperatureCubs[i] = temperatureCub;
                    temperatureCub.transform.position = new Vector3(55, -22.5f + 5 * i, 0);
                    listOfMoleculesInTemperaturesCubs[i] = new List<GameObject>();
                }
                CreateMoleculesWithDifferentTemperatiresOnTheLeftAndOnTheRight(nOfMolecules, coldSpeed, hotSpeed, moleculePrefabs);
            }
            else
                CreateRandomSpeedMolecules(nOfMolecules, maxSpeed, new GameObject[] { moleculePrefabs[0] });
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Gravity
        if (UseGravity)
        {
            int heavyButtom = 0;
            int heavyUp = 0;
            int TotalButtom = 0;
            int TotalUp = 0;
            for (int i = 0; i < molecules.Count; i++)
            {
                Molecule script = molecules[i].gameObject.GetComponent<Molecule>();
                Vector3 gravitationForce = script.Mass * g;
               // Debug.Log("Force");
                script.UseForce(gravitationForce);
                //Count
                if (script.Mass == 10)
                {
                    if (molecules[i].gameObject.transform.position.y < 0)
                        heavyButtom++;
                    else
                        heavyUp++;
                }
                if (molecules[i].gameObject.transform.position.y < 0)
                    TotalButtom++;
                else
                    TotalUp++;
            }
            //Debug.Log(heavyUp);
            //Debug.Log(TotalUp);
            float heavyButtomProcent = (heavyButtom * 100 / TotalButtom);
            float heavyUpProcent = (heavyUp * 100 / TotalUp);
            //Debug.Log("Heavy Up = " + heavyUpProcent + "%    Heavy Buttom = " + heavyButtomProcent + "%");
        }
        IntermolecularInteractions();
        //if(false)
            for (int i = 0; i < molecules.Count; i++)
                 molecules[i].GetComponent<Molecule>().UpdateTemperatureTexture(0, hotSpeed* hotSpeed);
        if (WithTemperatureCubs)
            UpdateTemperatureCubs();
    }

    public void CreateRandomSpeedMolecules(int nOfMolecules, float maxSpeed, GameObject[]prefabs)
    {
        for(int i = 0; i < nOfMolecules; i++)
        {
            int prefab = Random.Range(0, prefabs.Length);
            GameObject molecule = Instantiate(prefabs[prefab]) as GameObject;
            molecules.Add(molecule);
            Molecule moleculeScript = molecule.gameObject.GetComponent<Molecule>();
            scripts.Add(moleculeScript);
            Vector3 position = new Vector3(Random.Range(-spawnZone, spawnZone), Random.Range(-spawnZone, spawnZone), Random.Range(-spawnZone, spawnZone));
            molecule.transform.position = position;
            float maxProection = maxSpeed / Mathf.Sqrt(3);
            if (prefab == 1)
            {
                maxProection = maxProection / Mathf.Sqrt(10);
            }
            Vector3 speed = new Vector3(Random.Range(-maxProection, maxProection), Random.Range(-maxProection, maxProection), Random.Range(-maxProection, maxProection));
            moleculeScript.ChangeImpuls(moleculeScript.Mass * speed);
        }
    }
    public void CreateMoleculesWithDifferentTemperatiresOnTheLeftAndOnTheRight(int nOfMolecules, float coldSpeed, float hotSpeed, GameObject[] prefabs)
    {
        for (int i = 0; i < nOfMolecules; i++)
        {
            int prefab = Random.Range(0, prefabs.Length);
            GameObject molecule = Instantiate(prefabs[prefab]) as GameObject;
            molecules.Add(molecule);
            Molecule moleculeScript = molecule.gameObject.GetComponent<Molecule>();
            scripts.Add(moleculeScript);
            Vector3 position;
            if (i%2==0)
                position = new Vector3(Random.Range(0, spawnZone), Random.Range(-spawnZone, spawnZone), Random.Range(-spawnZone, spawnZone));
            else
                position = new Vector3(Random.Range(-spawnZone, 0), Random.Range(-spawnZone, spawnZone), Random.Range(-spawnZone, spawnZone));
            molecule.transform.position = position;
            float speedProjection = 0;
            if (i % 2 == 0)
                speedProjection = hotSpeed / Mathf.Sqrt(3);
            else
                speedProjection = coldSpeed / Mathf.Sqrt(3);
            Vector3 speed = new Vector3(Random.Range(-speedProjection, speedProjection), Random.Range(-speedProjection, speedProjection), Random.Range(-speedProjection, speedProjection));
            speed = speed * 100;
            speed = Vector3.ClampMagnitude(speed, speedProjection * Mathf.Sqrt(3));
            moleculeScript.ChangeImpuls(moleculeScript.Mass * speed);
        }
    }
    public void UpdateTemperatureCubs()
    {
        for (int i = 0; i < 10; i++)
        {
            temperatureOfTemperatureCubs[i] = 0;
            listOfMoleculesInTemperaturesCubs[i].Clear();
        }
        foreach (GameObject molecule in molecules)
        {
            //int xIndex = (int)Mathf.Floor((molecule.transform.position.x + 25f) / 5);
            int yIndex = (int)Mathf.Floor((molecule.transform.position.y + 25f) / 5);
            if(0<= yIndex & yIndex<10)
                listOfMoleculesInTemperaturesCubs[yIndex].Add(molecule);
        }
        for (int i = 0; i < 10; i++)
        {
            float sumOfVelositiesInSquare = 0;
            foreach (GameObject molecule in listOfMoleculesInTemperaturesCubs[i])
            {
                Molecule script = molecule.GetComponent<Molecule>();
                sumOfVelositiesInSquare += (script.speed.magnitude) * (script.speed.magnitude);
            }
            temperatureOfTemperatureCubs[i] = (sumOfVelositiesInSquare / listOfMoleculesInTemperaturesCubs[i].Count);
            //Debug.Log(temperatureOfTemperatureCubs[i]);
            float deltaTemperaturesBetweemTextures = (hotSpeed* hotSpeed - coldSpeed* coldSpeed) / 9;
            List<float> deltasProcentage = new List<float>();
            for (float t = 0; t <= hotSpeed* hotSpeed; t += deltaTemperaturesBetweemTextures)
                deltasProcentage.Add(Mathf.Abs(t - temperatureOfTemperatureCubs[i]));
            int minIndex = 0;
            for (int k = 0; k < deltasProcentage.Count; k++)
                if (deltasProcentage[minIndex] > deltasProcentage[k])
                    minIndex = k;
            temperatureCubs[i].GetComponent<TemperatureCube>().UpdateTemperatureTexture(minIndex);
        }
    }
    public void IntermolecularInteractions()
    {
        foreach (Molecule molecule in scripts)
        {
            foreach(Molecule molecule2 in scripts)
            {
                if (molecule == molecule2)
                    continue;
                Vector3 from1to2 = molecule2.gameObject.transform.position - molecule.gameObject.transform.position;
                Vector3 e1to2 = from1to2.normalized;
                Vector3 forceAt1 = e1to2 * intermolecularInteraction / from1to2.magnitude;
                if (molecule.nearRadius > from1to2.magnitude)
                    forceAt1 = Vector3.zero;
                Vector3 forceAt2 = -forceAt1;
                molecule.UseForce(forceAt1);
                molecule2.UseForce(forceAt2);
            }
        }
    }
}
