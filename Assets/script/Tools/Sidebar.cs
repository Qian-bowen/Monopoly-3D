using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using GlobalsNS;
using CardNS;

/*
WARNING: canvas cannot move in unity
so bind it to image
*/
public class Sidebar : MonoBehaviour, IPointerDownHandler
{
    private bool ishide=true;

    public void OnPointerDown (PointerEventData eventData){
        //if is hide, move outside
        if(ishide)
        {
            transform.position+=new Vector3(105,0,0);
        }
        else{
            transform.position+=new Vector3(-105,0,0);
        }
        ishide=!ishide;
    }

}
