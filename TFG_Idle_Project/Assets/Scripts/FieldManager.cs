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

    [SerializeField]
    private Sprite[] riceSprites;

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
            { "SunflowerField", (fieldSprites[1], "Sunflower") },
            { "RiceField", (fieldSprites[2], "Rice") }
        };
    }
    private void Update()
    {
        StartGrowth();
    }

    public void SetField()
    {
        if (canCollect)
        {
            GetIncome();
            ResetField();
            return;
        }

        if (!(gameObject.CompareTag(ClickableTag) || gameObject.CompareTag(PlantationClickableTag)))
            return;

        if (!CanBuy())
            return;

        if (fieldMappings.TryGetValue(createPlant.fieldName, out var fieldInfo))
        {
            var image = gameObject.GetComponent<Image>();
            image.sprite = fieldInfo.sprite;
            gameObject.tag = fieldInfo.newTag;
            hoverAnim.IncrementValue(gameObject.tag);
            StartCoroutine(StartGrowth());
        }
    }

    private void ResetField()
    {
        var image = gameObject.GetComponent<Image>();
        image.sprite = fieldSprites[0];
        gameObject.tag = "Plantation";
        canCollect = false;
    }

    private void GetIncome()
    {
        if (gameObject.tag == "Sunflower")
        {
            OnHoverAnim.GlobalGrowthFactors.Money += 20f;
        }
        else if (gameObject.tag == "Rice")
        {
            OnHoverAnim.GlobalGrowthFactors.Money += 50f;
        }
    }


    IEnumerator StartGrowth()
    {
        var image = gameObject.GetComponent<Image>();
        var plantGrowthTimes = new Dictionary<string, (float growth, float finish, Sprite[] sprites)>
    {
        { "Sunflower", (10f, 5f, sunflowerSprites) },
        { "Rice", (10f, 5f, riceSprites) }
    };

        if (plantGrowthTimes.TryGetValue(gameObject.tag, out var plantInfo))
        {
            yield return new WaitForSeconds(plantInfo.growth);
            image.sprite = plantInfo.sprites[0];
            yield return new WaitForSeconds(plantInfo.finish);
            image.sprite = plantInfo.sprites[1];
            canCollect = true;
        }
    }

    private bool CanBuy()
{
    double requiredCost = 0f;
   
    switch (gameObject.tag)
    {
        case "Clickable":
            requiredCost = OnHoverAnim.GlobalGrowthFactors.TerrainCost;
            break;
        case "Plantation_Clickable":
                if(createPlant.fieldName == "SunflowerField")
                requiredCost = OnHoverAnim.GlobalGrowthFactors.SunFlowerCost;
                else if(createPlant.fieldName == "RiceField")
                requiredCost = OnHoverAnim.GlobalGrowthFactors.RiceFlowerCost;
            break;
        default:
            return false;
    }

    if (OnHoverAnim.GlobalGrowthFactors.Money >= requiredCost)
    {
        OnHoverAnim.GlobalGrowthFactors.Money -= requiredCost;
        return true;
    }
    else
    {
        return false;
    }
}
}