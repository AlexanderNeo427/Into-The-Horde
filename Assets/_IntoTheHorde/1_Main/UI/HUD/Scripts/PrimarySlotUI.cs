using UnityEngine;
using TMPro;

namespace IntoTheHorde
{
    [DisallowMultipleComponent]
    public class PrimarySlotUI : MonoBehaviour
    {
        [SerializeField] TMP_Text _bulletsLeftInMag;
        [SerializeField] TMP_Text _reserveAmmo;

        public void SetInfo(int bulletsLeftInMag, int reserveAmmo)
        {
            _bulletsLeftInMag.text = bulletsLeftInMag.ToString();
            _reserveAmmo.text      = reserveAmmo.ToString();
        }

        public void TurnOffText()
        {
            _bulletsLeftInMag.text = "";
            _reserveAmmo.text      = "";
        }
    }
}
