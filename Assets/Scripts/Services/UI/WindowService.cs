﻿using System;
using Constants;
using Enums;
using UI;
using UI.Windows;
using UnityEngine;

namespace Services.UI
{
    public class WindowService : IWindowService
    {
        private readonly IUiFactory _uiFactory;
        private readonly ILocalizationService _localizationService;
        private readonly IAssetProviderService _assetProviderService;

        private BackgroundBlocker _backgroundBlocker;

        // TODO refactor

        public WindowService(IUiFactory uiFactory, ILocalizationService localizationService,
            IAssetProviderService assetProviderService)
        {
            _uiFactory = uiFactory;
            _localizationService = localizationService;
            _assetProviderService = assetProviderService;
        }

        public void ShowStartGameMessageWindow(int scoreGoal, Action onButtonClickCallback)
        {
            string translation = _localizationService.GetTranslation(TranslationKeys.ScoreGoal);
            string message = string.Format(translation, scoreGoal);
            string buttonText = _localizationService.GetTranslation(TranslationKeys.Start);
            ShowMessageWindow(onButtonClickCallback, AssetPaths.GoalIconSpritePath, message, buttonText);
        }

        public void ShowGameWinMessageWindow(Action onButtonClickCallback)
        {
            string message = _localizationService.GetTranslation(TranslationKeys.GameWinMessage);
            string buttonText = _localizationService.GetTranslation(TranslationKeys.Ok);
            ShowMessageWindow(onButtonClickCallback, AssetPaths.WinIconSpritePath, message, buttonText);
        }

        public void ShowGameOverMessageWindow(Action onButtonClickCallback)
        {
            string message = _localizationService.GetTranslation(TranslationKeys.GameOverMessage);
            string buttonText = _localizationService.GetTranslation(TranslationKeys.Ok);
            ShowMessageWindow(onButtonClickCallback, AssetPaths.LoseIconSpritePath, message, buttonText);
        }

        public void ShowWindow(WindowType windowType)
        {
            ShowBackgroundBlocker();

            BaseWindow baseWindow = windowType switch
            {
                WindowType.PuzzleLevels => _uiFactory.CreatePuzzleLevelsWindow(),
                WindowType.Settings => _uiFactory.CreateSettingsWindow(),
                _ => throw new ArgumentOutOfRangeException(nameof(windowType), windowType, null),
            };

            baseWindow.Show();

            baseWindow.OnHided += OnWindowHided;
        }

        public void Cleanup()
        {
            _backgroundBlocker = null;
        }

        private void OnWindowHided(object sender, EventArgs e)
        {
            BaseWindow baseWindow = (BaseWindow)sender;
            baseWindow.OnHided -= OnWindowHided;

            _backgroundBlocker.Deactivate();
        }

        private void ShowMessageWindow(Action onButtonClickCallback, string iconSpritePath, string message,
            string buttonText)
        {
            ShowBackgroundBlocker();

            MessageInGameWindow messageWindow = _uiFactory.GetMessageWindow();
            Sprite goalIcon = _assetProviderService.LoadSprite(iconSpritePath);
            messageWindow.ShowMessage(goalIcon, message, buttonText, onButtonClickCallback);

            messageWindow.OnHided += OnWindowHided;
        }

        private void ShowBackgroundBlocker()
        {
            if (_backgroundBlocker == null)
            {
                _backgroundBlocker = _uiFactory.CreateBackgroundBlocker();
            }

            _backgroundBlocker.Activate();
        }
    }
}