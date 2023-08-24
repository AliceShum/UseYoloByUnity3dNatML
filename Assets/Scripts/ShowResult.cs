using UnityEngine;

public class ShowResult : MonoBehaviour
{
    public Transform dotParent;
    public GameObject dot;
    public GameObject box;

    private void Start()
    {
        dot.SetActive(false);
    }

    public void Show(Yolov8DataStructure data)
    {
        CreateBox(data.boxPos, data.boxSize);
        for (int i = 0; i < data.pointPosArr.Length; i++)
        {
            CreateRedDot(i, data.pointPosArr[i]);
        }
    }


    //create red dots on specific position
    void CreateRedDot(int boxIndex, Vector2 pos)
    {
        GameObject newDot = Instantiate(dot).gameObject;
        newDot.transform.SetParent(dotParent);
        newDot.GetComponent<RectTransform>().anchoredPosition = new Vector2(pos.x, -pos.y);
        newDot.name = boxIndex.ToString();
        newDot.SetActive(true);
    }

    void CreateBox(Vector2 pos, Vector2 size)
    {
        box.GetComponent<RectTransform>().anchoredPosition = new Vector2(pos.x, -pos.y);
        box.GetComponent<RectTransform>().sizeDelta = size;
        box.SetActive(true);
    }
}
