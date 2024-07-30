using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;

public class EntityFX : MonoBehaviour
{
    protected SpriteRenderer sr;
    protected Player player;

    [Header("PopUp Text")]
    [SerializeField] GameObject popupTextPrefab;

    [Header("Flash FX")]
    [SerializeField] float flashDuration;
    [SerializeField] Material hitMat;
    Material originalMat;

    [Header("Ailment Colors")]
    [SerializeField] Color[] chilledColor;
    [SerializeField] Color[] igniteColor;
    [SerializeField] Color[] shockedColor;

    [Header("Ailment Particles")]
    [SerializeField] ParticleSystem igniteFX;
    [SerializeField] ParticleSystem chillFX;
    [SerializeField] ParticleSystem shockFX;

    [Header("Hit FX")]
    [SerializeField] GameObject hitFXPrefab;
    [SerializeField] GameObject criticalHitFXPrefab;

    GameObject myHealthBar;

    protected virtual void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        myHealthBar = GetComponentInChildren<UI_HealthBar>().gameObject;

        player = PlayerManager.instance.player;
        
        originalMat = sr.material;
    }

    public void CreatePopUpText(string _text)
    {
        float randomX = Random.Range(-1, 1);
        float randomY = Random.Range(1.5f, 3);
        Vector3 positionOffset = new Vector3(randomX, randomY, 0);

        GameObject newText = Instantiate(popupTextPrefab, transform.position + positionOffset, Quaternion.identity);

        newText.GetComponent<TextMeshPro>().text = _text;
    }

    public void MakeTransparent(bool _isTransparent)
    {
        if (_isTransparent)
        {
            myHealthBar.SetActive(false);
            sr.color = Color.clear;
        }
        else
        { 
            myHealthBar.SetActive(true);
            sr.color = Color.white;
        }
    }

    IEnumerator FlashFX()
    {
        sr.material = hitMat;
        Color currentColor = sr.color;
        sr.color = Color.white;

        yield return new WaitForSeconds(flashDuration);

        sr.material = originalMat;
        sr.color = currentColor;
    }

    void RedColorBlink()
    {
        if (sr.color != Color.white)
            sr.color = Color.white;
        else
            sr.color = Color.red;
    }

    void IgniteColorFX()
    {
        if (sr.color != igniteColor[0])
            sr.color = igniteColor[0];
        else
            sr.color = igniteColor[1];
    }

    void ChillColorFX()
    {
        if (sr.color != chilledColor[0])
            sr.color = chilledColor[0];
        else
            sr.color = chilledColor[1];
    }

    public void IgniteFXFor(float _seconds)
    {
        igniteFX.Play();
        InvokeRepeating("IgniteColorFX", 0, 0.3f);
        Invoke("CancelColorChange", _seconds);
    }

    public void ChillFXFor(float _seconds)
    {
        chillFX.Play();
        InvokeRepeating("ChillColorFX", 0, 0.3f);
        Invoke("CancelColorChange", _seconds);
    }

    public void ShockFXFor(float _seconds)
    {
        shockFX.Play();
        InvokeRepeating("ShockColorFX", 0, 0.3f);
        Invoke("CancelColorChange", _seconds);
    }

    void ShockColorFX()
    {
        if (sr.color != shockedColor[0])
            sr.color = shockedColor[0];
        else
            sr.color = shockedColor[1];
    }

    void CancelColorChange()
    {
        CancelInvoke();
        sr.color = Color.white;

        igniteFX.Stop();
        chillFX.Stop();
        shockFX.Stop();
    }

    public void CreateHitFX(Transform _target, bool _critical)
    {
        float zRotation = Random.Range(-90f, 90f);
        float xPosition = Random.Range(-0.5f, 0.5f);
        float yPosition = Random.Range(-0.5f, 0.5f);

        Vector3 hitRotation = new Vector3(0, 0, zRotation);

        GameObject hitFX = hitFXPrefab;

        if (_critical)
        {
            hitFX = criticalHitFXPrefab;

            float yRotation = 0f;
            zRotation = Random.Range(-45, 45);

            if (GetComponent<Entity>().facingDir == -1)
                yRotation = 180;

            hitRotation = new Vector3(0, yRotation, zRotation);
        }

        GameObject newHitFX = Instantiate(hitFX, _target.position + new Vector3(xPosition, yPosition), Quaternion.identity);

        newHitFX.transform.Rotate(hitRotation);

        Destroy(newHitFX, 0.5f);
    }
}
