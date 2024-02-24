using UnityEngine;
using TMPro;

namespace IntoTheHorde
{
    [DisallowMultipleComponent]
    public class Credits : MonoBehaviour
    {
        public static readonly float SPAWN_Y = 605f;

        [SerializeField] float      _creditsDuration = 10f;
        [SerializeField] float      _spawnRate = 2f;
        [SerializeField] GameObject _textPrefab;

        float m_credisTimer = 0f;
        float m_spawnTimer  = 0f;
        int   m_counter     = 0;

        void Start()
        {
            // Enable cursor in credits scene
            Cursor.visible   = true;
            Cursor.lockState = CursorLockMode.Confined;
        }

        void Update()
        {
            m_spawnTimer += Time.deltaTime;
            if (m_spawnTimer > _spawnRate)
            {
                m_spawnTimer = 0f;
                GameObject go = Instantiate( _textPrefab, transform );
                go.GetComponent<RectTransform>().anchoredPosition = Vector3.zero + Vector3.up * SPAWN_Y;

                string role = RoleLookupTable.Roles[m_counter];
                go.GetComponent<TextMeshProUGUI>().text = role + " - Alexander Neo";

                ++m_counter;
                if (m_counter >= RoleLookupTable.Roles.Length)
                {
                    int rand = (int)Random.Range( 0, RoleLookupTable.Roles.Length );
                    m_counter = rand;
                }
            }

            m_credisTimer += Time.deltaTime;
        }
    }

    public class RoleLookupTable
    {
        public static readonly string[] Roles =
        {
            "Production Manager", "Director","Costume Designer", "Casting Director","Camera operator","Boom operator",
            "Sound mixer","Props Master","Makeup Artist","Editor", "Assistant Art Director", "Set Designer",
            "Lead Storyboard Artist", "Assistant Storyboard Artist", "Conceptual Artist", "Graphic Designer", "On-set Consultant",
            "Cinematographer", "Music Supervisor", "Visual Effects Supervisor", "Associate Producer",
            "3D FX Animator", "Supervising Sound Editor", "Dialogue Editor", "Extras casting", "Projectionist",
            "Stunt Coordinator", "Emotional Supporter", "Post-production Lead", "Assistant Programmer", "Production Secretary",
            "Producer",
            "Lead programmer", "Game Designer", "AI Programmer", "Gameplay Programmer",
            "Artist", "Sound Designer", "QA Tester", "Project Manager",
            "Screenwriter", "Cinematographer", "Music Supervisor", "Visual Effects Supervisor", "Associate Producer",
            "3D FX Animator", "Supervising Sound Editor", "Dialogue Editor", "Extras casting", "Projectionist",
            "Stunt Coordinator", "Emotional Supporter", "Post-production Lead", "Assistant Programmer", "Production Secretary",
            "Producer",
            "Playtest Coordinator", "Creative Director", "Environment Artist",
            "Systems Designer", "Monetization Designers", "UX Researcher", "PR Manager", "UI Lead Designer",
        };
    }
}
