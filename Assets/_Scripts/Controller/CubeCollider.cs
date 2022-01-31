using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCollider : MonoBehaviour
{
    // Start is called before the first frame update
    public enum color{
        green,
        red
    };
    public color SideColor;
    private MapColorChange map;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
 
    }
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.tag);
        if(other.gameObject.tag == "MapCube")
        {
            map = other.gameObject.GetComponent<MapColorChange>();
            map.ColorChange(SideColor);
        }
    }
}