using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImage_FX : MonoBehaviour
{
    SpriteRenderer sr;
    float colorLoseRate;

    public void SetUpAfterImage(float _losingSpeed, Sprite _image)
    {
        sr = GetComponent<SpriteRenderer>();

        sr.sprite = _image;  
        colorLoseRate = _losingSpeed;
    }

    void Update()
    {
        float alpha = sr.color.a - colorLoseRate * Time.deltaTime;
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);

        if (sr.color.a <= 0)
            Destroy(gameObject);
    }
}
