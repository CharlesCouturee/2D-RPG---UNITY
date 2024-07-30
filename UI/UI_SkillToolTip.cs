using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_SkillToolTip : UI_ToolTip
{
    [SerializeField] TextMeshProUGUI skillDescription;
    [SerializeField] TextMeshProUGUI skillName;
    [SerializeField] TextMeshProUGUI skillCost;
    [SerializeField] float defaultNameFontSize;

    public void ShowToolTip(string _skillDescription, string _skillName, int _skillCost)
    {
        skillCost.text = "Cost: " + _skillCost; 
        skillName.text = _skillName;
        skillDescription.text = _skillDescription;

        AdjustPosition();
        //AdjustFontSize(skillName);

        gameObject.SetActive(true);
    }

    public void HideToolTip()
    {
        skillName.fontSize = defaultNameFontSize;
        gameObject.SetActive(false);
    }
}
