using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random=System.Random;
using BlockNS;
using typeNS;

namespace MapNS
{
    public class Map: MonoBehaviour
    {
        int len;
        int width;
        int block_type_size=3;
        BLOCK_TYPE [][] map;

        //default map
        public Map()
        {
            len=15;
            width=10;

            map=new BLOCK_TYPE[len][];
            for(int i=0;i<len;++i)
            {
                map[i]=new BLOCK_TYPE[width];
            }

            for(int i=0;i<len;++i)
            {
                for(int j=0;j<width;++j)
                {
                    map[i][j]=BLOCK_TYPE.NONE;
                }
            }

            Random rnd = new Random();
            //the fringe is valid map
            for(int i=0;i<width;++i)
            {
                map[0][i]=(BLOCK_TYPE)rnd.Next(1,block_type_size);
                map[len-1][i]=(BLOCK_TYPE)rnd.Next(1,block_type_size);
            }
            for(int i=0;i<len;++i)
            {
                map[i][0]=(BLOCK_TYPE)rnd.Next(1,block_type_size);
                map[i][width-1]=(BLOCK_TYPE)rnd.Next(1,block_type_size);
            }
        }

        public Map(int l,int w,int[][] tmp_map)
        {
            len=l;
            width=w;
            map=new BLOCK_TYPE[len][];
            for(int i=0;i<len;++i)
            {
                map[i]=new BLOCK_TYPE[width];
            }
            for(int i=0;i<len;++i)
            {
                for(int j=0;j<width;++j)
                {
                    map[i][j]=(BLOCK_TYPE)tmp_map[i][j];
                }
            }
        }

        public bool edit_set_map_block(int i,int j,BLOCK_TYPE b)
        {
            if(i>=len||j>=width) return false;
            map[i][j]=b;
            return true;
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
            string grass="Prefab/Terrain/tile-plain_grass";
            string house="Prefab/House/ordinary";
            string restaurant="Prefab/House/building-restaurant";
            string house_base="Prefab/Terrain/tile-road-straight";
            
            scale_block(house,2f,2f,2f);
            scale_block(restaurant,2f,2f,2f);

            //terrain block
            scale_block(grass,2f,2f,0.2f);
            scale_block(house_base,2f,2f,0.8f);
            //prefab scale shuould be add to another new file

            for(int i=0;i<len;++i)
            {
                for(int j=0;j<width;++j)
                {
                    Block block= gameObject.AddComponent<Block>();
                    block.instantiate_block((BLOCK_TYPE)map[i][j],i,j);
                }
            }
        }

        public BLOCK_TYPE get_block_type(int i,int j)
        {
            return map[i][j];
        }

        public void convert_block_type(int i,int j,BLOCK_TYPE t)
        {
            map[i][j]=t;
        }
    }

}
