﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MySecondGame
{
    public class BoatController
    {
        readonly GameObject boat;                   
        readonly Moveable moveableScript;                           
        readonly Vector3 fromPosition = new Vector3(5, 1, 0);
        readonly Vector3 toPosition = new Vector3(-5, 1, 0);
        readonly Vector3[] from_positions;                                          //回来角色要移动的位置
        readonly Vector3[] to_positions;                                            //去时角色要移动的位置

        // change frequently
        int to_or_from; // to->-1; from->1          
        MyCharacterController[] passenger = new MyCharacterController[2];           //乘客数组

        public BoatController()
        {
            to_or_from = 1;           

            from_positions = new Vector3[] { new Vector3(4.5F, 1.5F, 0), new Vector3(5.5F, 1.5F, 0) };
            to_positions = new Vector3[] { new Vector3(-5.5F, 1.5F, 0), new Vector3(-4.5F, 1.5F, 0) };

            boat = Object.Instantiate(Resources.Load("Prefabs/Boat", typeof(GameObject)), fromPosition, Quaternion.identity, null) as GameObject;
            boat.name = "boat";

            moveableScript = boat.AddComponent(typeof(Moveable)) as Moveable;
            boat.AddComponent(typeof(ClickGUI));
        }


        public void Move()                                          //设置船的移动方向
        {
            if (to_or_from == -1)
            {
                moveableScript.setDestination(fromPosition);
                to_or_from = 1;
            }
            else
            {
                moveableScript.setDestination(toPosition);
                to_or_from = -1;
            }
        }

        public int getEmptyIndex()               //获得船第一个为空的位置
        {
            for (int i = 0; i < passenger.Length; i++)
            {
                if (passenger[i] == null)
                {
                    return i;
                }
            }
            return -1;
        }

        public bool isEmpty()                   //判断船是否为空
        {
            for (int i = 0; i < passenger.Length; i++)
            {
                if (passenger[i] != null)
                {
                    return false;
                }
            }
            return true;
        }

        public Vector3 getEmptyPosition()       //获取第一个空的位置的Transform
        {
            Vector3 pos;
            int emptyIndex = getEmptyIndex();
            if (to_or_from == -1)
            {
                pos = to_positions[emptyIndex];
            }
            else
            {
                pos = from_positions[emptyIndex];
            }
            return pos;
        }

        public void GetOnBoat(MyCharacterController characterCtrl)      //上船
        {
            int index = getEmptyIndex();
            passenger[index] = characterCtrl;
        }

        public MyCharacterController GetOffBoat(string passenger_name)      //下船
        {
            for (int i = 0; i < passenger.Length; i++)
            {
                if (passenger[i] != null && passenger[i].getName() == passenger_name)
                {
                    MyCharacterController charactorCtrl = passenger[i];
                    passenger[i] = null;
                    return charactorCtrl;
                }
            }
            Debug.Log("Cant find passenger in boat: " + passenger_name);
            return null;
        }

        public GameObject getGameobj()          //获取船的实例
        {
            return boat;
        }

        public int get_to_or_from()             //获取船现在的运行状态
        { // to->-1; from->1
            return to_or_from;
        }

        public int[] getCharacterNum()           //获取船上的角色的数目
        {
            int[] count = { 0, 0 };
            for (int i = 0; i < passenger.Length; i++)
            {
                if (passenger[i] == null)
                    continue;
                if (passenger[i].getType() == 0)
                {   // 0->priest, 1->devil
                    count[0]++;
                }
                else
                {
                    count[1]++;
                }
            }
            return count;
        }

        public void reset()             //重置船
        {
            moveableScript.reset();
            if (to_or_from == -1)
            {
                Move();
            }
            passenger = new MyCharacterController[2];
        }
    }
}
