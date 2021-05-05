using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TagNS;
using typeNS;

namespace BlockNS
{
    public class Block : MonoBehaviour
    {
        private GameObject block;
        
        // Start is called before the first frame update
        void Awake() {

        }
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }


        public void instantiate_block(BLOCK_TYPE b,int i,int j)
        {
            GameObject blk;
            switch(b)
            {
                case BLOCK_TYPE.ORDINARY:
                {
                    blk=Resources.Load("Prefab/House/ordinary", typeof(GameObject)) as GameObject;
                    break;
                }
                case BLOCK_TYPE.RESTAURANT:
                {
                    blk=Resources.Load("Prefab/House/building-restaurant", typeof(GameObject)) as GameObject;
                    break;
                }
                case BLOCK_TYPE.NONE:
                {
                    blk=Resources.Load("Prefab/Terrain/tile-plain_grass", typeof(GameObject)) as GameObject;
                    
                    break;
                }
                default:
                return;
            }
            // Vector3 objectSize_old = blk.GetComponent<Renderer>().bounds.size;
            // float scale_x=std_len/objectSize_old.x;
            // float scale_y=std_width/objectSize_old.y;
            
            Vector3 objectSize = blk.GetComponent<Renderer>().bounds.size;
  
            Vector3 pos=new Vector3(i*objectSize.x,0.01f,j*objectSize.z);
            Quaternion rot = Quaternion.Euler(0, 0, 0);
            GameObject the_block=Instantiate(blk,pos,rot) as GameObject;
            the_block.name="block"+i.ToString()+","+j.ToString();
            TagHelper.AddTag("block"+i+','+j);
            the_block.gameObject.tag="block"+i+','+j;
            Debug.Log(the_block.gameObject.tag);        
        }

        public void instantiate_terrain_block(BLOCK_TYPE b,int i,int j)
        {
            GameObject blk;
            switch(b)
            {
                case BLOCK_TYPE.ORDINARY:
                {
                    blk=Resources.Load("Prefab/House/ordinary", typeof(GameObject)) as GameObject;
                    break;
                }
                case BLOCK_TYPE.RESTAURANT:
                {
                    blk=Resources.Load("Prefab/House/building-restaurant", typeof(GameObject)) as GameObject;
                    break;
                }
                case BLOCK_TYPE.NONE:
                {
                    blk=Resources.Load("Prefab/Terrain/tile-plain_grass", typeof(GameObject)) as GameObject;
                    
                    break;
                }
                default:
                return;
            }
            
            Vector3 objectSize = blk.GetComponent<Renderer>().bounds.size;
  
            Vector3 pos=new Vector3(i*objectSize.x,0.01f,j*objectSize.z);
            Quaternion rot = Quaternion.Euler(0, 0, 0);
            Instantiate(blk,pos,rot);        
        }
    }
}