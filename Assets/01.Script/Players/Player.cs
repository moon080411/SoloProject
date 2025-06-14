using _01.Script.Entities;
using _01.Script.FSM;
using _01.Script.SO;
using UnityEngine;

namespace _01.Script.Players
{
    public class Player : Entity
    {
        [field : SerializeField] public PlayerInputSO PlayerInput { get; private set; }

        [SerializeField] private StateDataSO[] states;

        private EntityStateMachine _stateMachine;
        
        public Inventory ScInventory { get; private set; }
        
        public Mental ScMental { get; private set; }
        
        public SnowEffectGenerate ScSnow { get; private set; }
        
        
        protected override void Awake()
        {
            base.Awake();
            ScInventory = GetCompo<Inventory>();
            ScMental = GetCompo<Mental>();
            ScSnow = GetCompo<SnowEffectGenerate>();
            _stateMachine = new EntityStateMachine(this, states);
            PlayerInput.OnClickEvent += Click;
        }

        private void Click(int click)
        {
            RaycastHit hit = new RaycastHit();
            if (click == 0)
            {
                hit = PlayerInput.GetClickRay();
            }
            else if (click == 1)
            {
                hit = PlayerInput.GetInteractRay();
            }
            if (hit.collider != null && hit.collider.TryGetComponent(out IActionable actionable))
            {
                if (click == 0)
                {
                    actionable.Action();
                }
                else if (click == 1)
                {
                    actionable.ItemAction(ScInventory.GetCurrentItem());
                }
            }
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
