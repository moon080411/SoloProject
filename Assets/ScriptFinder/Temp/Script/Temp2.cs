using Plugins.ScriptFinder.RunTime.Finder;
using UnityEngine;

namespace ScriptFinder.Temp.Script
{
    public class Temp2 : MonoBehaviour
    {
        [SerializeField] private ScriptFinderSO temp1Finder;
        private Temp1 _temp1;
        private void Awake()
        {
            //Use to Awake
            temp1Finder.GetTarget<Temp1>().Test();
            temp1Finder.GetTargetTransform().name = "Temp404";
            //Use to Update
            _temp1 = temp1Finder.GetTarget<Temp1>();
        }
        
        private void Update()
        {
            //Use to Update
            _temp1.TestInUpdate();
            //No use to Update
            //temp1Finder.GetTarget<Temp1>().TestInUpdate();
        }
    }
}
