using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockNS;
using typeNS;

namespace CharacterNS
{
    public class Character : MonoBehaviour
    {    
        private int player_id=-1;
        private GameObject me;
        private float speed = 4;
        public Vector3 end;

        public void set_character(int pid,GameObject m,int x,int z)
        {
            float rotate_y=0;
            player_id=pid;
            me=m;
            if (x == 0)
            {
                rotate_y=0;
                end = new Vector3(-2f, 0f, 2 * z);
            }
            else if (x == 14) 
            {
                rotate_y=180;
                end = new Vector3(30f, 0f, 2f * z);
            }
            else if (z == 0) 
            {
                rotate_y=270;
                end = new Vector3(2 * x, 0f, -2f); 
            }
            else if (z == 9) 
            {
                rotate_y=90;
                end = new Vector3(2 * x, 0f, 20f);
            }
            me.transform.rotation=Quaternion.Euler(0, rotate_y, 0);
            me.transform.position = end;
        }
        public int get_player_id()
        {
            return player_id;
        }

        public void move_character(int x, int z)
        {
            if (x == 0) 
            { 
                end = new Vector3(-2f, 0f, 2 * z);
                return; 
            }
            if (x == 14) 
            { 
                end = new Vector3(30f, 0f, 2f * z); 
                return; 
            }
            if (z == 0)
            { 
                end = new Vector3(2 * x, 0f, -2f); 
                return;
            }
            if (z == 9) 
            {
                end = new Vector3(2 * x, 0f, 20f); 
                return; 
            }
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            float step = speed * Time.deltaTime;

            if (me.transform.position != end) {
            
                if (me.transform.position.x == end.x && end.x == -2f && me.transform.position.z!=20f){
                if (me.transform.position.z < end.z) {
                    Quaternion target = Quaternion.Euler(0, 360, 0);
                    if (me.transform.position.z == -2f) me.transform.rotation = Quaternion.RotateTowards(me.transform.rotation, target, 100f);
                    me.transform.position = Vector3.MoveTowards(me.transform.position, end, step); }
                else
                {
                    Quaternion target = Quaternion.Euler(0, 360, 0);
                    if (me.transform.position.z == -2f) me.transform.rotation = Quaternion.RotateTowards(me.transform.rotation, target, 100f);
                    me.transform.position = Vector3.MoveTowards(me.transform.position, new Vector3(-2f, 0, 20f), step);
                }
                }
                if (me.transform.position.x == end.x && end.x == 30f && me.transform.position.z != -2f){
                if (me.transform.position.z > end.z) {
                    Quaternion target = Quaternion.Euler(0, 180, 0);
                    if (me.transform.position.z == 20f) me.transform.rotation = Quaternion.RotateTowards(me.transform.rotation, target, 100f);
                    me.transform.position = Vector3.MoveTowards(me.transform.position, end, step); }
                else
                {
                    Quaternion target = Quaternion.Euler(0, 180, 0);
                    if (me.transform.position.z == 20f) me.transform.rotation = Quaternion.RotateTowards(me.transform.rotation, target, 100f);
                    me.transform.position = Vector3.MoveTowards(me.transform.position, new Vector3(30f, 0, -2f), step);
                }
                }
            
                if (me.transform.position.z == end.z && end.z == 20f && me.transform.position.x != 30f){
                if (me.transform.position.x < end.x) {
                    Quaternion target = Quaternion.Euler(0, 90, 0);
                    if (me.transform.position.x == -2f) me.transform.rotation = Quaternion.RotateTowards(me.transform.rotation, target, 100f);
                    me.transform.position = Vector3.MoveTowards(me.transform.position, end, step); }
                else
                {
                    Quaternion target = Quaternion.Euler(0, 90, 0);
                    if (me.transform.position.x == -2f) me.transform.rotation = Quaternion.RotateTowards(me.transform.rotation, target, 100f);
                    me.transform.position = Vector3.MoveTowards(me.transform.position, new Vector3(30f, 0, 20f), step);
                }
                }
                if (me.transform.position.z == end.z && end.z == -2f && me.transform.position.x != -2f){
                if (me.transform.position.x > end.x) {
                    Quaternion target = Quaternion.Euler(0, 270, 0);
                    if (me.transform.position.x == 30f) me.transform.rotation = Quaternion.RotateTowards(me.transform.rotation, target, 100f);
                    me.transform.position = Vector3.MoveTowards(me.transform.position, end, step); }
                else
                {
                    Quaternion target = Quaternion.Euler(0, 270, 0);
                    if (me.transform.position.z == 30f) me.transform.rotation = Quaternion.RotateTowards(me.transform.rotation, target, 100f);
                    me.transform.position = Vector3.MoveTowards(me.transform.position, new Vector3(-2f, 0, 20f), step);
                }
            }
                
            if (me.transform.position.x == -2f && me.transform.position.z != 20f && me.transform.position.x != end.x) {
                Quaternion target = Quaternion.Euler(0, 360, 0);
                if (me.transform.position.z == -2f) me.transform.rotation = Quaternion.RotateTowards(me.transform.rotation, target, 100f);
                me.transform.position = Vector3.MoveTowards(me.transform.position, new Vector3(-2f, 0, 20f), step);
            }
            if (me.transform.position.x == 30f && me.transform.position.z != -2f && me.transform.position.x != end.x) {
                Quaternion target = Quaternion.Euler(0, 180, 0);
                if (me.transform.position.z == 20f) me.transform.rotation = Quaternion.RotateTowards(me.transform.rotation, target, 100f);
                me.transform.position = Vector3.MoveTowards(me.transform.position, new Vector3(30f, 0, -2f), step);
            }
            if (me.transform.position.z == 20f && me.transform.position.z != 30f && me.transform.position.z != end.z) {
                Quaternion target = Quaternion.Euler(0, 90, 0);
                if (me.transform.position.x == -2f) me.transform.rotation = Quaternion.RotateTowards(me.transform.rotation, target, 100f);
                me.transform.position = Vector3.MoveTowards(me.transform.position, new Vector3(30f, 0, 20f), step);
            }
            if (me.transform.position.z == -2f && me.transform.position.z != -2f && me.transform.position.z != end.z) { 
                Quaternion target = Quaternion.Euler(0, 270, 0);
                if (me.transform.position.x == 30f) me.transform.rotation = Quaternion.RotateTowards(me.transform.rotation, target, 100f);
                me.transform.position = Vector3.MoveTowards(me.transform.position, new Vector3(-2f, 0, -2f), step);
            }
        }


        }

    }
}
