using UnityEngine;

namespace _01.Script.FSM
{
    [CreateAssetMenu(fileName = "StateData", menuName = "SO/StateData", order = 0)]
    public class StateDataSO : ScriptableObject
    {
        public string stateName;
        public string className;
        public string animParamName;
        //hash�� public ���� ���ϸ� ������ �ȵǼ� ��Ÿ�ӿ��� ������ �����. ����!
        public int animationHash;

        private void OnValidate()
        {
            animationHash = Animator.StringToHash(animParamName);
        }
    }
}
