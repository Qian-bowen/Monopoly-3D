using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class updown : MonoBehaviour
{
    public Button up;
    public Button down;
    // Start is called before the first frame update
    void Start()
    {
        Transform camT = Camera.main.transform;
        up.onClick.AddListener(() => AddRotation(camT, 5f));
        down.onClick.AddListener(() => AddRotation(camT, -5f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private static float WrapAngle(float angle)
    {
        if(angle>85)
        {
            angle=45f;
        }
        return angle;
    }

    void AddRotation(Transform t, float angle_add)
    {
        Debug.Log("angle:"+t.eulerAngles.x);
        float tmp=t.eulerAngles.x+angle_add;
        float cur=WrapAngle(tmp);
        Debug.Log("cur:"+cur);



        t.eulerAngles = new Vector3(cur, t.eulerAngles.y, t.eulerAngles.z);
        Debug.Log("after angle:"+t.eulerAngles.x);
    }
}
