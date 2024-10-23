using UnityEngine;

public class TargetDummy : MonoBehaviour, ITarget
{
    public ParticleSystem deathEffect;

    private Vector3 originPosition;
    private float shakeDuration = 0f;

    private float health = 50f;

    public Vector3 WorldPosition => transform.position;

    private void Start()
    {
        originPosition = transform.position;
    }

    public void Hit()
    {
        shakeDuration = .2f;
        health -= Random.Range(5f, 15f);

        if (health <= 0f)
        {
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;
            deathEffect.Play();
            Destroy(gameObject, 1f);
        }
    }

    private void Update()
    {
        if (shakeDuration > 0f)
        {
            shakeDuration -= Time.deltaTime;
            transform.position = originPosition + Random.insideUnitSphere * shakeDuration;

            if (shakeDuration <= 0f)
            {
                transform.position = originPosition;
            }
        }
    }
}
