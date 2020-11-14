using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MG
{
    public class Shield : MonoBehaviour
    {
        //public float range = 1;
        public float duration = 3f;
        public float diceFactor = 1f;

        public DiceInteractable attachedToDice = null;

        public AudioTrigger fxOnProjectileImpact;
       
        public void init(DiceInteractable dice, int number)
        {
            attachedToDice = dice;
            //setRange(range);
            Invoke("autodestroy", duration * diceFactor * number);
        }

        public void autodestroy()
        {
            GameObject.Destroy(gameObject);
        }

        void setRange(float val)
        {
            transform.localScale = Vector3.one * val;
        }

        void OnTriggerEnter(Collider other)
        {
            Projectile obstacle = other.GetComponent<Projectile>();
            if (obstacle != null)
            {
                obstacle.autodestroy();
                if (fxOnProjectileImpact != null) fxOnProjectileImpact.trigger(obstacle.gameObject);
                return;
            }
        }
    }
}
