using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace YANG
{
    public class StatDisplay : MonoBehaviour
    {
        [SerializeField] private GameObject panelRoot;
        [SerializeField] private TMP_Text statsText;
        [SerializeField] private TMP_Text title;

        private void Awake()
        {
            Hide();
            title.text = "STATISTICS";
        }

        public void ShowUnitStats(Entity unit)
        {
            panelRoot.SetActive(true);
            statsText.text = unit.DisplayStats();
        }

        public void Hide()
        {
            panelRoot.SetActive(false);
        }
    }
}