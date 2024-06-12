using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldManager : MonoBehaviour
{
    [SerializeField]
    private Sprite[] fieldSprites;

    private CreatePlant createPlant;

    private static readonly string ClickableTag = "Clickable";
    private static readonly string PlantationClickableTag = "Plantation_Clickable";

    private Dictionary<string, (Sprite sprite, string newTag)> fieldMappings;

    private void Start()
    {
        createPlant = GameObject.Find("ScriptHolder").GetComponent<CreatePlant>();

        fieldMappings = new Dictionary<string, (Sprite, string)>
        {
            { "Field", (fieldSprites[0], "Plantation") },
            { "SunflowerField", (fieldSprites[1], "Sunflower") }
        };
    }

    public void SetField()
    {
        string fieldName = createPlant.fieldName;
        if (gameObject.CompareTag(ClickableTag) || gameObject.CompareTag(PlantationClickableTag))
        {
            if (fieldMappings.TryGetValue(fieldName, out var fieldInfo))
            {
                var image = gameObject.GetComponent<Image>();
                image.sprite = fieldInfo.sprite;
                gameObject.tag = fieldInfo.newTag;
            }
        }
    }
}