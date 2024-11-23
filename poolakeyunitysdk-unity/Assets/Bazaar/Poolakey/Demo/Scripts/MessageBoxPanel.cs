using RTLTMPro;
using TMPro;
using UnityEngine;

namespace PoolakeyDemo
{
    public class MessageBoxPanel:MonoBehaviour
    {
        [SerializeField] private RTLTextMeshPro _text_message;

        public void Show(string message)
        {
            _text_message.Farsi = true;
            _text_message.text = message;
            gameObject.SetActive(true);
        }

        public void OnClickOk()
        {
            gameObject.SetActive(false);
        }
    }
}