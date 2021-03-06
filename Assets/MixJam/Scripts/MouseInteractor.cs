﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MG
{
    public class MouseInteractor : MonoBehaviour
    {
        public Interactor interactor;
        public LayerMask layerInteractable;

        //Vector3 worldPosition;

        public float planeOffset = 1f;
        public float launchMultiplicator = 5f;
        public float launchUpMultiplicator = 5f;
        public float launchAngularVelocity = 1f;
        public float postLaunchFactorControl = 1f;
        public float postLaunchTimeInfluence = 4f;

        Vector3 currentVelocity;

        void Update()
        {
            updateMousePosition();

            if (interactor.hasObjectGrabbed() && isActionLaunch())
            {
                interactor.launchGrabbedObject(currentVelocity * launchMultiplicator + Vector3.up * launchUpMultiplicator, launchAngularVelocity);
            }

            if (isActionGrab())
            {
                Interactable obj = returnPointedInteractable();
                if (obj != null)
                {
                    interactor.addGrabbedObject(obj);
                }else
                {
                    StartZone diceZone = returnPointedZone();
                    if (diceZone != null && diceZone.dice != null && diceZone.dice.isGrabbable())
                    {
                        interactor.addGrabbedObject(diceZone.dice);
                    }
                }
                return;
            }
            interactor.influenceLastLaunchedObject(Vector3.Scale(new Vector3(1f,0f,1f),currentVelocity) * postLaunchFactorControl, postLaunchTimeInfluence);
        }

        bool isActionGrab()
        {
            return Input.GetMouseButton(0);
        }
        bool isActionLaunch()
        {
            return Input.GetMouseButtonUp(0);
        }


        Interactable returnPointedInteractable()
        {
            Interactable r = null;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, 100, layerInteractable, QueryTriggerInteraction.Collide);
            foreach (var item in hits)
            {
                r = item.collider.gameObject.GetComponent<Interactable>();
                if (r != null && r.isGrabbable()) return r;
            }
            return null;
        }

        StartZone returnPointedZone()
        {
            StartZone r = null;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, 100, layerInteractable, QueryTriggerInteraction.Collide);
            foreach (var item in hits)
            {
                r = item.collider.gameObject.GetComponent<StartZone>();
                if (r != null) return r;
            }
            return null;
        }

        void updateMousePosition()
        {
            Plane plane = new Plane(Vector3.up, -planeOffset);
            float distance;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (plane.Raycast(ray, out distance))
            {
                Vector3 pos = ray.GetPoint(distance);
                Vector3 vel = (pos - transform.position) / Time.deltaTime;
                currentVelocity = 0.6f * vel + 0.4f * currentVelocity;
                transform.position = pos;
            }
        }
    }
}
