using System;

namespace Gravitons.UI.Modal
{
    using UnityEngine;

    public class ModalManager : MonoBehaviour
    {
        [Tooltip("The modal database reference, all modals will be retrieved from this database")]
        [SerializeField] protected ModalDatabase m_ModalDatabase;
        [Tooltip("The identifier of the default modal")]
        [SerializeField] protected string m_DefaultIdentifier = "Default";

        private static ModalManager s_Instance;

        private static ModalManager Instance
        {
            get { return s_Instance; }
        }

        public void OnEnable()
        {
            s_Instance = this;
        }

        private void OnDisable()
        {
            s_Instance = null;
        }

        /// <summary>
        /// Shows a generic (default) modal
        /// </summary>
        /// <param name="title">Modal title</param>
        /// <param name="body">Modal body</param>
        /// <param name="buttons">Modal button properties</param>
        /// <returns>The modal instantiated</returns>
        public static Modal Show(string title, string body, ModalButton[] buttons)
        {
            return Instance.InternalShow(Instance.m_DefaultIdentifier, new GenericModalContent(){ Title = title, Body = body} , buttons);
        }
        
        /// <summary>
        /// Shows a modal with the specified identifier
        /// </summary>
        /// <param name="identifier">The identifier of the modal to show</param>
        /// <param name="modalContent">The content to show in the modal</param>
        /// <param name="modalButton">Modal button properties</param>
        /// <returns>The modal instantiated</returns>
        public static Modal Show(string identifier, ModalContentBase modalContent, ModalButton[] modalButton)
        {
            return Instance.InternalShow(identifier, modalContent, modalButton);
        }

        /// <summary>
        /// Shows a generic (default) modal
        /// </summary>
        /// <param name="identifier">The identifier of the modal to show</param>
        /// <param name="modalContent">The content to show in the modal</param>
        /// <param name="modalButton">Modal button properties</param>
        /// <returns>The modal instantiated</returns>
        private Modal InternalShow(string identifier, ModalContentBase modalContent, ModalButton[] modalButton)
        {
            GameObject modalObj = m_ModalDatabase.GetModal(identifier);
            if (modalObj == null)
            {
                Debug.LogError("Error! Failed to get a modal prefab with the identifier: " + identifier + ". Ensure that the identifier matches the one in the database");
                return null;
            }
            
            GameObject clone = Instantiate(modalObj, transform);
            var modal = clone.GetComponent<Modal>();
            modal.Show(modalContent, modalButton);
            return modal;
        }
        
#if UNITY_2019_3_OR_NEWER
        /// <summary>
        /// Reset the static variables for domain reloading.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void DomainReset()
        {
            s_Instance = null;
        }
#endif
    }
}