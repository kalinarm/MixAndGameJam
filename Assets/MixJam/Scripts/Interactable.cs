﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MG
{
    public class Interactable : MonoBehaviour
    {
        [SerializeField] bool grabbable = true;
        protected Rigidbody rigid;
        public Interactor attachedInteractor;

        [Header("Fx")]
        public AudioTrigger audioGrab;
        public AudioTrigger audioLaunch;
        public AudioTrigger audioCollideSelf;
        public AudioTrigger audioCollideWall;

        public virtual void Start()
        {
            rigid = GetComponent<Rigidbody>();
        }

        public virtual bool isGrabbable()
        {
            return grabbable;
        }
        public void setGrabbable(bool active)
        {
            grabbable = active;
        }
        public void grab()
        {
            setGrabbable(false);
            audioGrab.trigger(gameObject);
        }
        public void launch(Vector3 velocity, float angularVel = 0f)
        {
            detach();
            rigid.velocity = velocity;
            rigid.angularVelocity = new Vector3(Random.Range(-angularVel, angularVel), Random.Range(-angularVel, angularVel), Random.Range(-angularVel, angularVel));
            OnLaunched();
            audioLaunch.trigger(gameObject);
        }
        
        public virtual void attach(Interactor interactor)
        {
            attachedInteractor = interactor;
            ConfigurableJoint joint = gameObject.AddComponent<ConfigurableJoint>();
            joint.connectedBody = interactor.Rigid;
            //joint.spring = interactor.attachForce;
            //joint.damper = interactor.attachViscosity;
            joint.autoConfigureConnectedAnchor = false;
            joint.anchor = Vector3.zero;
            joint.connectedAnchor = Vector3.zero;

            joint.xMotion = ConfigurableJointMotion.Limited;
            joint.yMotion = ConfigurableJointMotion.Limited;
            joint.zMotion = ConfigurableJointMotion.Limited;

            var limit = joint.linearLimitSpring;
            limit.spring = interactor.attachForce;
            limit.damper = interactor.attachViscosity;
            joint.linearLimitSpring = limit;

        }
        public virtual void detach()
        {
            ConfigurableJoint joint = gameObject.GetComponent<ConfigurableJoint>();
            if (joint == null) return;
            GameObject.Destroy(joint);
            attachedInteractor = null;
        }

        #region virtual callback
        public virtual void OnLaunched()
        {

        }
        #endregion
    }
}