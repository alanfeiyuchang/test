using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnEnviiorment : MonoBehaviour
{
    // Start is called before the first frame update
    public static SpawnEnviiorment instanace;
    private void Awake()
    {
        instanace = this;
    }
    [SerializeField] public GameObject waterParent;
    [SerializeField] private GameObject sand;
    [SerializeField] private GameObject grassTop;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void spawnTile(GameObject _tile)
    {
        //Debug.Log(_tile.transform.position);
        //spawnGround(_tile.transform);
        foreach(Transform piece in _tile.transform)
        {
            if(piece.name == "Green_Tile(Clone)")
            {
                spawnGrass(piece, _tile.transform);
            }
            else
            {
                spawnWater(piece);
            }
        }
    }
    public void spawnWater(Vector3 _pos)
    {
        GameObject _empty = new GameObject("WaterMark");
        _empty.transform.SetParent(waterParent.transform);
        _empty.transform.position = new Vector3(_pos.x, _pos.y + 0.1f, _pos.z);
        _empty.transform.localScale = new Vector3(1, 1, 1);
    }
    public void spawnWater(Transform _trans)
    {
        GameObject _water = Instantiate(waterParent, _trans.position, _trans.rotation, transform);
        GameObject _empty = new GameObject("WaterMark");
        _empty.transform.SetParent(_water.transform);
        _empty.transform.localPosition = new Vector3(0, 0, 0);
        _empty.transform.localScale = new Vector3(1, 1, 1);
        GameObject _empty2 = new GameObject("WaterMark");
        _empty2.transform.SetParent(_water.transform);
        _empty2.transform.localPosition = new Vector3(0, 0, 0);
        _empty2.transform.localScale = new Vector3(1, 1, 1);
        _water.transform.localScale = _trans.localScale;
    }
    public void spawnGround(Transform _trans)
    {
        GameObject _sand = Instantiate(sand);
        _sand.transform.SetParent(transform);
        _sand.transform.position = _trans.position;
    }
    public void spawnSand(Vector3 _pos)
    {
        GameObject _sand = Instantiate(sand);
        _sand.transform.SetParent(transform);
        _sand.transform.position = _pos;
    }
    public void spawnGrass(Vector3 _pos)
    {
        GameObject _grass = Instantiate(grassTop);
        _grass.transform.SetParent(transform);
        _grass.transform.position = new Vector3(_pos.x,_pos.y+0.5f,_pos.z);
    }
    public void spawnGrass(Transform _trans, Transform parent)
    {
        GameObject _grass = Instantiate(grassTop);
        _grass.transform.SetParent(transform);
        _grass.transform.localScale = _trans.localScale;
        _grass.transform.position = _trans.position;
    }
}
