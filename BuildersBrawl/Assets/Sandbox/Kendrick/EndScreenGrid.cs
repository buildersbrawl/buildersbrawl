using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    public int rows = 5;
    public int columns = 3;
    public GameObject inputFieldPrefab;

    // Start is called before the first frame update
    void Start()
    {
        /*RectTransform parentRect = gameObject.GetComponent<RectTransform>();
        GridLayoutGroup gridLayout = gameObject.GetComponent<GridLayoutGroup>();
        gridLayout.cellSize = new Vector2(parentRect.rect.width / columns, parentRect.rect.height / rows);
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                GameObject inputField = Instantiate(inputFieldPrefab);
                inputField.transform.SetParent(gameObject.transform, false);
            }
        }*/
        Populate();
    }

    void Populate()
    {
        GameObject newObj;

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                newObj = (GameObject)Instantiate(inputFieldPrefab, transform);
                newObj.GetComponent<Image>().color = Random.ColorHSV();
            }
        }
    }
}
