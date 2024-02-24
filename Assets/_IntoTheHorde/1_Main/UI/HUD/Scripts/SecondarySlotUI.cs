using UnityEngine;
using TMPro;

namespace IntoTheHorde
{
    [DisallowMultipleComponent]
    public class SecondarySlotUI : MonoBehaviour
    {
        [SerializeField] TMP_Text _bulletsRemaining;

        public void SetNumBullets(int numBullets) => _bulletsRemaining.text = numBullets.ToString();

        public void TurnOffText() => _bulletsRemaining.text = "";
    }
}
