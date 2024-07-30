using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ToolTip : MonoBehaviour
{
    [SerializeField] float xLimit = 960f;
    [SerializeField] float yLimit = 540f;

    [SerializeField] float xOffset = 150f;
    [SerializeField] float yOffset = 150f;

    public virtual void AdjustPosition()
    { 
        Vector2 mousePos = Input.mousePosition;

        float newXOffset = 0f;
        float newYOffset = 0f;

        if (mousePos.x > xLimit)
            newXOffset = -xOffset;
        else
            newXOffset = xOffset;

        if (mousePos.y > yLimit)
            newYOffset = -yOffset;
        else
            newYOffset = yOffset + 200;

       transform.position = new Vector2(mousePos.x + newXOffset, mousePos.y + newYOffset);
    }

    public void AdjustFontSize(TextMeshProUGUI _text)
    {
        if (_text.text.Length > 12)
            _text.fontSize = _text.fontSize * 1f;
    }
}
