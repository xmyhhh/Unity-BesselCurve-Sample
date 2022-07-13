using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BezierCurve : MonoBehaviour
{
    public GameObject curveUI;
    public int curveLen = 8;
    public GameObject dotPrefab;
    public GameObject curveDotPrefab;

    public Vector3 p1 = new Vector3(0, 0, 0);
    public Vector3 p2 = new Vector3(2, 1, 0);
    public Vector3 p3 = new Vector3(4, 1, 0);
    public Vector3 p4 = new Vector3(3, 0, 0);


    float p2Factorx = -0.17f;
    float p2Factory = 0.65f;
    float p2Miny = 1.2f;

    float p3Factorx = 0f;
    float p3Factory = 1.53f;
    float p3Miny = 1.6f;


    List<GameObject> pArray; //保存曲线的控制点

    List<GameObject> curveUIObject;
    void Start()
    {
        curveUIObject = new List<GameObject>();


        pArray = new List<GameObject>();
        InitCurvelUI();
    }

    private void Update()
    {

        p4 = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        p2.x = (p4.x * p2Factorx);
        p2.y = Mathf.Max((float)(p4.y * p2Factory), p2Miny);


        p3.x = (float)(p4.x * p3Factorx);
        p3.y = Mathf.Max((p4.y * p3Factory), p3Miny);

        foreach (var p in pArray)
        {
            Destroy(p);
        }
        pArray.Clear();

        pArray.Add(Instantiate(dotPrefab, p1, Quaternion.identity));
        pArray.Add(Instantiate(dotPrefab, p2, Quaternion.identity));
        pArray.Add(Instantiate(dotPrefab, p3, Quaternion.identity));
        pArray.Add(Instantiate(dotPrefab, p4, Quaternion.identity));

        UpdateCurvelUI();



    }

    void UpdateCurvelUI()
    {
        for (int i = 0; i < curveLen; i++)
        {
            float t = (float)(1.0 / (curveLen - 1)) * i;

            curveUIObject[i].GetComponent<RectTransform>().anchoredPosition = Camera.main.WorldToScreenPoint((CubicBezier(p1, p2, p3, p4, t)));
            if (i >= 1)
            {
                LookAt(curveUIObject[i], curveUIObject[i-1].GetComponent<RectTransform>().anchoredPosition);
            }
        }
    }

    void InitCurvelUI()
    {
        for (int i = 0; i < curveLen; i++)
        {
            var obj = curveUI.transform.Find(i.ToString()).gameObject;
            float scaleFctor = Mathf.Max((1.0f / curveLen) * (i + 1), 0.4f);
            obj.transform.localScale = new Vector3(scaleFctor, scaleFctor, scaleFctor);
            curveUIObject.Add(obj);
        }

    }

    void LookAt(GameObject gameObject, Vector2 taregtPosition)
    {
        var newAxisY = (gameObject.GetComponent<RectTransform>().anchoredPosition - taregtPosition);

        float rot_z = Vector2.Angle(gameObject.transform.up, newAxisY);

        gameObject.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
    }

    Vector3 QuadraticBezier(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        var q0 = LinearInterpolate(p0, p1, t);
        var q1 = LinearInterpolate(p1, p2, t);

        return LinearInterpolate(q0, q1, t);
    }
    Vector3 CubicBezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        var q0 = LinearInterpolate(p0, p1, t);
        var q1 = LinearInterpolate(p1, p2, t);
        var q2 = LinearInterpolate(p2, p3, t);

        var r0 = LinearInterpolate(q0, q1, t);
        var r1 = LinearInterpolate(q1, q2, t);

        var s = LinearInterpolate(r0, r1, t);
        return s;
    }
    Vector3 LinearInterpolate(Vector3 q0, Vector3 q1, float t)
    {
        return (q1 - q0) * t + q0;
    }
}



