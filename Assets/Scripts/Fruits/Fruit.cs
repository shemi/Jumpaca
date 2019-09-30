using UnityEngine;

public class Fruit : MonoBehaviour
{

    public float pushVelocity = 10f;
    public string type;

    public bool isInitialPush = false;

    private Animator _animator;
    private BoxCollider2D _collider;
    
    void Start()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<BoxCollider2D>();
    }

    public void Hit()
    {
        _collider.enabled = false;
        _animator.SetBool("Explode", true);
        SoundManager.instance.JumpSoundFX();
    }

    public void AfterExplodeAnimation()
    {
        Destroy(gameObject, 0f);
    }
    
}
