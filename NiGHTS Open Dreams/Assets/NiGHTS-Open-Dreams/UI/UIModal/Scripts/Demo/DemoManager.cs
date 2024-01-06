namespace Gravitons.UI.Modal
{
    using UnityEngine.UI;
    using UnityEngine;

    /// <summary>
    /// Manages the UI in the demo scene
    /// </summary>
    public class DemoManager : MonoBehaviour
    {
        public Button button;
        public Button button2;
        public Image image;

        private void Start()
        {
            button.onClick.AddListener(ShowModal);
            button2.onClick.AddListener(ShowModalWithCallback);
        }

        /// <summary>
        /// Show a simple modal
        /// </summary>
        private void ShowModal()
        {
            ModalManager.Show("Modal Title", "Show your message here", new[] { new ModalButton() { Text = "OK" } });
        }

        /// <summary>
        /// Shows a modal with callback
        /// </summary>
        private void ShowModalWithCallback()
        {
            ModalManager.Show("Change Background Color", "Change background color to a random color",
                new[] { new ModalButton() { Text = "YES", Callback = ChangeColor }, new ModalButton() { Text = "NO" } });
        }

        /// <summary>
        /// Change background color to a random color
        /// </summary>
        private void ChangeColor()
        {
            image.color = new Color(Random.value, Random.value, Random.value);
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(ShowModal);
            button2.onClick.RemoveListener(ShowModalWithCallback);
        }
    }
}