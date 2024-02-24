
using UnityEngine;
using TMPro;

namespace IntoTheHorde
{
    [DisallowMultipleComponent]
    public class LeaderboardEntryView : MonoBehaviour
    {
        [SerializeField] TMP_Text _killsText;
        [SerializeField] TMP_Text _roundTimeText;

        public void SetInfo(int kills, float roundTime)
        {
            _killsText.text = kills.ToString();
            _roundTimeText.text = TimeConverter( roundTime );
        }

        /*
         *  Converts duration to a string in minutes and seconds 
         */
        string TimeConverter(float duration)
        {
            float minutes = (int)(duration / 60f);

            return ((int)(minutes)).ToString() + "min " + 
                   ((int)(duration % 60f)).ToString() + "sec"; ;
        }
    }
}
