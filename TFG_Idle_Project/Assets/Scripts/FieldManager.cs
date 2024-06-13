using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.UI;

public class FieldManager : MonoBehaviour
{
    [SerializeField]
    private Sprite[] fieldSprites;

    private CreatePlant createPlant;
    OnHoverAnim hoverAnim;

    [SerializeField]
    private Sprite[] sunflowerSprites;

    bool canCollect = false;

    private static readonly string ClickableTag = "Clickable";
    private static readonly string PlantationClickableTag = "Plantation_Clickable";

    private Dictionary<string, (Sprite sprite, string newTag)> fieldMappings;


    private void Start()
    {
        createPlant = GameObject.Find("ScriptHolder").GetComponent<CreatePlant>();
        hoverAnim = GameObject.Find("ScriptHolder").GetComponent<OnHoverAnim>();

        fieldMappings = new Dictionary<string, (Sprite, string)>
        {
            { "Field", (fieldSprites[0], "Plantation") },
            { "SunflowerField", (fieldSprites[1], "Sunflower") }
        };
    }
    private void Update()
    {
        StartGrowth();
    }

    public void SetField()
    {
        if (!canCollect)
        {
            string fieldName = createPlant.fieldName;
            if (gameObject.CompareTag(ClickableTag) || gameObject.CompareTag(PlantationClickableTag))
            {
                if (fieldMappings.TryGetValue(fieldName, out var fieldInfo))
                {
                    var image = gameObject.GetComponent<Image>();
                    image.sprite = fieldInfo.sprite;
                    gameObject.tag = fieldInfo.newTag;
                    hoverAnim.IncrementValue(gameObject.tag);
                    StartCoroutine(StartGrowth());
                }
            }
        }
        else
        {
            gameObject.GetComponent<Image>().sprite = fieldSprites[0];
            gameObject.tag = "Plantation";
            canCollect = false;
        }
    }

    IEnumerator StartGrowth()
    {
        float plantGrowth;
        float plantFinish;
        var image = gameObject.GetComponent<Image>();  
        switch (gameObject.tag)
        {
            case "Sunflower":
                plantGrowth = 10f;
                plantFinish = 5f;
                yield return new WaitForSeconds(plantGrowth);
                image.sprite = sunflowerSprites[0];
                yield return new WaitForSeconds(plantFinish);
                image.sprite = sunflowerSprites[1];
                canCollect = true;
                break;
            default:
                yield return null;
                break;
        }
    }

   
}