using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUpText_FX : MonoBehaviour
{
    TextMeshPro myText;

    [SerializeField] float speed;
    [SerializeField] float disapearanceSpeed;
    [SerializeField] float colorDisapearanceSpeed;

    [SerializeField] float lifetime;

    float textTimer;
        
    void Start()
    {
        myText = GetComponent<TextMeshPro>();
        textTimer = lifetime;
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(
                    transform.position,
                    new Vector2(transform.position.x, transform.position.y + 1),
                    speed * Time.deltaTime
                    );

        textTimer -= Time.deltaTime;

        if (textTimer < 0)
        {
            float alpha = myText.color.a - colorDisapearanceSpeed * Time.deltaTime;

            myText.color = new Color(myText.color.r, myText.color.g, myText.color.b, alpha);

            if (myText.color.a < 50)
                speed = disapearanceSpeed;

            if (myText.color.a < 0)
                Destroy(gameObject);
        }
    }
}
