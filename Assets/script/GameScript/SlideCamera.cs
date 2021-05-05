using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SlideCamera : MonoBehaviour
{
    float duration=4f;
    float time=0f;
    //float min_height=5f;
    bool move=false;
    Vector3 move_to=Vector3.zero;

    public void camera_follow(Vector3 end) {
        move=true;
        move_to=end;
        move_to.y=transform.position.y;//height is not changed

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(move)
        {
            transform.position=Vector3.Slerp(transform.position, move_to, time);
            time+=Time.deltaTime/duration;
            if(transform.position==move_to)
            {
                move=false;
                time=0f;
            }
        }
    }
}
