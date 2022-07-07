using UnityEngine;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour
{
    public int m_PlayerNumber = 1;
    public Rigidbody m_Shell;
    public Rigidbody m_GiantShell;
    public Transform m_FireTransform;
    public Slider m_AimSlider;
    public AudioSource m_ShootingAudio;
    public AudioClip m_ChargingClip;
    public AudioClip m_FireClip;
    public float m_MinLaunchForce = 15f;
    public float m_MaxLaunchForce = 30f;
    public float m_MaxChargeTime = 0.75f;

    private string m_FireButton;
    private float m_CurrentLaunchForce;
    private float m_ChargeSpeed;
    private bool m_Fired;
    private float nextFireTime;
    private float nextExplosionTime;
    private float explosionCooldown;

    private void OnEnable()
    {
        m_CurrentLaunchForce = m_MinLaunchForce;
        m_AimSlider.value = m_MinLaunchForce;
    }


    private void Start()
    {
        m_FireButton = "Fire" + m_PlayerNumber;
        m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
        explosionCooldown = 5f;
        m_Fired = false;
    }


    private void Update()
    {
        m_AimSlider.value = m_MinLaunchForce;

        if (Input.GetKeyDown(KeyCode.E) && !m_Fired)
        // special exploding bullets key
        {
            GiantBulletFire(m_CurrentLaunchForce, 1);
            m_Fired = false;
        }
        else if (m_CurrentLaunchForce >= m_MaxLaunchForce && !m_Fired)
        {
            m_CurrentLaunchForce = m_MaxLaunchForce;
            Fire(m_CurrentLaunchForce, 1);
        }
        else if (Input.GetButtonDown(m_FireButton))
        {
            m_Fired = false;
            m_CurrentLaunchForce = m_MinLaunchForce;

            m_ShootingAudio.clip = m_ChargingClip;
            m_ShootingAudio.Play();
        }
        else if (Input.GetButton(m_FireButton) && !m_Fired)
        {
            m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;
            m_AimSlider.value = m_CurrentLaunchForce;
        }
        else if (Input.GetButtonUp(m_FireButton) && !m_Fired)
        {
            Fire(m_CurrentLaunchForce, 1);
        }

    }


    public void Fire(float launchForce, float fireRate)
    {
        if (Time.time <= nextFireTime) return;

        nextFireTime = Time.time + fireRate;
        m_Fired = true;

        Rigidbody shellInstance =
            Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
        shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward;

        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();

        m_CurrentLaunchForce = m_MinLaunchForce;
    }

    public void GiantBulletFire(float launchForce, float fireRate)
    {
        if (Time.time <= nextExplosionTime) return;
        // longer CD than regular shooting
        nextExplosionTime = Time.time + fireRate + explosionCooldown;
        m_Fired = true;
        Rigidbody shellInstance =
            Instantiate(m_GiantShell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
        shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward;

        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();

        m_CurrentLaunchForce = m_MinLaunchForce;
    }

}