using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_FX : EntityFX
{
    [Header("After Image FX")]
    [SerializeField] float afterImageCooldown;
    [SerializeField] GameObject afterImagePrefab;
    [SerializeField] float colorLoseRate;
    float afterImageCooldownTimer;

    [Header("Screen Shake FX")]
    [SerializeField] float shakeMultiplier;
    public Vector3 shakeSwordImpact;
    public Vector3 shakeHighDamage;
    CinemachineImpulseSource screenShake;

    [Space]
    [SerializeField] ParticleSystem dustFX;

    protected override void Start()
    {
        base.Start();

        screenShake = GetComponent<CinemachineImpulseSource>();
    }

    void Update()
    {
        afterImageCooldownTimer -= Time.deltaTime;
    }

    public void CreateAfterImage()
    {
        if (afterImageCooldownTimer < 0)
        {
            afterImageCooldownTimer = afterImageCooldown;

            GameObject newAfterImage = Instantiate(afterImagePrefab, transform.position, transform.rotation);
            newAfterImage.GetComponent<AfterImage_FX>().SetUpAfterImage(colorLoseRate, sr.sprite);
        }
    }

    public void ScreenShake(Vector3 _shakePower)
    {
        screenShake.m_DefaultVelocity = new Vector3(_shakePower.x * player.facingDir, _shakePower.y) * shakeMultiplier;
        screenShake.GenerateImpulse();
    }

    public void PlayDustFX()
    {
        if (dustFX != null)
            dustFX.Play();
    }
}
