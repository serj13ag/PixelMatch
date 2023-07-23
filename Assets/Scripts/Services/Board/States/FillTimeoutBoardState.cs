﻿using System.Collections.Generic;
using Constants;
using Entities;

namespace Services.Board.States
{
    public class FillTimeoutBoardState : BaseTimeoutBoardState
    {
        private readonly IBoardService _boardService;
        private readonly IGamePieceService _gamePieceService;

        private int _numberOfGamePiecesToMove;
        private int _numberOfMovedGamePieces;

        public FillTimeoutBoardState(IBoardService boardService, IGamePieceService gamePieceService)
            : base(Settings.Timeouts.FillBoardTimeout)
        {
            _boardService = boardService;
            _gamePieceService = gamePieceService;
        }

        protected override void OnTimeoutEnded()
        {
            if (_gamePieceService.TryGetLowestRowWithEmptyGamePiece(out int lowestEmptyRow))
            {
                List<GamePiece> spawnedGamePieces =
                    _gamePieceService.FillBoardWithRandomGamePieces(lowestEmptyRow + Settings.FillBoardOffsetY);

                _numberOfGamePiecesToMove = spawnedGamePieces.Count;

                foreach (GamePiece spawnedGamePiece in spawnedGamePieces)
                {
                    spawnedGamePiece.Move(spawnedGamePiece.Position);
                    spawnedGamePiece.OnPositionChanged += OnGamePiecePositionChanged;
                }
            }
        }

        private void OnGamePiecePositionChanged(GamePiece gamePiece)
        {
            _numberOfMovedGamePieces++;

            if (_numberOfMovedGamePieces == _numberOfGamePiecesToMove)
            {
                if (_gamePieceService.HasAvailableMoves())
                {
                    _boardService.ChangeStateToWaiting();
                }
                else
                {
                    _gamePieceService.ClearBoard();
                    _boardService.ChangeStateToFill();
                }
            }
        }
    }
}