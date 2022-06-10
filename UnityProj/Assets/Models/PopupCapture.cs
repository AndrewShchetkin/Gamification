using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Models
{    
    public class PopupCapture : MonoBehaviour
    {
        HexCell currentHexCell;
        public HexMapEditor mapEditor;

        public HexCell CurrentHexCell { 
            get
            {
                return currentHexCell;
            } 
            set
            {
                if(value)
                {

                    BlockCell(CurrentHexCell);
                    ShowPopup();
                }
                currentHexCell = value;
            }
        }
        public void ShowPopup()
        {
            gameObject.SetActive(true);
        }

        public void OnOkClick()
        {
            gameObject.SetActive(false);
            mapEditor.CaptureCell(CurrentHexCell);
            CurrentHexCell = null;
        }

        public void OnCancelClick()
        {
            gameObject.SetActive(false);
        }

        [DllImport("__Internal")]
        private static extern void BlockCell(string jsonCell);

        void BlockCell(HexCell cell)
        {
            Cell targetCell = new Cell(cell.ColorIndex, cell.Elevation, cell.coordinates.X, cell.coordinates.Y, cell.coordinates.Z);
            targetCell.isBlocked = true;
            string jsonCell = JsonUtility.ToJson(targetCell);
#if UNITY_WEBGL == true && UNITY_EDITOR == false
				BlockCell(jsonCell);
#endif
        }
    }
}
