using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class OnHoverAnim : MonoBehaviour
{
    public Tile[] plantTile;
    private int spriteIndex = 0;
    private bool canPlace = false;
    [SerializeField] private Tilemap InteractiveMap;
    [SerializeField] private Tile MarkedTile;
    [SerializeField] private Tile RemovedTile;
    [SerializeField] private GameObject fieldPrefab;
    private List<GameObject> fieldList;
    public GameObject[] UIPlant;
    public Sprite[] extraSprites;

    public static class GlobalGrowthFactors
    {
        public static double FieldGrowthFactor = 1.0f;
        public static double SunflowerGrowthFactor = 1.0f;
        public const double GrowthRate = 1.5f; 
    }

    // Start is called before the first frame update
    void Start()
    {
        fieldList = new List<GameObject>();
        RecorrerTilemap();
    }

    // Update is called once per frame
    void Update()
    {
 
    }


    public void SetPutTileTexture(int SpriteID)
    {
        spriteIndex = SpriteID;
    }

    public void RecorrerTilemap()
    {
        // Obtenemos los límites del tilemap
        BoundsInt bounds = InteractiveMap.cellBounds;

        // Recorremos cada posición dentro de los límites del tilemap
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                for (int z = bounds.zMin; z < bounds.zMax; z++)
                {
                    Vector3Int pos = new Vector3Int(x, y, z);

                    // Aquí puedes hacer lo que necesites con cada posición del tilemap
                    TileBase tile = InteractiveMap.GetTile(pos);
                    if (tile != null)
                    {
                        pos *= 105;
                        pos.x += 50;
                        pos.y += 55;
                        GameObject fieldButton = Instantiate(fieldPrefab,(pos),Quaternion.identity) as GameObject;
                        fieldButton.transform.SetParent(GameObject.Find("Canvas").transform, false);
                        fieldList.Add(fieldButton);
                    }
                }
            }
        }
    }

    public void ChangeTexture(bool isPlantationMode = false)
    {
        Dictionary<string, (Sprite, string)> defaultTagSpriteMap = new Dictionary<string, (Sprite, string)>
        {
            { "Non_Clickable", (extraSprites[0], "Clickable") },
            { "Clickable", (extraSprites[1], "Non_Clickable") },
            { "Plantation_Clickable", (extraSprites[3], "Plantation") }
        };

        Dictionary<string, (Sprite, string)> plantationTagSpriteMap = new Dictionary<string, (Sprite, string)>
        {
            { "Plantation", (extraSprites[2], "Plantation_Clickable") },
            { "Clickable", (extraSprites[1], "Non_Clickable") },
            { "Plantation_Clickable", (extraSprites[3], "Plantation") }
        };

        var tagSpriteMap = isPlantationMode ? plantationTagSpriteMap : defaultTagSpriteMap;

        for (int i = 0; i < fieldList.Count; i++)
        {
            string currentTag = fieldList[i].tag;
            if (tagSpriteMap.ContainsKey(currentTag))
            {
                var (sprite, newTag) = tagSpriteMap[currentTag];
                fieldList[i].GetComponent<Image>().sprite = sprite;
                fieldList[i].tag = newTag;
            }
        }
    }

    public void IncrementValue(string fieldName)
    {
        switch (fieldName)
        {
            case "Plantation":
                GlobalGrowthFactors.FieldGrowthFactor *= GlobalGrowthFactors.GrowthRate;
                GlobalGrowthFactors.FieldGrowthFactor = Mathf.Round((float)GlobalGrowthFactors.FieldGrowthFactor);
                Debug.Log($"Field Growth Factor: {GlobalGrowthFactors.FieldGrowthFactor}");
                UIPlant[0].GetComponentInChildren<Text>().text = GlobalGrowthFactors.FieldGrowthFactor.ToString();
                break;
            case "Sunflower":
                GlobalGrowthFactors.SunflowerGrowthFactor *= GlobalGrowthFactors.GrowthRate;
                GlobalGrowthFactors.SunflowerGrowthFactor = Mathf.Round((float)GlobalGrowthFactors.SunflowerGrowthFactor);
                Debug.Log($"Field Growth Factor: {GlobalGrowthFactors.SunflowerGrowthFactor}");
                UIPlant[1].GetComponentInChildren<Text>().text = GlobalGrowthFactors.SunflowerGrowthFactor.ToString();
                break;
            default:
                Debug.Log("Cagada Historica");
                break;
        }
    }
}


