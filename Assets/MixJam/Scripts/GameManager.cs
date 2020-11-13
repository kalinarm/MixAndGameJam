using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MG
{
    namespace Evt
    {
        public class DicePickNumber : IEvent
        {
            public DiceInteractable dice;
            public int number;
            public DicePickNumber(DiceInteractable dice, int number)
            {
                this.dice = dice;
                this.number = number;
            }
        }
    }

    public class GameManager : MonoBehaviour
    {
        #region singleton
        static GameManager instance;
        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = GameObject.FindObjectOfType<GameManager>();
                    if (instance == null)
                    {
                        instance = new GameObject("GameManager").AddComponent<GameManager>();
                    }
                }
                return instance;
            }
        }

        #endregion

        public GameManagerData data;

        EventManager evtMgr = new EventManager();

        public static EventManager Events
        {
            get
            {
                return Instance.evtMgr;
            }
        }

        void Start()
        {
            evtMgr.AddListener<Evt.DicePickNumber>(onDicePickNumber);
        }
        void Update()
        {
            evtMgr.doStep(Time.deltaTime);
        }

        void triggerFx(GameObject prefab, Vector3 pos, Fx.FxParams parameters = null)
        {
            if (prefab == null) return;
            GameObject obj = GameObject.Instantiate(prefab);
            obj.transform.position = pos;
            if (parameters != null)
            {
                Fx.FxParamsReader reader = obj.GetComponent<Fx.FxParamsReader>();
                if( reader != null)
                {
                    reader.Init(parameters);
                }
            }
        }

        #region callback
        public void onDicePickNumber(Evt.DicePickNumber evt)
        {
            Debug.Log("GAME : dice pick number " + evt.number);
            triggerFx(data.fxDicePickNumber, evt.dice.transform.position, new Fx.FxParams(evt.number.ToString(), evt.dice.color));
        }
        #endregion
    }
}
