using UnityEngine;  namespace Menu {     public class GameMenu : MonoBehaviour     {         [SerializeField]private Inventory _inventory;         [SerializeField]private Menu _pauseMenu;         [SerializeField]private Menu _interface;         public bool IsPaused         {             get => _isPaused;             private set             {                 _isPaused = value;                 if (_isPaused)                 {                     Time.timeScale = 0f;                 }                 else                 {                     Time.timeScale = 1f;                 }             }         }
        private bool _isPaused;
        public Mode State
        {
            get => _state;
            set
            {
                switch (value)
                {
                    case Mode.Game:
                        SettingsGameMode();
                        break;
                    case Mode.Pause:
                        SettingsPauseMode();
                        break;
                    case Mode.Inventory:
                        SettingsInventoryMode();
                        break;
                    default:
                        value = Mode.Game;
                        break;
                }
                _state = value;
            }
        }
        private Mode _state; 
        private InputСontroller _inputController;         public enum Mode
        {
            Game,
            Pause,
            Inventory
        }         private void SettingsGameMode()
        {
            _inventory.Hide();
            _pauseMenu.Hide();
            _interface.Show();
            IsPaused = false;
        }         private void SettingsPauseMode()
        {
            _inventory.Hide();
            _pauseMenu.Show();
            _interface.Show();
            IsPaused = true;
        }         private void SettingsInventoryMode()
        {
            _inventory.Show();
            _pauseMenu.Hide();
            _interface.Hide();
            IsPaused = true;
        }         private void Awake()         {             _inputController = new();             _inputController.Player.Inventory.performed += context => SwitchInventoryMode();             _inputController.Player.Pause.performed += context => SwitchPauseMode();             State = Mode.Game;         }         private void OnValidate()
        {
            if (_inventory is null ||
                _pauseMenu is null ||
                _interface is null)
            {
                Debug.LogError("Not set any Menus in GameMenu!");
            }
        }         public void SwitchInventoryMode()         {             SwitchMode(Mode.Inventory);         }         public void SwitchPauseMode()
        {
            SwitchMode(Mode.Pause);
        }         private void SwitchMode( Mode mode)
        {
            if (State == mode)             {                 State = Mode.Game;             }             else             {                 State = mode;             }
        }         private void OnEnable() => _inputController.Enable();         private void OnDisable() => _inputController.Disable();     } }