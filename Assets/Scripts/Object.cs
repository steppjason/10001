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

    private const float OFFSET = 2;
    private const float OFFSCREEN = 1200;
    
    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseOver(){
        DisplayObjectInfo();
    }

    private void OnMouseExit() {
       ResetInfo();
    }

    private void DisplayObjectInfo(){
        uiPanel.GetComponent<RectTransform>().position = new Vector3(0,0 - OFFSET,0);
        ObjectName.SetText("[ " + objName + " ]");
        ObjectDescription.SetText(objDescription);
    }

    private void ResetInfo(){
        uiPanel.GetComponent<RectTransform>().position = new Vector3(OFFSCREEN,0,0);
    }
}
