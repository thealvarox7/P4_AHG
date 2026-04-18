
using UnityEngine;
[RequireComponent (typeof(Rigidbody))]
public class BulletPhysicsClase : MonoBehaviour
{
    [SerializeField] public float velocity = 0.5f;
    [SerializeField] public float lifetime = 3.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.linearVelocity = velocity * transform.forward;
        Destroy(this.gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log ("hit"+ other.name);
    }
    
}
   