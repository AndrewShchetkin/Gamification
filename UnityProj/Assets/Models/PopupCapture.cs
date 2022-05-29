using System;
using System.Collections.Generic;
using System.Linq;
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
            mapEditor.EditCell(CurrentHexCell);
            CurrentHexCell = null;
        }

        public void OnCancelClick()
        {
            gameObject.SetActive(false);
        }
    }
}
