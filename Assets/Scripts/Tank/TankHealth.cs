using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour
{
    public float m_StartingHealth = 100f;
    public Slider m_Slider;
    public Image m_FillImage;
    public Color m_FullHealthColor = Color.green;
    public Color m_ZeroHealthColor = Color.red;
    public GameObject m_ExplosionPrefab;

    private AudioSource m_ExplosionAudio;
    public AudioSource audioSource;
    public AudioClip m_BonkAudioClip;

    private ParticleSystem m_ExplosionParticles;
    private float m_CurrentHealth;
    private bool m_Dead;
    private Rigidbody m_RigidBody;


    private void Awake()
    {
        m_ExplosionParticles = Instantiate(m_ExplosionPrefab).GetComponent<ParticleSystem>();
        m_ExplosionAudio = m_ExplosionParticles.GetComponent<AudioSource>();
        m_ExplosionParticles.gameObject.SetActive(false);
        m_RigidBody = gameObject.GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }


    private void OnEnable()
    {
        m_CurrentHealth = m_StartingHealth;
        m_Dead = false;
        SetHealthUI();
    }
    private void OnCollisionEnter(Collision collision)
    {
        // player decides to drive into non-tank objects
        if (collision.gameObject.tag != "Player")
        {
            //Debug.Log("ram occurred");
            var ramSpeed = collision.relativeVelocity.magnitude;
            if (ramSpeed > 0.005)
            {
                // die for going high speed into walls
                Debug.Log("WARNING PLAYER BOOM WALL");
                m_RigidBody.GetComponent<TankHealth>().TakeDamage(100);
            }
            // lose some hp for small bumps
            Debug.Log("player bonk wall");
            if (!m_Dead)
            {
                audioSource.PlayOneShot(m_BonkAudioClip, 1.0F);
                m_RigidBody.GetComponent<TankHealth>().TakeDamage(15);
            }

        }
    }

    public void TakeDamage(float amount)
    {
        // Adjust the tank's current health, update the UI based on the new health and check whether or not the tank is dead.
        m_CurrentHealth -= amount;

        SetHealthUI();
        if (m_CurrentHealth <= 0f && !m_Dead)
        {
            OnDeath();
        }
    }


    private void SetHealthUI()
    {
        // Adjust the value and colour of the slider.
        m_Slider.value = m_CurrentHealth;
        m_FillImage.color = Color.Lerp(m_ZeroHealthColor, m_FullHealthColor, m_CurrentHealth / m_StartingHealth);
    }


    private void OnDeath()
    {
        // Play the effects for the death of the tank and deactivate it.
        m_Dead = true;

        m_ExplosionParticles.transform.position = transform.position;
        m_ExplosionParticles.gameObject.SetActive(true);
        m_ExplosionParticles.Play();
        m_ExplosionAudio.Play();

        gameObject.SetActive(false);
    }
}