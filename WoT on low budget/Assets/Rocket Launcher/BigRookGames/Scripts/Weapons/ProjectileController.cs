using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BigRookGames.Weapons
{
    public class ProjectileController : MonoBehaviour
    {
        // --- Config ---
        public float speed = 100;
        public LayerMask collisionLayerMask;

        // --- Explosion VFX ---
        public GameObject rocketExplosion;

        // --- Projectile Mesh ---
        public MeshRenderer projectileMesh;

        // --- Script Variables ---
        private bool exploading;

        // --- Audio ---
        public AudioSource inFlightAudioSource;
        public AudioSource Bounce,TargetHit;

        // --- VFX ---
        public ParticleSystem disableOnHit;

        private bool impact = false, CollOn=false,waitframe=true;
        RaycastHit hit;
        Rigidbody rb;
        int layerMask = ~0;
        private void Start(){
            rb = GetComponent<Rigidbody>();
            if (Physics.Raycast(transform.position, transform.forward, out hit, (speed*Time.deltaTime)+0.4f, layerMask)){
                impact=true;
                transform.position += transform.forward * hit.distance;
                OnImpact();
            }else{
                rb.AddForce(transform.forward * speed,ForceMode.VelocityChange);
            }
        }
        private void FixedUpdate(){ 
            if (exploading) return;
            if (Physics.Raycast(transform.position, rb.velocity.normalized, out hit, (speed*Time.deltaTime)+0.3f, layerMask) && !impact){
                impact=true;
                transform.position += rb.velocity.normalized * hit.distance;
                rb.velocity=Vector3.zero;
                OnImpact();
            }
        }
        private void Update()
        {
            // --- Check to see if the target has been hit. We don't want to update the position if the target was hit ---
            if (exploading) return;
            if(!CollOn && !waitframe){
                GetComponent<SphereCollider>().enabled=true;
                CollOn=true;
            }
            waitframe = false;
            if(transform.position.y<-20f)
                Destroy(gameObject, 5f);
            Debug.DrawRay(transform.position,rb.velocity*Time.deltaTime,Color.red);
        }
        
        /// <summary>
        /// Explodes on contact.
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log("collision");
            Bounce.PlayOneShot(Bounce.clip);
        }


        /// <summary>
        /// Instantiates an explode object.
        /// </summary>
        private void OnImpact(){
            // --- return if not enabled because OnCollision is still called if compoenent is disabled ---
            if (!enabled) return;

            // --- Explode when hitting an object and disable the projectile mesh ---
            Explode();
            projectileMesh.enabled = false;
            exploading = true;
            inFlightAudioSource.Stop();
            foreach(Collider col in GetComponents<Collider>())
            {
                col.enabled = false;
            }
            disableOnHit.Stop();

            // --- Destroy this object after 2 seconds. Using a delay because the particle system needs to finish ---
            Destroy(gameObject, 5f);
        }
        private void Explode()
        {
            // --- Instantiate new explosion option. I would recommend using an object pool ---
            GameObject newExplosion = Instantiate(rocketExplosion, transform.position, rocketExplosion.transform.rotation, null);
            if(hit.transform.gameObject.layer==6){
                TargetHit.PlayOneShot(TargetHit.clip);
                Debug.Log("target");
            }
            Debug.Log("hit");
        }
    }
}