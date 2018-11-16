using UnityEngine;

public class Example : MonoBehaviour
{
    RectTransform m_RectTransform;
    float m_XAxis, m_YAxis;

    void Start()
    {
        //Fetch the RectTransform from the GameObject
        m_RectTransform = GetComponent<RectTransform>();
        //Initiate the x and y positions
        m_XAxis = 0.5f;
        m_YAxis = 0.5f;


        Debug.Log("anchorPosition:"+m_RectTransform.anchoredPosition);
        Debug.Log("anchoredPosition3D:" + m_RectTransform.anchoredPosition3D);
        Debug.Log("anchorMax:" + m_RectTransform.anchorMax);    //正规化的值（0-1），表示占符RectTransform的百分比
        Debug.Log("anchorMin:" + m_RectTransform.anchorMin);    //正规化的值（0-1），表示占符RectTransform的百分比
        Debug.Log("offsetMax:" + m_RectTransform.offsetMax);    //当前矩形右上角相对于锚点右上角的偏移（以锚点为坐标系原点）
        Debug.Log("offsetMin:" + m_RectTransform.offsetMin);    //当前矩形左上角相对于锚点左上角的偏移（以锚点为坐标系原点）
        Debug.Log("pivot:" + m_RectTransform.pivot);            //正规化的值(0-1),表示中心位于物体自身长宽的百分比
        Debug.Log("rect:" + m_RectTransform.rect);
        Debug.Log("sizeDelta:" + m_RectTransform.sizeDelta);       //sizeDelta = offsetMax-offsetMin

        Vector3[] v4 = new Vector3[4];
        m_RectTransform.GetLocalCorners(v4);            //得到自身的四个角相对自身中心点（物体坐标系）的位置（左下，左上，右上，右下角）
        for (int i =0; i < v4.Length; i++)
        {
            Debug.Log(".GetLocalCorners(v4):" + v4[i]);
        }

        m_RectTransform.GetWorldCorners(v4);            //得到自身的四个角相对世界坐标系的位置（左下，左上，右上，右下角）
        for (int i = 0; i < v4.Length; i++)
        {
            Debug.Log(".GetWorldCorners(v4):" + v4[i]);
        }
    }

    private void Update()
    {

        Vector3[] v4 = new Vector3[4];
        m_RectTransform.GetLocalCorners(v4);
        for (int i = 0; i < v4.Length; i++)
        {
            Debug.Log(".GetLocalCorners(v4):" + v4[i]);
        }

        m_RectTransform.GetWorldCorners(v4);
        for (int i = 0; i < v4.Length; i++)
        {
            Debug.Log(".GetWorldCorners(v4):" + v4[i]);
        }
    }
    void OnGUI()
    {
        ////The Labels show what the Sliders represent
        //GUI.Label(new Rect(0, 20, 150, 80), "Anchor Position X : ");
        //GUI.Label(new Rect(300, 20, 150, 80), "Anchor Position Y : ");

        ////Create a horizontal Slider that controls the x and y Positions of the anchors
        //m_XAxis = GUI.HorizontalSlider(new Rect(150, 20, 100, 80), m_XAxis, -50.0f, 50.0f);
        //m_YAxis = GUI.HorizontalSlider(new Rect(450, 20, 100, 80), m_YAxis, -50.0f, 50.0f);

        ////Detect a change in the GUI Slider
        //if (GUI.changed)
        //{
        //    //Change the RectTransform's anchored positions depending on the Slider values
        //    m_RectTransform.anchoredPosition = new Vector2(m_XAxis, m_YAxis);
        //}
    }
}