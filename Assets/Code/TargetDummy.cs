using UnityEngine;

public class TargetDummy : MonoBehaviour, ITarget
{
    public ParticleSystem deathEffect;

    public Transform graphics;
    private Vector3 originPosition;
    private float shakeDuration = 0f;

    private float health = 50f;

    public Vector3 WorldPosition => transform.position;

    public bool IsDead => health <= 0f;

    private void Start()
    {
        originPosition = graphics.localPosition;
    }

    public void Hit()
    {
        if (health <= 0f)
            return;

        shakeDuration = .2f;
        health -= Random.Range(5f, 15f);

        if (health <= 0f)
        {
            /*GetComponent<MeshRenderer>().enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;*/
            deathEffect.Play();
            Destroy(gameObject, 5f);
        }
    }

    private void Update()
    {
        if (shakeDuration > 0f)
        {
            shakeDuration -= Time.deltaTime;
            graphics.localPosition = originPosition + Random.insideUnitSphere * shakeDuration;

            if (shakeDuration <= 0f)
            {
                graphics.localPosition = originPosition;
            }
        }
    }
}
