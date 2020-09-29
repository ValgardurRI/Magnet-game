using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;

namespace MagnetGame
{
    public class Utils
    {
        public static T GetComponentFromRaycast<T>(Vector2 position)
        {
            //Raycast on chessboard
            var results = new List<RaycastResult>();
            PointerEventData pointData = new PointerEventData(EventSystem.current);
            pointData.position = position;

            EventSystem.current.RaycastAll(pointData, results);
            //Find the cell that was raycast on
            var targetCell = results.Where(item => item.gameObject.GetComponent<T>() != null);
            if (targetCell.Count() != 0)
            {
                return targetCell.First().gameObject.GetComponent<T>();
            }
            else
            {
                return default(T);
            }
        }
    }
}