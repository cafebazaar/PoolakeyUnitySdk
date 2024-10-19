using TMPro;
using UnityEngine;

namespace PoolakeyDemo
{
    public class MessageBoxPanel:MonoBehaviour
    {
        [SerializeField] private TMP_Text _text_message;

        public void Show(string message)
        {
            _text_message.text = message;
            gameObject.SetActive(true);
        }

        public void OnClickOk()
        {
            gameObject.SetActive(false);
        }
    }
}