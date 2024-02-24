using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace IntoTheHorde
{
    public class CurrentStateText : MonoBehaviour
    {
        [SerializeField] RegularZombieController _zombie;
        [SerializeField] Health _zombieHealth;

        TMP_Text m_text;

        private void Awake()
        {
            m_text = GetComponent<TMP_Text>();
        }

        private void Update()
        {
            m_text.text = _zombie.CurrentState + ": " + _zombieHealth.Value;
            transform.position = _zombie.transform.position + Vector3.up * 3f;
        }
    }
}
