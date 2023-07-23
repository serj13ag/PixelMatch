using Enums;
using EventArguments;
using Infrastructure.StateMachine;
using Services.Mono.Sound;
using Services.UI;

namespace Services
{
    public class GameRoundService : IGameRoundService
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly ISoundMonoService _soundMonoService;
        private readonly IWindowService _windowService;
        private readonly IScoreService _scoreService;

        private readonly string _levelName;

        private bool _roundIsActive;

        public bool RoundIsActive => _roundIsActive;

        public GameRoundService(string levelName, GameStateMachine gameStateMachine, ISoundMonoService soundMonoService,
            IWindowService windowService, IScoreService scoreService)
        {
            _levelName = levelName;
            _gameStateMachine = gameStateMachine;
            _soundMonoService = soundMonoService;
            _windowService = windowService;
            _scoreService = scoreService;

            scoreService.OnScoreChanged += OnScoreChanged;
        }

        public void StartGame()
        {
            _windowService.ShowStartGameMessageWindow(_scoreService.ScoreGoal, StartRound);
        }

        public void EndRound()
        {
            if (_scoreService.ScoreGoalReached)
            {
                _soundMonoService.PlaySound(SoundType.Win);
                _windowService.ShowGameWinMessageWindow(ReloadLevel);
            }
            else
            {
                _soundMonoService.PlaySound(SoundType.Lose);
                _windowService.ShowGameOverMessageWindow(ReloadLevel);
            }

            _roundIsActive = false;
        }

        private void OnScoreChanged(object sender, ScoreChangedEventArgs e)
        {
            if (_scoreService.ScoreGoalReached)
            {
                EndRound();
            }
        }

        private void StartRound()
        {
            _roundIsActive = true;
        }

        private void ReloadLevel()
        {
            _gameStateMachine.Enter<GameLoopState, string>(_levelName);
        }
    }
}