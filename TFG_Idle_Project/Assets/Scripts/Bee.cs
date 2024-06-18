using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : MonoBehaviour
{
    public float speed;

    public float moveCounter;

    public float moveTime;

    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        moveCounter -= Time.deltaTime;

        if (moveCounter < 0)
        {
            moveCounter = moveTime;
            float newRotation = Random.Range(0f, 360f);
            transform.rotation = Quaternion.Euler(0f,0f, newRotation);
        }

        transform.position += transform.up * Time.deltaTime * speed;

        if (Input.GetMouseButtonDown(0)) // 0 is the left mouse button
        {
            DetectClick();
        }
    }

    void DetectClick()
    {

        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);


        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPosition, Vector2.zero);

        if (hit.collider != null)
        {
            hit.collider.gameObject.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
        }
    }

    void OnClick()
    {
        OnHoverAnim.GlobalGrowthFactors.Money += 10;
        Destroy(gameObject);
    }
}
