using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Behaviors
{
    /// <summary>
    /// Set size of ui elements in the page
    /// </summary>
    [ExecuteInEditMode]
    public class AdjustmentScreen : MonoBehaviour
    {
        bool isLandscape = true;
        /// <summary>
        /// Unity awake method
        /// </summary>
        void Awake()
        {
            ScoreBoardAdjustment();
        }

        /// <summary>
        /// Unity update method
        /// </summary>
        void Update()
        {           
            ScoreBoardAdjustment();
        }

        void ScoreBoardAdjustment()
        {
            if (Screen.width > Screen.height)
            {
                gameObject.GetComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
                gameObject.GetComponent<CanvasScaler>().scaleFactor = ((float)Screen.width / 3000);
                isLandscape = true;
            }
            else
            {
                gameObject.GetComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                isLandscape = false;
            }
        }
    }
}
