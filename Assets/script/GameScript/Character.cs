using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockNS;
using typeNS;

namespace CharacterNS
{
    public class Character : MonoBehaviour
    {    
        //public GameObject me;//default player
        private GameObject me2;
        float step_size;
        private float speed = 3;
        private Vector3 end;
        private void Awake() {
            
        }

        public void init_character(charType ctype,float x,float y,float z)
        {
            step_size=1;
            Vector3 init_pos=new Vector3(x,y,z);
            GameObject me;
            string char_path="";
            switch(ctype)
            {
                case charType.CHAR1:
                {
                    char_path="Prefab/Character/character1";
                    break;
                }
                case charType.CHAR2:
                {
                    char_path="Prefab/Character/character2";
                    break;
                }
                case charType.CHAR3:
                {
                    char_path="Prefab/Character/character3";
                    break;
                }
                case charType.CHAR4:
                {
                    char_path="Prefab/Character/character4";
                    break;
                }
            }
            me=Resources.Load(char_path, typeof(GameObject)) as GameObject;       
            me.transform.position=new Vector3(0f,1f,0f);
            Quaternion rot = Quaternion.Euler(0, 0, 0);
            end = init_pos;
            me2 =Instantiate(me,init_pos,rot);
            me2.name="character"+((int)ctype+1).ToString();
        }

        public void move_character(int x, int z)
        {
            if (x == 0) { end = new Vector3(-2f, 0f, 2 * z);return; };
            if (x == 14) { end = new Vector3(30f, 0f, 2f * z); return; }
            if (z == 0) { end = new Vector3(2 * x, 0f, -2f); return;}
            if (z == 9) { end = new Vector3(2 * x, 0f, 20f); return; }

        }
        // Start is called before the first frame update
        void Start()
        {

        }

            // Update is called once per frame
            void Update()
            {
                float step = speed * Time.deltaTime;

                if (me2.transform.position != end) {
                
                    if (me2.transform.position.x == end.x && end.x == -2f && me2.transform.position.z!=20f){
                    if (me2.transform.position.z < end.z) {
                        Quaternion target = Quaternion.Euler(0, 360, 0);
                        if (me2.transform.position.z == -2f) me2.transform.rotation = Quaternion.RotateTowards(me2.transform.rotation, target, 100f);
                        me2.transform.position = Vector3.MoveTowards(me2.transform.position, end, step); }
                    else
                    {
                        Quaternion target = Quaternion.Euler(0, 360, 0);
                        if (me2.transform.position.z == -2f) me2.transform.rotation = Quaternion.RotateTowards(me2.transform.rotation, target, 100f);
                        me2.transform.position = Vector3.MoveTowards(me2.transform.position, new Vector3(-2f, 0, 20f), step);
                    }
                    }
                    if (me2.transform.position.x == end.x && end.x == 30f && me2.transform.position.z != -2f){
                    if (me2.transform.position.z > end.z) {
                        Quaternion target = Quaternion.Euler(0, 180, 0);
                        if (me2.transform.position.z == 20f) me2.transform.rotation = Quaternion.RotateTowards(me2.transform.rotation, target, 100f);
                        me2.transform.position = Vector3.MoveTowards(me2.transform.position, end, step); }
                    else
                    {
                        Quaternion target = Quaternion.Euler(0, 180, 0);
                        if (me2.transform.position.z == 20f) me2.transform.rotation = Quaternion.RotateTowards(me2.transform.rotation, target, 100f);
                        me2.transform.position = Vector3.MoveTowards(me2.transform.position, new Vector3(30f, 0, -2f), step);
                    }
                    }
                
                    if (me2.transform.position.z == end.z && end.z == 20f && me2.transform.position.x != 30f){
                    if (me2.transform.position.x < end.x) {
                        Quaternion target = Quaternion.Euler(0, 90, 0);
                        if (me2.transform.position.x == -2f) me2.transform.rotation = Quaternion.RotateTowards(me2.transform.rotation, target, 100f);
                        me2.transform.position = Vector3.MoveTowards(me2.transform.position, end, step); }
                    else
                    {
                        Quaternion target = Quaternion.Euler(0, 90, 0);
                        if (me2.transform.position.x == -2f) me2.transform.rotation = Quaternion.RotateTowards(me2.transform.rotation, target, 100f);
                        me2.transform.position = Vector3.MoveTowards(me2.transform.position, new Vector3(30f, 0, 20f), step);
                    }
                    }
                    if (me2.transform.position.z == end.z && end.z == -2f && me2.transform.position.x != -2f){
                    if (me2.transform.position.x > end.x) {
                        Quaternion target = Quaternion.Euler(0, 270, 0);
                        if (me2.transform.position.x == 30f) me2.transform.rotation = Quaternion.RotateTowards(me2.transform.rotation, target, 100f);
                        me2.transform.position = Vector3.MoveTowards(me2.transform.position, end, step); }
                    else
                    {
                        Quaternion target = Quaternion.Euler(0, 270, 0);
                        if (me2.transform.position.z == 30f) me2.transform.rotation = Quaternion.RotateTowards(me2.transform.rotation, target, 100f);
                        me2.transform.position = Vector3.MoveTowards(me2.transform.position, new Vector3(-2f, 0, 20f), step);
                    }
                    }
                    
                if (me2.transform.position.x == -2f && me2.transform.position.z != 20f && me2.transform.position.x != end.x) {
                    Quaternion target = Quaternion.Euler(0, 360, 0);
                    if (me2.transform.position.z == -2f) me2.transform.rotation = Quaternion.RotateTowards(me2.transform.rotation, target, 100f);
                    me2.transform.position = Vector3.MoveTowards(me2.transform.position, new Vector3(-2f, 0, 20f), step);

                }
                if (me2.transform.position.x == 30f && me2.transform.position.z != -2f && me2.transform.position.x != end.x) {
                    Quaternion target = Quaternion.Euler(0, 180, 0);
                    if (me2.transform.position.z == 20f) me2.transform.rotation = Quaternion.RotateTowards(me2.transform.rotation, target, 100f);
                    me2.transform.position = Vector3.MoveTowards(me2.transform.position, new Vector3(30f, 0, -2f), step);
                }
                if (me2.transform.position.z == 20f && me2.transform.position.z != 30f && me2.transform.position.z != end.z) {
                    Quaternion target = Quaternion.Euler(0, 90, 0);
                    if (me2.transform.position.x == -2f) me2.transform.rotation = Quaternion.RotateTowards(me2.transform.rotation, target, 100f);
                    me2.transform.position = Vector3.MoveTowards(me2.transform.position, new Vector3(30f, 0, 20f), step);
                }
                if (me2.transform.position.z == -2f && me2.transform.position.z != -2f && me2.transform.position.z != end.z) { 
                    Quaternion target = Quaternion.Euler(0, 270, 0);
                    if (me2.transform.position.x == 30f) me2.transform.rotation = Quaternion.RotateTowards(me2.transform.rotation, target, 100f);
                    me2.transform.position = Vector3.MoveTowards(me2.transform.position, new Vector3(-2f, 0, -2f), step);
                }
            }


            }

    }
}
