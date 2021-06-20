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
        int playernum=0;

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
                GameGlobals.map[0][i].type = BLOCK_TYPE.P0;//a
                GameGlobals.map[len-1][i].type= BLOCK_TYPE.P0;//d
            }
            for(int i=0;i<len;++i)
            {
                GameGlobals.map[i][0].type= BLOCK_TYPE.P0;//s
                GameGlobals.map[i][width-1].type= BLOCK_TYPE.P0; //w
            }
        }

        private void scale_block(string dst,float std_len,float std_width,float std_height)
        {
            GameObject blk;
            blk=Resources.Load(dst, typeof(GameObject)) as GameObject;
            //init all scale to 1 first[]
            blk.transform.localScale =new Vector3(1f,1f,1f);
            Vector3 objectSize2 = blk.GetComponent<Renderer>().bounds.size;

            float scale_x=(float)std_len/objectSize2.x;
            float scale_y=(float)std_height/objectSize2.y;
            float scale_z=(float)std_width/objectSize2.z;
            blk.transform.localScale =new Vector3(scale_x,scale_y,scale_z);
        }

        public void instantiate_map()
        {
            len=15;
            width=10;

            if(!GameGlobals.map.Any()){
                init_global_map();
            }

            string grass="Prefab/Terrain/tile-plain_grass";
            string house_base="Prefab/Terrain/tile-road-straight";
       

            string p0="Prefab/House2/building-0";
            string p1l1="Prefab/House2/building-1-1";
            string p1l2="Prefab/House2/building-1-2";
            string p1l3="Prefab/House2/building-1-3";
            string p2l1="Prefab/House2/building-2-1";
            string p2l2="Prefab/House2/building-2-2";
            string p2l3="Prefab/House2/building-2-3";
            string p3l1="Prefab/House2/building-3-1";
            string p3l2="Prefab/House2/building-3-2";
            string p3l3="Prefab/House2/building-3-3";
            string p4l1="Prefab/House2/building-4-1";
            string p4l2="Prefab/House2/building-4-2";
            string p4l3="Prefab/House2/building-4-3";

            string tree1 = "Prefab/decorate/tree1";
            string tree2 = "Prefab/decorate/tree2";
            string tree3 = "Prefab/decorate/tree3";
            string guide = "Prefab/decorate/guidepost";
            string ball = "Prefab/decorate/ballon";
            string bike = "Prefab/decorate/bike-old";

            scale_block(p0,2f,2f,2f);
            scale_block(p1l1,2f,2f,2f);
            scale_block(p1l2,2f,2f,2f);
            scale_block(p1l3,2f,2f,2f);
            scale_block(p2l1,2f,2f,2f);
            scale_block(p2l2,2f,2f,2f);
            scale_block(p2l3,2f,2f,2f);
            scale_block(p3l1,2f,2f,2f);
            scale_block(p3l2,2f,2f,2f);
            scale_block(p3l3,2f,2f,2f);
            scale_block(p4l1,2f,2f,2f);
            scale_block(p4l2,2f,2f,2f);
            scale_block(p4l3,2f,2f,2f);
            scale_block(tree1,2f, 2f, 3f);
            scale_block(tree2, 2f, 2f, 3f);
            scale_block(tree3, 2f, 2f, 3f);
            scale_block(guide, 2f, 2f, 2f);
            scale_block(ball, 2f, 2f, 3f);
            scale_block(bike, 2f, 2f, 2f);
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
            for (int k = 2; k < 14; k=k+2) {
                gameObject.AddComponent<Block>().instantiate_block(BLOCK_TYPE.TREE1, k, 2) ;
                gameObject.AddComponent<Block>().instantiate_block(BLOCK_TYPE.TREE1, k, 7);
            }
            gameObject.AddComponent<Block>().instantiate_block(BLOCK_TYPE.BALL, 8, 6);
            gameObject.AddComponent<Block>().instantiate_block(BLOCK_TYPE.BIKE, 10, 4);
            gameObject.AddComponent<Block>().instantiate_block(BLOCK_TYPE.GUIDE, 3, 3);

        }

        public int type_to_next_level(int x ,int z ,changeType type)
        {
            int level=0;
            BLOCK_TYPE block= GameGlobals.map[x][z].type;
            if(type==changeType.LEVEL_UP)
            {
                if(block==BLOCK_TYPE.P0)
                    level=1;
                else if(block==BLOCK_TYPE.P1L1||block==BLOCK_TYPE.P2L1||block==BLOCK_TYPE.P3L1||block==BLOCK_TYPE.P4L1)
                    level=2;
                else if(block==BLOCK_TYPE.P1L2||block==BLOCK_TYPE.P2L2||block==BLOCK_TYPE.P3L2||block==BLOCK_TYPE.P4L2)
                    level=3;
                else
                    level=3;
            }
            else if(type==changeType.LEVEL_DOWN)
            {
                if(block==BLOCK_TYPE.P1L1||block==BLOCK_TYPE.P2L1||block==BLOCK_TYPE.P3L1||block==BLOCK_TYPE.P4L1)
                    level=0;
                else if(block==BLOCK_TYPE.P1L2||block==BLOCK_TYPE.P2L2||block==BLOCK_TYPE.P3L2||block==BLOCK_TYPE.P4L2)
                    level=1;
                else if(block==BLOCK_TYPE.P1L3||block==BLOCK_TYPE.P2L3||block==BLOCK_TYPE.P3L3||block==BLOCK_TYPE.P4L3)
                    level=2;
                else
                    level=0;
            }
            return level;
        }

        public void blockchanged(int x ,int z ,changeType type)
        {
            Debug.Log("block change");
            is_changed = true;
            changed_x = x;
            changed_z = z;
            playernum = GameGlobals.map[x][z].playernum;
            changed_level = type_to_next_level(x,z,type);
        }


        private void Update()
        {
            if (is_changed == true) {
                string old = "block" + changed_x + "," + changed_z;
                Destroy(GameObject.Find(old));
                Block block = gameObject.AddComponent<Block>();
                BLOCK_TYPE type=BLOCK_TYPE.P0;
                Debug.Log("level:"+changed_level);
                if(playernum==1)
                {
                    if(changed_level==1)
                        type=BLOCK_TYPE.P1L1;
                    if(changed_level==2)
                        type=BLOCK_TYPE.P1L2;
                    if(changed_level==3)
                        type=BLOCK_TYPE.P1L3;
                }
                if(playernum==2)
                {
                    if(changed_level==1)
                        type=BLOCK_TYPE.P2L1;
                    if(changed_level==2)
                        type=BLOCK_TYPE.P2L2;
                    if(changed_level==3)
                        type=BLOCK_TYPE.P2L3;
                }
                if(playernum==3)
                {
                    if(changed_level==1)
                        type=BLOCK_TYPE.P3L1;
                    if(changed_level==2)
                        type=BLOCK_TYPE.P3L2;
                    if(changed_level==3)
                        type=BLOCK_TYPE.P3L3;
                }
                if(playernum==4)
                {
                    if(changed_level==1)
                        type=BLOCK_TYPE.P4L1;
                    if(changed_level==2)
                        type=BLOCK_TYPE.P4L2;
                    if(changed_level==3)
                        type=BLOCK_TYPE.P4L3;
                }
                GameGlobals.map[changed_x][changed_z].type=type;
                block.instantiate_block(type, changed_x, changed_z);
                is_changed = false;
            }
        }
    }

}



// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using System.Linq;
// using Random=System.Random;
// using BlockNS;
// using typeNS;
// using GlobalsNS;


// namespace MapNS
// {
//     public class Map: MonoBehaviour
//     {
//         int len=15;
//         int width=10;
//         int block_type_size=4;
//         bool is_changed=false;
//         int changed_x=0;
//         int changed_z=0;
//         int changed_level=1;
//         int playernum=0;

//         //default map
//         public Map()
//         {

//         }


//         void init_global_map()
//         {
//             Debug.Log("store map");
//             for(int i=0;i<len;++i)
//             {
//                 List<BlockInfo> line=new List<BlockInfo>();
//                 for(int j=0;j<width;++j)
//                 {
//                     BlockInfo block=new BlockInfo();
//                     block.type=BLOCK_TYPE.NONE;
//                     block.pos_x=i;
//                     block.pos_y=j;
//                     block.playernum=-1;
//                     line.Add(block);
//                 }
//                 GameGlobals.map.Add(line);
//             }
//             for(int i=0;i<width;++i)
//             {
//                 GameGlobals.map[0][i].type = BLOCK_TYPE.P0;//a
//                 GameGlobals.map[len-1][i].type= BLOCK_TYPE.P0;//d
//             }
//             for(int i=0;i<len;++i)
//             {
//                 GameGlobals.map[i][0].type= BLOCK_TYPE.P0;//s
//                 GameGlobals.map[i][width-1].type= BLOCK_TYPE.P0; //w
//             }
//         }

//         private void scale_block(string dst,float std_len,float std_width,float std_height)
//         {
//             GameObject blk;
//             blk=Resources.Load(dst, typeof(GameObject)) as GameObject;
//             //init all scale to 1 first[]
//             blk.transform.localScale =new Vector3(1f,1f,1f);
//             Vector3 objectSize2 = blk.GetComponent<Renderer>().bounds.size;

//             float scale_x=(float)std_len/objectSize2.x;
//             float scale_y=(float)std_height/objectSize2.y;
//             float scale_z=(float)std_width/objectSize2.z;
//             blk.transform.localScale =new Vector3(scale_x,scale_y,scale_z);
//         }

//         public void instantiate_map()
//         {
//             len=15;
//             width=10;

//             if(!GameGlobals.map.Any())
//             {
//                 init_global_map();
//             }

//             string grass="Prefab/Terrain/tile-plain_grass";
//             string house_base="Prefab/Terrain/tile-road-straight";

//             string p0="Prefab/House2/building-0";
//             string p1l1="Prefab/House2/building-1-1";
//             string p1l2="Prefab/House2/building-1-2";
//             string p1l3="Prefab/House2/building-1-3";
//             string p2l1="Prefab/House2/building-2-1";
//             string p2l2="Prefab/House2/building-2-2";
//             string p2l3="Prefab/House2/building-2-3";
//             string p3l1="Prefab/House2/building-3-1";
//             string p3l2="Prefab/House2/building-3-2";
//             string p3l3="Prefab/House2/building-3-3";
//             string p4l1="Prefab/House2/building-4-1";
//             string p4l2="Prefab/House2/building-4-2";
//             string p4l3="Prefab/House2/building-4-3";

//             scale_block(p0,2f,2f,2f);
//             scale_block(p1l1,2f,2f,2f);
//             scale_block(p1l2,2f,2f,2f);
//             scale_block(p1l3,2f,2f,2f);
//             scale_block(p2l1,2f,2f,2f);
//             scale_block(p2l2,2f,2f,2f);
//             scale_block(p2l3,2f,2f,2f);
//             scale_block(p3l1,2f,2f,2f);
//             scale_block(p3l2,2f,2f,2f);
//             scale_block(p3l3,2f,2f,2f);
//             scale_block(p4l1,2f,2f,2f);
//             scale_block(p4l2,2f,2f,2f);
//             scale_block(p4l3,2f,2f,2f);

//             //terrain block
//             scale_block(grass,2f,2f,0.2f);
//             scale_block(house_base,2f,2f,0.8f);
//             //prefab scale shuould be add to another new file

//             for(int i=0;i<len;++i)
//             {
//                 for(int j=0;j<width;++j)
//                 {
//                     Block block= gameObject.AddComponent<Block>();
//                     BLOCK_TYPE type=GlobalsNS.GameGlobals.map[i][j].type;
//                     block.instantiate_block(type,i,j);
//                 }
//             }
//         }

//         public int type_to_next_level(int x ,int z ,changeType type)
//         {
//             int level=0;
//             BLOCK_TYPE block= GameGlobals.map[x][z].type;
//             if(type==changeType.LEVEL_UP)
//             {
//                 if(block==BLOCK_TYPE.P0)
//                     level=1;
//                 else if(block==BLOCK_TYPE.P1L1||block==BLOCK_TYPE.P2L1||block==BLOCK_TYPE.P3L1||block==BLOCK_TYPE.P4L1)
//                     level=2;
//                 else if(block==BLOCK_TYPE.P1L2||block==BLOCK_TYPE.P2L2||block==BLOCK_TYPE.P3L2||block==BLOCK_TYPE.P4L2)
//                     level=3;
//                 else
//                     level=3;
//             }
//             else if(type==changeType.LEVEL_DOWN)
//             {
//                 if(block==BLOCK_TYPE.P1L1||block==BLOCK_TYPE.P2L1||block==BLOCK_TYPE.P3L1||block==BLOCK_TYPE.P4L1)
//                     level=0;
//                 else if(block==BLOCK_TYPE.P1L2||block==BLOCK_TYPE.P2L2||block==BLOCK_TYPE.P3L2||block==BLOCK_TYPE.P4L2)
//                     level=1;
//                 else if(block==BLOCK_TYPE.P1L3||block==BLOCK_TYPE.P2L3||block==BLOCK_TYPE.P3L3||block==BLOCK_TYPE.P4L3)
//                     level=2;
//                 else
//                     level=0;
//             }
//             return level;
//         }

//         public void blockchanged(int x ,int z ,changeType type)
//         {
//             Debug.Log("block change");
//             is_changed = true;
//             changed_x = x;
//             changed_z = z;
//             playernum = GameGlobals.map[x][z].playernum;
//             changed_level = type_to_next_level(x,z,type);
//         }


//         private void Update()
//         {
//             if (is_changed == true) {
//                 string old = "block" + changed_x + "," + changed_z;
//                 Destroy(GameObject.Find(old));
//                 Block block = gameObject.AddComponent<Block>();
//                 BLOCK_TYPE type=BLOCK_TYPE.P0;
//                 Debug.Log("level:"+changed_level);
//                 if(playernum==1)
//                 {
//                     if(changed_level==1)
//                         type=BLOCK_TYPE.P1L1;
//                     if(changed_level==2)
//                         type=BLOCK_TYPE.P1L2;
//                     if(changed_level==3)
//                         type=BLOCK_TYPE.P1L3;
//                 }
//                 if(playernum==2)
//                 {
//                     if(changed_level==1)
//                         type=BLOCK_TYPE.P2L1;
//                     if(changed_level==2)
//                         type=BLOCK_TYPE.P2L2;
//                     if(changed_level==3)
//                         type=BLOCK_TYPE.P2L3;
//                 }
//                 if(playernum==3)
//                 {
//                     if(changed_level==1)
//                         type=BLOCK_TYPE.P3L1;
//                     if(changed_level==2)
//                         type=BLOCK_TYPE.P3L2;
//                     if(changed_level==3)
//                         type=BLOCK_TYPE.P3L3;
//                 }
//                 if(playernum==4)
//                 {
//                     if(changed_level==1)
//                         type=BLOCK_TYPE.P4L1;
//                     if(changed_level==2)
//                         type=BLOCK_TYPE.P4L2;
//                     if(changed_level==3)
//                         type=BLOCK_TYPE.P4L3;
//                 }
//                 GameGlobals.map[changed_x][changed_z].type=type;
//                 block.instantiate_block(type, changed_x, changed_z);
//                 is_changed = false;
//             }
//         }
//     }

// }

