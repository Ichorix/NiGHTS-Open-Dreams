namespace Gravitons.UI.Modal
{
    using UnityEngine;

    /// <summary>
    /// Base class for all modals
    /// </summary>
    public abstract class Modal : MonoBehaviour
    {
        /// <summary>
        /// Closes this modal
        /// </summary>
        public virtual void Close()
        {
            Destroy(gameObject);
        }

        /// <summary>
        /// Shows the modal with the given content
        /// </summary>
        /// <param name="modalContent">Content to show</param>
        /// <param name="modalButton">Button properties</param>
        public abstract void Show(ModalContentBase modalContent, ModalButton[] modalButton);
        
    }
}