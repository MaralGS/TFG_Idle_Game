using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class OnHoverAnim : MonoBehaviour
{
    private Grid grid;
    [SerializeField] private Tilemap interactiveMap = null;
    [SerializeField] private Tilemap pathMap = null;
    [SerializeField] private Tile hoverTile = null;
    [SerializeField] private Tile putTile = null;
    //SerializeField] private RuleTile pathTile = null;
    public Tile[] plantTile;
    private int spriteIndex = 0;


    private Vector3Int previousMousePos = new Vector3Int();

    // Start is called before the first frame update
    void Start()
    {
        grid = gameObject.GetComponent<Grid>();
    }

    // Update is called once per frame
    void Update()
    {
        // Mouse over -> highlight tile
        Vector3Int mousePos = GetMousePos();
        if (!mousePos.Equals(previousMousePos))
        {
            
            interactiveMap.SetTile(previousMousePos, null); // Remove old hoverTile
            interactiveMap.SetTile(mousePos, hoverTile);
            previousMousePos = mousePos;
        }

        // Left mouse click -> add path tile
        if (Input.GetMouseButton(0))
        {
            pathMap.SetTile(mousePos, plantTile[spriteIndex]);
        }

        // Right mouse click -> remove path tile
        if (Input.GetMouseButton(1))
        {
            pathMap.SetTile(mousePos, null);
        }
    }

    Vector3Int GetMousePos()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return grid.WorldToCell(mouseWorldPos);
    }

    public void SetPutTileTexture(int SpriteID)
    {
        spriteIndex = SpriteID;
    }
}

