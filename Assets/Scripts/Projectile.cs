using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float speed;
    private bool hit;
    private Animator anim;
    private float direction;
    private float lifetime;

    private BoxCollider2D boxCollider2D;
    void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    void OnEnable()
    {
        Invoke("DeactiveMe", 1.2f);
    }
    void Start()
    {
        Debug.Log("clicked");
    }

    public void DeactiveMe()
    {
        gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (hit) return;
        float movemementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movemementSpeed, 0, 0);
        lifetime += Time.deltaTime;
        if (lifetime > 5) gameObject.SetActive(false);

    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;
        boxCollider2D.enabled = false;
        anim.SetTrigger("explode");

        if (collision.tag == "Enemy")
            collision.GetComponent<Health>()?.TakeDamage(1);
    }
    public void SetDirection(float _direction)
    {
        lifetime = 0;
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;
        if (boxCollider2D != null)
            boxCollider2D.enabled = true;

        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != _direction)
        {
            localScaleX = -localScaleX;
            transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
        }
    }
    private void Deactivate()
    {
        gameObject.SetActive(false);
    }


}
