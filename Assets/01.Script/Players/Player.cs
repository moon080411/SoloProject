using _01.Script.Entities;
using _01.Script.FSM;
using _01.Script.Items;
using _01.Script.Manager;
using _01.Script.SO;
using Plugins.ScriptFinder.RunTime.Finder;
using UnityEngine;

namespace _01.Script.Players
{
    public class Player : Entity
    {
        [field : SerializeField] public PlayerInputSO PlayerInput { get; private set; }

        [SerializeField] private StateDataSO[] states;
        
        [SerializeField] private ScriptFinderSO uiManagerFinder;

        private EntityStateMachine _stateMachine;
        
        private UIManager _uiManager;

        public bool IsUseCheat { get; set; }

        public Inventory ScInventory { get; private set; }
        
        public Mental ScMental { get; private set; }
        
        public SnowEffectGenerate ScSnow { get; private set; }

        [SerializeField] private GameObject infTorch;

        private float timer;

        private bool isChacking = true;
        
        
        protected override void Awake()
        {
            base.Awake();
            ScInventory = GetCompo<Inventory>();
            ScMental = GetCompo<Mental>();
            ScSnow = GetCompo<SnowEffectGenerate>();
            _uiManager = uiManagerFinder.GetTarget<UIManager>();
            _stateMachine = new EntityStateMachine(this, states);
            Time.timeScale = 1f;
            PlayerInput.OnClickEvent += Click;
            PlayerInput.OnInfPressed += InfMake;
        }

        private void InfMake()
        {
            IsUseCheat = true;
            GameObject temp = Instantiate(infTorch, transform.position + transform.forward + Vector3.up, Quaternion.identity);
            temp.GetComponent<Item>().SetRigidbody();
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
            PlayerInput.OnClickEvent -= Click;
            PlayerInput.OnInfPressed -= InfMake;
        }


        private void Start()
        {
            _stateMachine.ChangeState("IDLE");
            SoundManager.Instance.PlayBGM("Game");
        }

        private void Update()
        {
            _stateMachine.UpdateStateMachine();
            if (isChacking)
            {
                timer += Time.deltaTime;
                _uiManager.SetTimeText((int)timer);
            }
        }
        
        public void GameOver()
        {
            isChacking = false;
            PlayerInput.OnClickEvent -= Click;
            Time.timeScale = 0;
            _uiManager.ShowGameOver(timer);
        }

        public void ChangeState(string newStateName) => _stateMachine.ChangeState(newStateName);
    }
}
