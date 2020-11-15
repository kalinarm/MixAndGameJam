using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MG
{
    public class Projectile : MonoBehaviour
    {
        public float speed = 1f;
        public float lifetime = 5f;
        public float recoilForce = 1f;
        Rigidbody rigid;

        public AudioTrigger onCreate;
        public AudioTrigger onDestroy;
        public AudioTrigger onHitPlayer;
        public AudioTrigger onHitSomethingElse;

        void Start()
        {
            rigid = GetComponent<Rigidbody>();
            gameObject.AddComponent<Helpers.Temporary>().duration = lifetime;
            rigid.velocity = transform.forward * speed;
            if (onCreate != null) onCreate.trigger(gameObject);
        }

        public void autodestroy()
        {
            if (onDestroy != null) onDestroy.trigger(gameObject);
            GameObject.Destroy(gameObject);
        }

        void OnCollisionEnter(Collision col)
        {
            Avatar avatar = col.gameObject.GetComponent<Avatar>();
            if (avatar != null)
            {
                avatar.recoil(-(transform.position - avatar.transform.position).normalized * recoilForce);
                if (onHitPlayer != null) onHitPlayer.trigger(gameObject);
                autodestroy();
                return;
            }

            if (onHitSomethingElse != null) onHitSomethingElse.trigger(gameObject);
            autodestroy();
        }
    }
}
