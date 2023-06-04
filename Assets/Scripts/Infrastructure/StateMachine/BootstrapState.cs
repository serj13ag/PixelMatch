﻿using Constants;

namespace Infrastructure.StateMachine
{
    public class BootstrapState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;

        public BootstrapState(GameStateMachine gameStateMachine, GlobalServices services)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = services.SceneLoader;

            services.InitGlobalServices();
        }

        public void Enter()
        {
            _sceneLoader.LoadScene(Settings.BootstrapSceneName, OnBootstrapSceneLoaded);
        }

        public void Exit()
        {
        }

        private void OnBootstrapSceneLoaded()
        {
            _gameStateMachine.Enter<LoadProgressState>();
        }
    }
}