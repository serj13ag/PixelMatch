using PersistentData;
using UnityEngine;
using Random = System.Random;

namespace Controllers
{
    public class MainController : MonoBehaviour
    {
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private ParticleController _particleController;
        [SerializeField] private ScreenFaderController _screenFaderController;
        [SerializeField] private SceneController _sceneController;
        [SerializeField] private GameStateController _gameStateController;
        [SerializeField] private ScoreController _scoreController;

        [SerializeField] private Factory _factory;
        [SerializeField] private Board _board;

        [SerializeField] private TilesData _tilesData;
        [SerializeField] private GamePiecesData _gamePiecesData;
        [SerializeField] private ColorData _colorData;
        [SerializeField] private LevelData _levelData;
        [SerializeField] private MoveData _moveData;

        private Random _random;
        private GameDataRepository _gameDataRepository;

        private void Start()
        {
            _sceneController.UpdateSceneNameText();

            _screenFaderController.FadeOff();

            _random = new Random();
            _gameDataRepository =
                new GameDataRepository(_tilesData, _gamePiecesData, _colorData, _moveData, _levelData);

            _factory.Init(_random, _gameDataRepository, _particleController);
            _board.Init(_particleController, _factory, _random, _scoreController, _gameDataRepository);
            _gameStateController.Init(_board, _cameraController);

            _gameStateController.StartGame();
        }
    }
}