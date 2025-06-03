using _01.Script.Entities;
using _01.Script.FSM;
using _01.Script.Items;
using _01.Script.SO;
using Plugins.SerializedFinder.RunTime.Dependencies;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

namespace _01.Script.Players
{
    public class Player : Entity, IDependencyProvider
    {
        [field : SerializeField] public PlayerInputSO PlayerInput { get; private set; }

        [SerializeField] private StateDataSO[] states;

        private EntityStateMachine _stateMachine;
        
        public Inventory Inventory { get; private set; }
        
        
        [Provide]
        public Player ProvidePlayer() => this;
        
        protected override void Awake()
        {
            base.Awake();
            Inventory = GetCompo<Inventory>();
            _stateMachine = new EntityStateMachine(this, states);
        }

        
        
        private void OnDestroy()
        {
        }


        private void Start()
        {
            _stateMachine.ChangeState("IDLE");
        }

        private void Update()
        {
            _stateMachine.UpdateStateMachine();
        }

        public void ChangeState(string newStateName) => _stateMachine.ChangeState(newStateName);
    }
}
