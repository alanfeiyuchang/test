using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WooHooFly.Colors;

public class CubeCollider : MonoBehaviour
{
    // Start is called before the first frame update
    public static CubeCollider instance;
    //Will become this color once game starts
    public TileColor Color;
    private TileManager map;
    public bool newPlayer = false;
    [SerializeField] private Material playerColor;
    [SerializeField] private Color waterSideColor;
    [SerializeField] private Color waterTopColor;
    [SerializeField] private Color lavaSideColor;
    [SerializeField] private Color lavaTopColor;
    [SerializeField] private Material water;
    [SerializeField] private Material lava;
    [SerializeField] private GameObject liquad;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        TileManager.instance.changeColor(gameObject, Color);
        updatePlayerColor();
    }

    // Update is called once per frame
    void Update()
    {
 
    }
    private void updatePlayerColor()
    {
        playerColor = liquad.GetComponent<Renderer>().material;
        if (Color == TileColor.grey)
        {
            playerColor.SetFloat("_Fill", 0f);
        }
        else
        {
            StartCoroutine(fillContainer(0.5f));
        }
        /*Renderer _liquad = liquad.GetComponent<Renderer>();
        if (Color == TileColor.green)
        {
            _liquad.material = water;
        }
        else
        {
            _liquad.material = lava;
        }*/
        
    }
    private IEnumerator fillContainer(float time)
    {
        float cont = 0;
        float current_fill, fill;
        if (playerColor.GetFloat("_Fill") > 0)
        {
            cont = 0;
            current_fill = playerColor.GetFloat("_Fill");
            while (cont <= time)
            {
                fill = Mathf.Lerp(current_fill, 0f, cont / time);
                playerColor.SetFloat("_Fill", fill);
                //Debug.Log("Fill" + playerColor.GetFloat("_Fill"));
                cont += Time.deltaTime;
                yield return null;
            }
        }
        if(Color == TileColor.green)
        {
            playerColor.SetColor("_SideColor", waterSideColor);
            playerColor.SetColor("_TopColor", waterTopColor);
        }
        else if(Color == TileColor.red)
        {
            playerColor.SetColor("_SideColor", lavaSideColor);
            playerColor.SetColor("_TopColor", lavaTopColor);
        }
        cont = 0;
        while (cont <= time)
        {
            fill = Mathf.Lerp(0, 0.5f, cont / time);
            playerColor.SetFloat("_Fill", fill);
            cont += Time.deltaTime;
            yield return null;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        
        //change mapCube's color
        if (other.gameObject.tag == "ChangeTile")
        {
            map = other.gameObject.GetComponent<TileManager>();
            map.ColorChange(Color);
   
        }
        //change playerCube's color
        else if(other.gameObject.tag == "ColorTile")
        {
            if (newPlayer)
            {
                map = other.gameObject.GetComponent<TileManager>();
                if(Color != map.MapColor)
                {
                    Color = map.MapColor;
                    updatePlayerColor();
                    //Debug.Log("I am changing to" + Color);
                }      
            }
            else
            {
                map = other.gameObject.GetComponent<TileManager>();
                TileColor c = map.MapColor;
                Debug.Log(c);
                Color = c;
                TileManager.instance.changeColor(gameObject, c);
            }
            
        }

    }




    
}
