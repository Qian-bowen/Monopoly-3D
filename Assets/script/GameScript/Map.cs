using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random=System.Random;
using BlockNS;
using typeNS;
using GlobalsNS;


namespace MapNS
{
    public class Map: MonoBehaviour
    {
        int len=15;
        int width=10;
        int block_type_size=4;
        bool is_changed=false;
        int changed_x=0;
        int changed_z=0;
        int changed_level=1;

        //default map
        public Map()
        {

        }


        void init_global_map()
        {
            Debug.Log("store map");
            for(int i=0;i<len;++i)
            {
                List<BlockInfo> line=new List<BlockInfo>();
                for(int j=0;j<width;++j)
                {
                    BlockInfo block=new BlockInfo();
                    block.type=BLOCK_TYPE.NONE;
                    block.pos_x=i;
                    block.pos_y=j;
                    line.Add(block);
                }
                GameGlobals.map.Add(line);
            }
            for(int i=0;i<width;++i)
            {
                GameGlobals.map[0][i].type = BLOCK_TYPE.ORDINARY;//a
                GameGlobals.map[len-1][i].type= BLOCK_TYPE.ORDINARY;//d
            }
            for(int i=0;i<len;++i)
            {
                GameGlobals.map[i][0].type= BLOCK_TYPE.ORDINARY;//s
                GameGlobals.map[i][width-1].type= BLOCK_TYPE.ORDINARY; //w
            }
        }

        private void scale_block(string dst,float std_len,float std_width,float std_height)
        {
            GameObject blk;
            blk=Resources.Load(dst, typeof(GameObject)) as GameObject;
            //init all scale to 1 first
            blk.transform.localScale =new Vector3(1f,1f,1f);
            Vector3 objectSize2 = blk.GetComponent<Renderer>().bounds.size;
            
            Debug.Log("init block size:"+objectSize2);
            float scale_x=(float)std_len/objectSize2.x;
            float scale_y=(float)std_height/objectSize2.y;
            float scale_z=(float)std_width/objectSize2.z;
            Debug.Log("scale:"+scale_x+' '+scale_z);
            //blk.transform.localScale =new Vector3(scale_x,scale_y,scale_z);
            blk.transform.localScale =new Vector3(scale_x,scale_y,scale_z);
        }

        public void instantiate_map()
        {
            len=15;
            width=10;

            if(!GameGlobals.map.Any())
            {
                init_global_map();
            }

            string grass="Prefab/Terrain/tile-plain_grass";
            string house="Prefab/House/ordinary";
            string restaurant="Prefab/House/building-restaurant";
            string hotel = "Prefab/House/building-hotel";
            string house_base="Prefab/Terrain/tile-road-straight";
            
            scale_block(house,2f,2f,2f);
            scale_block(restaurant,2f,2f,2f);
            scale_block(hotel, 2f, 2f, 2f);
            //terrain block
            scale_block(grass,2f,2f,0.2f);
            scale_block(house_base,2f,2f,0.8f);
            //prefab scale shuould be add to another new file

            for(int i=0;i<len;++i)
            {
                for(int j=0;j<width;++j)
                {
                    Block block= gameObject.AddComponent<Block>();
                    BLOCK_TYPE type=GlobalsNS.GameGlobals.map[i][j].type;
                    block.instantiate_block(type,i,j);
                }
            }
        }

        public void blockchanged(int x ,int z ,int level)
        {
            is_changed = true;
            changed_x = x;
            changed_z = z;
            changed_level = level;
            Debug.Log("change level:"+level);
        }
        private void Update()
        {
            if (is_changed == true) {
                string old = "block" + changed_x + "," + changed_z;
                Destroy(GameObject.Find(old));
                if (changed_level == 2) {
                    //map[changed_x][changed_z] = BLOCK_TYPE.RESTAURANT;
                    Block block = gameObject.AddComponent<Block>();
                    block.instantiate_block(GameGlobals.map[changed_x][changed_z].type, changed_x, changed_z);
                    is_changed = false;
                }
                if (changed_level == 3) {
                    //map[changed_x][changed_z] = BLOCK_TYPE.HOTEL;
                    Block block = gameObject.AddComponent<Block>();
                    block.instantiate_block(GameGlobals.map[changed_x][changed_z].type, changed_x, changed_z);
                    is_changed = false;
                }
                
            }
        }
    }

}

