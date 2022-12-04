using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maksvel : MonoBehaviour
{
    [SerializeField]
    public MoleculeManager moleculeManager;
    public float min = 100;
    public float max = 1000;
    public float step = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Dictionary<float, int> maksvel = new Dictionary<float, int>();
            float speedBorder = min;
            while (speedBorder < max)
            {
                maksvel.Add(speedBorder, 0);
                speedBorder += step;
            }
            foreach(Molecule script in moleculeManager.scripts)
            {
                //for(int i = 0; i < (max - min) / step; i++)
                //{
                //    if(script.speed.magnitude- min - i * step < step)
                //    {
                //        float minSpeedOfStep = min + i * step;
                //        maksvel[minSpeedOfStep]++;
                //    }
                //}
                speedBorder = min;
                while (speedBorder < max)
                {
                    if (script.speed.magnitude - speedBorder < step && script.speed.magnitude - speedBorder >=0)
                    {
                        maksvel[speedBorder]++;
                        break;
                    }
                    speedBorder += step;
                }
            }
            string speeds = "";
            string nOfMolecules = "";
            foreach(float speed in maksvel.Keys)
            {
                speeds += (speed + "; ").ToString();
                nOfMolecules += (maksvel[speed] + "; ").ToString();
            }
            Debug.Log(speeds);
            Debug.Log(nOfMolecules);
        }
    }
}
