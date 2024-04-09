using System;
using UnityEngine;

namespace SkyCrush.WSGenerator
{
    //todo этого класса внутри генератора быть не должно, он внешний, нужно доделать реализацию отображени€ доп информации через аттрибуты (смотри SequenceCore)
    [Serializable]
    public class CustomStageData
    {
        public float MovementSpeed => movementSpeed;

        [SerializeField]
        private float movementSpeed;
    }
}
