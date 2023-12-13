using UnityEngine;

namespace Gravitons.UI.Modal
{
    [CreateAssetMenu(fileName = "New Modal Database", menuName = "Gravitons/UI/Modal Database", order = 0)]
    public class ModalDatabase : ScriptableObject
    {
        [SerializeField] protected ModalData[] m_ModalData;

        public GameObject GetModal(string identifier)
        {
            for (int i = 0; i < m_ModalData.Length; i++)
            {
                if (m_ModalData[i].identifier == identifier)
                {
                    return m_ModalData[i].prefab;
                }
            }
            return null;
        }
    }

    [System.Serializable]
    public class ModalData
    {
        public string identifier;
        public GameObject prefab;
    }
}