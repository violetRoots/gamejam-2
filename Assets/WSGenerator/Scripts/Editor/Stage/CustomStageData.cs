using System;
using UnityEngine;

namespace SkyCrush.WSGenerator
{
    //todo ����� ������ ������ ���������� ���� �� ������, �� �������, ����� �������� ���������� ����������� ��� ���������� ����� ��������� (������ SequenceCore)
    [Serializable]
    public class CustomStageData
    {
        public float MovementSpeed => movementSpeed;

        [SerializeField]
        private float movementSpeed;
    }
}
