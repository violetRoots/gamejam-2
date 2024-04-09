using UnityEngine;
using NaughtyAttributes;

namespace SkyCrush.WSGenerator
{
    [CreateAssetMenu(fileName = "WSG_Stage", menuName = "WSGenerator/Stage", order = 1)]
    public partial class Stage : ScriptableObject
    {
        [Space(20)]
        [SerializeField]
        private float duration;

        //todo переделать - генератор не должен иметь никаких ссылок на RiderGame
        [Space(10)]
        [SerializeField]
        private CustomStageData customData;

        [Space(10)]
        [ReorderableList]
        [SerializeField]
        private GenerateObjectInfo[] generateObjects;
    }
}
