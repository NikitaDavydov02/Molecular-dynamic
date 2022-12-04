using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureCube : MonoBehaviour
{
    [SerializeField]
    List<Material> temperatureTextures;
    private float temperature;
    public 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateTemperatureTexture(int index)
    {
        GetComponent<Renderer>().material = temperatureTextures[index];
    }
}
