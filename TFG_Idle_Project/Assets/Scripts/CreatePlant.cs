using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePlant : MonoBehaviour
{
    [HideInInspector]
    public string fieldName;

    public void SetFieldName(string nameField)
    {
        fieldName = nameField;
    }

}
