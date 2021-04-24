using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Object : MonoBehaviour
{
    [SerializeField] string objName = "[TERRA]";
    [SerializeField] string objDescription = "{Home}";

    [SerializeField] Image uiPanel;
    [SerializeField] TextMeshProUGUI ObjectName;
    [SerializeField] TextMeshProUGUI ObjectDescription;

    private const float OFFSET = 3f;
    private const float OFFSCREEN = 1200;
    

    private void OnMouseOver(){
        DisplayObjectInfo();
    }

    private void OnMouseExit() {
       ResetInfo();
    }

    private void DisplayObjectInfo(){

        Vector3 screenPos = Camera.main.WorldToScreenPoint(this.transform.position);
        uiPanel.transform.position = screenPos;
        uiPanel.transform.position = new Vector3(uiPanel.transform.position.x, uiPanel.transform.position.y - 64, 0);


        ObjectName.SetText("[ " + objName + " ]");
        ObjectDescription.SetText(objDescription);
    }

    private void ResetInfo(){
        uiPanel.GetComponent<RectTransform>().position = new Vector3(0,OFFSCREEN,0);
    }
}
