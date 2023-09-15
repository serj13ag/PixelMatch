﻿using Data;

namespace Services
{
    public class PersistentDataService : IPersistentDataService
    {
        private readonly ISaveService _saveService;

        private PlayerData _playerData;

        public PlayerProgress Progress => _playerData.PlayerProgress;
        public GameSettings GameSettings => _playerData.GameSettings;

        public PersistentDataService(ISaveService saveService)
        {
            _saveService = saveService;
        }

        public void InitData(PlayerData savedPlayerData)
        {
            _playerData = savedPlayerData ?? new PlayerData();
        }

        public void Save()
        {
            _saveService.SaveData(_playerData);
        }

        public void ResetProgressAndSave()
        {
            _playerData.PlayerProgress = new PlayerProgress();

            Save();
        }
    }
}