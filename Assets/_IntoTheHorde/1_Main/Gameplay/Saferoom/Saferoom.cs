using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using Trisibo;

namespace IntoTheHorde
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof( Collider ))]
    public class Saferoom : MonoBehaviour
    {
        [SerializeField] SceneField      _nextScene;
        [SerializeField] DoorInteraction _saferoomDoor;

        // Keep track of number of actors in each team that entered the collider
        Dictionary<GameActor.TEAM, int> m_actorCountMap = new Dictionary<GameActor.TEAM, int>();

        void OnEnable()
        {
            EventManager.AddListener(GameEvent.OnDoorOpened, DoorInteractionHandler);
            EventManager.AddListener(GameEvent.OnDoorClosed, DoorInteractionHandler);
        }

        void OnDisable()
        {
            EventManager.RemoveListener(GameEvent.OnDoorOpened, DoorInteractionHandler);
            EventManager.RemoveListener(GameEvent.OnDoorClosed, DoorInteractionHandler);
        }

        void Awake() => GetComponent<Collider>().isTrigger = true;

        void Start()
        {
            m_actorCountMap[GameActor.TEAM.SURVIVOR] = 0;
            m_actorCountMap[GameActor.TEAM.ZOMBIE]   = 0;
        }

        void OnTriggerEnter(Collider other)
        {
            GameActor actor = other.GetComponent<GameActor>();

            if (actor != null)
                ++m_actorCountMap[actor.Team];

            if (CanLoadNextScene())
                SceneManager.LoadScene(_nextScene.BuildIndex);
        }

        void OnTriggerExit(Collider other)
        {
            GameActor actor = other.GetComponent<GameActor>();

            if (actor != null)
                --m_actorCountMap[actor.Team];

            if (CanLoadNextScene())
                SceneManager.LoadScene(_nextScene.BuildIndex);
        }

        bool CanLoadNextScene()
        {
            return m_actorCountMap[GameActor.TEAM.SURVIVOR] == 1 &&
                   m_actorCountMap[GameActor.TEAM.ZOMBIE]   == 0 &&
                   !_saferoomDoor.IsOpen;
        }

        void DoorInteractionHandler(EventArgs eventArgs)
        {
            if (CanLoadNextScene())
            {
                EventManager.RaiseEvent( GameEvent.OnGameEnd, EventArgs.Empty );
                SceneManager.LoadScene(_nextScene.BuildIndex);
            }
        }
    }
}
