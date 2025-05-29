using _01.Script.Entities;
using _01.Script.FSM;
using _01.Script.SO;
using Plugins.SerializedFinder.RunTime.Dependencies;
using UnityEngine;

namespace _01.Script.Players
{
    public class Player : Entity, IDependencyProvider
    {
        [field : SerializeField] public PlayerInputSO PlayerInput { get; private set; }

        [SerializeField] private StateDataSO[] states;

        private EntityStateMachine _stateMachine;
        
        
        [Provide]
        public Player ProvidePlayer() => this;
        
        protected override void Awake()
        {
            base.Awake();
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
