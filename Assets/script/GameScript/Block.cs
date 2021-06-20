using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using typeNS;

namespace BlockNS
{
    public class Block : MonoBehaviour
    {
        private GameObject block;
        
        public void instantiate_block(BLOCK_TYPE b,int i,int j)
        {
            GameObject blk;
            switch(b)
            {
                case BLOCK_TYPE.P1L1:
                {
                    blk=Resources.Load("Prefab/House2/building-1-1", typeof(GameObject)) as GameObject;
                    break;
                }
                case BLOCK_TYPE.P1L2:
                {
                    blk=Resources.Load("Prefab/House2/building-1-2", typeof(GameObject)) as GameObject;
                    break;
                }
                case BLOCK_TYPE.P1L3:
                {
                    blk=Resources.Load("Prefab/House2/building-1-3", typeof(GameObject)) as GameObject;
                    break;
                }
                case BLOCK_TYPE.P2L1:
                {
                    blk=Resources.Load("Prefab/House2/building-2-1", typeof(GameObject)) as GameObject;
                    break;
                }
                case BLOCK_TYPE.P2L2:
                {
                    blk=Resources.Load("Prefab/House2/building-2-2", typeof(GameObject)) as GameObject;
                    break;
                }
                case BLOCK_TYPE.P2L3:
                {
                    blk=Resources.Load("Prefab/House2/building-2-3", typeof(GameObject)) as GameObject;
                    break;
                }
                case BLOCK_TYPE.P3L1:
                {
                    blk=Resources.Load("Prefab/House2/building-3-1", typeof(GameObject)) as GameObject;
                    break;
                }
                case BLOCK_TYPE.P3L2:
                {
                    blk=Resources.Load("Prefab/House2/building-3-2", typeof(GameObject)) as GameObject;
                    break;
                }
                case BLOCK_TYPE.P3L3:
                {
                    blk=Resources.Load("Prefab/House2/building-3-3", typeof(GameObject)) as GameObject;
                    break;
                }
                case BLOCK_TYPE.P4L1:
                {
                    blk=Resources.Load("Prefab/House2/building-4-1", typeof(GameObject)) as GameObject;
                    break;
                }
                case BLOCK_TYPE.P4L2:
                {
                    blk=Resources.Load("Prefab/House2/building-4-2", typeof(GameObject)) as GameObject;
                    break;
                }
                case BLOCK_TYPE.P4L3:
                {
                    blk=Resources.Load("Prefab/House2/building-4-3", typeof(GameObject)) as GameObject;
                    break;
                }
                case BLOCK_TYPE.P0:
                {
                    blk=Resources.Load("Prefab/House2/building-0", typeof(GameObject)) as GameObject;
                    break;
                }
                case BLOCK_TYPE.NONE:
                {
                    blk=Resources.Load("Prefab/Terrain/tile-plain_grass", typeof(GameObject)) as GameObject;
                    
                    break;
                }

                case BLOCK_TYPE.TREE1:
                {
                        blk = Resources.Load("Prefab/decorate/tree1", typeof(GameObject)) as GameObject;
                        break;
                }
                case BLOCK_TYPE.TREE2:
                    {
                        blk = Resources.Load("Prefab/decorate/tree2", typeof(GameObject)) as GameObject;
                        break;
                    }
                case BLOCK_TYPE.TREE3:
                    {
                        blk = Resources.Load("Prefab/decorate/tree3", typeof(GameObject)) as GameObject;
                        break;
                    }
                case BLOCK_TYPE.GUIDE:
                    {
                        blk = Resources.Load("Prefab/decorate/guidepost", typeof(GameObject)) as GameObject;
                        break;
                    }
                case BLOCK_TYPE.BIKE:
                    {
                        blk = Resources.Load("Prefab/decorate/bike-old", typeof(GameObject)) as GameObject;
                        Vector3 pos1 = new Vector3(i * blk.GetComponent<Renderer>().bounds.size.x, 0.2f, j * blk.GetComponent<Renderer>().bounds.size.z);
                        Quaternion rot1 = Quaternion.Euler(0, 70, 0);
                        GameObject the_block1 = Instantiate(blk, pos1, rot1) as GameObject;
                        the_block1.name = "block" + i.ToString() + "," + j.ToString();
                        return;
                    }
                case BLOCK_TYPE.BALL:
                    {
                        blk = Resources.Load("Prefab/decorate/ballon", typeof(GameObject)) as GameObject;
                        Vector3 pos1 = new Vector3(i * blk.GetComponent<Renderer>().bounds.size.x, 2f, j * blk.GetComponent<Renderer>().bounds.size.z);
                        Quaternion rot1 = Quaternion.Euler(0, 0, 0);
                        GameObject the_block1 = Instantiate(blk, pos1, rot1) as GameObject;
                        the_block1.name = "block" + i.ToString() + "," + j.ToString();
                        return;
                    }
                default:
                return;
            }
            
            Vector3 objectSize = blk.GetComponent<Renderer>().bounds.size;
  
            Vector3 pos=new Vector3(i*objectSize.x,0.01f,j*objectSize.z);
            Quaternion rot = Quaternion.Euler(0, 0, 0);
            GameObject the_block=Instantiate(blk,pos,rot) as GameObject;
            the_block.name="block"+i.ToString()+","+j.ToString();     
        }

        public void instantiate_terrain_block(BLOCK_TYPE b,int i,int j)
        {
            GameObject blk;
            switch(b)
            {
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


// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using typeNS;

// namespace BlockNS
// {
//     public class Block : MonoBehaviour
//     {
//         private GameObject block;
        
//         public void instantiate_block(BLOCK_TYPE b,int i,int j)
//         {
//             GameObject blk;
//             switch(b)
//             {
//                 case BLOCK_TYPE.P1L1:
//                 {
//                     blk=Resources.Load("Prefab/House2/building-1-1", typeof(GameObject)) as GameObject;
//                     break;
//                 }
//                 case BLOCK_TYPE.P1L2:
//                 {
//                     blk=Resources.Load("Prefab/House2/building-1-2", typeof(GameObject)) as GameObject;
//                     break;
//                 }
//                 case BLOCK_TYPE.P1L3:
//                 {
//                     blk=Resources.Load("Prefab/House2/building-1-3", typeof(GameObject)) as GameObject;
//                     break;
//                 }
//                 case BLOCK_TYPE.P2L1:
//                 {
//                     blk=Resources.Load("Prefab/House2/building-2-1", typeof(GameObject)) as GameObject;
//                     break;
//                 }
//                 case BLOCK_TYPE.P2L2:
//                 {
//                     blk=Resources.Load("Prefab/House2/building-2-2", typeof(GameObject)) as GameObject;
//                     break;
//                 }
//                 case BLOCK_TYPE.P2L3:
//                 {
//                     blk=Resources.Load("Prefab/House2/building-2-3", typeof(GameObject)) as GameObject;
//                     break;
//                 }
//                 case BLOCK_TYPE.P3L1:
//                 {
//                     blk=Resources.Load("Prefab/House2/building-3-1", typeof(GameObject)) as GameObject;
//                     break;
//                 }
//                 case BLOCK_TYPE.P3L2:
//                 {
//                     blk=Resources.Load("Prefab/House2/building-3-2", typeof(GameObject)) as GameObject;
//                     break;
//                 }
//                 case BLOCK_TYPE.P3L3:
//                 {
//                     blk=Resources.Load("Prefab/House2/building-3-3", typeof(GameObject)) as GameObject;
//                     break;
//                 }
//                 case BLOCK_TYPE.P4L1:
//                 {
//                     blk=Resources.Load("Prefab/House2/building-4-1", typeof(GameObject)) as GameObject;
//                     break;
//                 }
//                 case BLOCK_TYPE.P4L2:
//                 {
//                     blk=Resources.Load("Prefab/House2/building-4-2", typeof(GameObject)) as GameObject;
//                     break;
//                 }
//                 case BLOCK_TYPE.P4L3:
//                 {
//                     blk=Resources.Load("Prefab/House2/building-4-3", typeof(GameObject)) as GameObject;
//                     break;
//                 }
//                 case BLOCK_TYPE.P0:
//                 {
//                     blk=Resources.Load("Prefab/House2/building-0", typeof(GameObject)) as GameObject;
//                     break;
//                 }
//                 case BLOCK_TYPE.NONE:
//                 {
//                     blk=Resources.Load("Prefab/Terrain/tile-plain_grass", typeof(GameObject)) as GameObject;
                    
//                     break;
//                 }
//                 default:
//                 return;
//             }
            
//             Vector3 objectSize = blk.GetComponent<Renderer>().bounds.size;
  
//             Vector3 pos=new Vector3(i*objectSize.x,0.01f,j*objectSize.z);
//             Quaternion rot = Quaternion.Euler(0, 0, 0);
//             GameObject the_block=Instantiate(blk,pos,rot) as GameObject;
//             the_block.name="block"+i.ToString()+","+j.ToString();     
//         }

//         public void instantiate_terrain_block(BLOCK_TYPE b,int i,int j)
//         {
//             GameObject blk;
//             switch(b)
//             {
//                 case BLOCK_TYPE.NONE:
//                 {
//                     blk=Resources.Load("Prefab/Terrain/tile-plain_grass", typeof(GameObject)) as GameObject;
                    
//                     break;
//                 }
//                 default:
//                 return;
//             }
            
//             Vector3 objectSize = blk.GetComponent<Renderer>().bounds.size;
  
//             Vector3 pos=new Vector3(i*objectSize.x,0.01f,j*objectSize.z);
//             Quaternion rot = Quaternion.Euler(0, 0, 0);
//             Instantiate(blk,pos,rot);        
//         }
//     }
// }
