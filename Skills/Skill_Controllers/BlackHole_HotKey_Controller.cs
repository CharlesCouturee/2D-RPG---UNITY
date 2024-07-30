using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlackHole_HotKey_Controller : MonoBehaviour
{
    SpriteRenderer sr;
    KeyCode myHotkey;
    TextMeshProUGUI myText;

    Transform myEnemy;
    Blackhole_Skill_Controller blackhole;

    public void SetUpHotkey(KeyCode _myNewHotKey, Transform _myEnemy, Blackhole_Skill_Controller _myBlackHole)
    {
        myText = GetComponentInChildren<TextMeshProUGUI>();
        sr = GetComponentInChildren<SpriteRenderer>();

        myEnemy = _myEnemy;
        blackhole = _myBlackHole;

        myHotkey = _myNewHotKey;
        myText.text = _myNewHotKey.ToString();
    }

    void Update()
    {
        if (Input.GetKeyDown(myHotkey))
        {
            blackhole.AddEnemyToList(myEnemy);

            myText.color = Color.clear;
            sr.color = Color.clear;
        }
    }
}
