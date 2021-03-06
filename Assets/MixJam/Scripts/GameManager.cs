﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MG
{
    namespace Evt
    {
        public class DicePickNumber : IEvent
        {
            public DiceInteractable dice;
            public int number;
            public DiceZone zone;
            public DicePickNumber(DiceInteractable dice, int number, DiceZone zone = null)
            {
                this.dice = dice;
                this.number = number;
                this.zone = zone;
            }
        }
        public class PlayerAtGoal : IEvent
        {
            public PlayerCharacter character;
            public GoalDestination goal;
            public PlayerAtGoal(PlayerCharacter character, GoalDestination goal)
            {
                this.character = character;
                this.goal = goal;
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
        public LevelData level;
        EventManager evtMgr = new EventManager();
        public bool menuScene = false;

        public List<Orderable> controlled = new List<Orderable>();

        public List<GameObject> winnObjects = new List<GameObject>();

        const string playerLevelKey = "STAGE_INDEX";

        public static EventManager Events
        {
            get
            {
                return Instance.evtMgr;
            }
        }

        void Start()
        {
            foreach(var item in winnObjects)
            {
                item.SetActive(false);
            }
            evtMgr.AddListener<Evt.DicePickNumber>(onDicePickNumber);
            evtMgr.AddListener<Evt.PlayerAtGoal>(onPlayerAtGoal);

            if (menuScene) return;

            if (level != null)
            {
                setGameIndex(level.levelIndex);
            }

            if (data != null && data.audioStartLevel)
            {
                data.audioStartLevel.trigger(gameObject);
            }
        }
        void Update()
        {
            evtMgr.doStep(Time.deltaTime);
        }

        public GameObject getPlayerAvatar()
        {
            if (controlled.Count == 0) return null;
            return controlled[0].gameObject;
        }
        public static void triggerFx(GameObject prefab, Vector3 pos, Fx.FxParams parameters = null)
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

        void onWin()
        {
            Debug.Log("Game Win");
            foreach (var item in winnObjects)
            {
                item.SetActive(true);
            }
            data.audioWin.trigger(gameObject);
        }
        void onLoose()
        {
            data.audioLoose.trigger(gameObject);
        }
        public void loadLevel(LevelData levelData)
        {
            SceneManager.LoadScene(levelData.levelIndex);
        }
        public void loadLevel(int i)
        {
            loadLevel(data.levels[i]);
        }
        public void restartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        public void onNewGame()
        {
            resetGameIndex();
            nextLevel();
        }
        public void nextLevel()
        {
            int index = SceneManager.GetActiveScene().buildIndex + 1;
            if (index > SceneManager.sceneCountInBuildSettings) return;
            SceneManager.LoadScene(index);
        }
        public void backToMenu()
        {
            SceneManager.LoadScene(0);
        }
        public void continueGame()
        {
            if (!canContinueGame())
            {
                SceneManager.LoadScene(1);
                return;
            }
            int levelIndex = PlayerPrefs.GetInt(playerLevelKey);
            SceneManager.LoadScene(levelIndex);
        }
        public bool canContinueGame()
        {
            return PlayerPrefs.HasKey(playerLevelKey) && PlayerPrefs.GetInt(playerLevelKey) > 1;
        }
        public void resetGameIndex()
        {
            PlayerPrefs.DeleteKey(playerLevelKey);
        }

        void setGameIndex(int index)
        {
            int i = PlayerPrefs.GetInt(playerLevelKey, 1);
            PlayerPrefs.SetInt(playerLevelKey, Mathf.Max(i, index));
        }
        public int getGameIndex()
        {
            if (!canContinueGame()) return 1;
            return PlayerPrefs.GetInt(playerLevelKey);
        }

        #region callback
        public void onDicePickNumber(Evt.DicePickNumber evt)
        {
            Debug.Log("GAME : dice pick number " + evt.number);
            string str = evt.number.ToString();
            int finalNumber = evt.number;
            if (evt.zone != null)
            {
                finalNumber = evt.zone.getCorrectNumber(evt.number);
                str = evt.number + evt.zone.getString() + " = " + finalNumber;
            }
            triggerFx(data.fxDicePickNumber, evt.dice.transform.position, new Fx.FxParams(str, evt.dice.color));

            if (evt.dice.diceType == DiceInteractable.DICE_TYPE.BOOM && finalNumber > 0)
            {
                Explosion expl = GameObject.Instantiate(data.diceExplosion);
                expl.excludeRigidFromForceApplied.Add(evt.dice.GetComponent<Rigidbody>());
                expl.transform.position = evt.dice.transform.position;
                expl.setRange(finalNumber);
            }
            else if (evt.dice.diceType == DiceInteractable.DICE_TYPE.SHIELD && finalNumber > 0)
            {
                Shield expl = GameObject.Instantiate(data.diceShield);
                expl.init(evt.dice, evt.number);
                expl.transform.position = evt.dice.transform.position;
                //expl.transform.SetParent(evt.dice.transform);
            }

            foreach (var item in controlled)
            {
                if (item == null) continue;
                item.onDiceLaunched(evt.dice, finalNumber, evt.zone);
            }
        }

        void onPlayerAtGoal(Evt.PlayerAtGoal evt)
        {
            Debug.Log("GAME : player at goal");
            onWin();
        }
        #endregion
    }
}
