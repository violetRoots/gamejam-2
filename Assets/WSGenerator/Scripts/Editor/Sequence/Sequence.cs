using UnityEngine;
using NaughtyAttributes;

namespace SkyCrush.WSGenerator
{
    [CreateAssetMenu(fileName = "WSG_Sequence", menuName = "WSGenerator/Sequence", order = 0)]
    public partial class Sequence : ScriptableObject
    {
#if UNITY_EDITOR
        [Button("Settings")]
        [SerializeField]
        private void GoToSettings() => Settings.Select();
#endif

        //todo добавить легкое переключение и отрисовку пользовательских классов с данными
        //[SerializeField]
        //[ReadOnly]
        //private string additionalDataType;
        //[SerializeField]
        //[OnValueChanged(OnChangeTypeIndexMethodName)]
        //[Dropdown(TypeIndexDropdownName)]
        //private int typeIndex;

        //[SerializeField]
        //private object data;

        [ReorderableList]
        [SerializeField]
        private Stage[] fixedStages;
        [ReorderableList]
        [SerializeField]
        private Stage[] randomStages;
    }
}
