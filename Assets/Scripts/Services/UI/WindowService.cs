﻿using System;
using Constants;
using UI;
using UnityEngine;

namespace Services.UI
{
    public class WindowService : IWindowService
    {
        private readonly IUiFactory _uiFactory;
        private readonly IAssetProviderService _assetProviderService;

        public WindowService(IUiFactory uiFactory, IAssetProviderService assetProviderService)
        {
            _uiFactory = uiFactory;
            _assetProviderService = assetProviderService;
        }

        public void ShowStartGameMessageWindow(int scoreGoal, Action onButtonClickCallback)
        {
            ShowMessage(onButtonClickCallback, AssetPaths.GoalIconSpritePath, $"score goal\n{scoreGoal}", "start");
        }

        public void ShowGameWinMessageWindow(Action onButtonClickCallback)
        {
            ShowMessage(onButtonClickCallback, AssetPaths.WinIconSpritePath, "you win!", "ok");
        }

        public void ShowGameOverMessageWindow(Action onButtonClickCallback)
        {
            ShowMessage(onButtonClickCallback, AssetPaths.LoseIconSpritePath, "you lose!", "ok");
        }

        private void ShowMessage(Action onButtonClickCallback, string iconSpritePath, string message, string buttonText)
        {
            MessageInGameWindow messageWindow = _uiFactory.GetMessageWindow();
            Sprite goalIcon = _assetProviderService.LoadSprite(iconSpritePath);
            messageWindow.ShowMessage(goalIcon, message, buttonText, onButtonClickCallback);
        }
    }
}