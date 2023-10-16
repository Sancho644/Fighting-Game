using System.Collections.Generic;
using System.Threading.Tasks;
using CameraLogic;
using Data;
using Hero;
using Infrastructure.Factory;
using Infrastructure.Services.PersistentProgress;
using Infrastructure.Services.PersistentProgress.SaveLoad;
using Logic;
using StaticData;
using UI.Elements;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure.Services
{
    public class InitGameWorldService : IInitGameWorldService
    {
        private const string LevelOne = "Level1";

        private IGameFactory _gameFactory;
        private IPersistentProgressService _progressService;
        private IStaticDataService _staticData;
        private ISaveLoadService _saveLoadService;

        public InitGameWorldService(IPersistentProgressService progressService,
            IStaticDataService staticData)
        {
            _progressService = progressService;
            _staticData = staticData;
        }

        public void Construct(IGameFactory gameFactory, ISaveLoadService saveLoadService)
        {
            _gameFactory = gameFactory;
            _saveLoadService = saveLoadService;
        }

        public async Task InitGameWorld()
        {
            var levelData = LevelStaticData();

            await InitSpawners(levelData);
            await InitLootPieces();
            GameObject hero = await InitHero(levelData);
            await InitHud(hero);

            CameraFollow(hero);
        }

        public PlayerProgress LoadProgress() =>
            _saveLoadService.LoadProgress();

        public PlayerProgress NewProgress()
        {
            PlayerProgress progress = new PlayerProgress(LevelOne);

            progress.HeroState.MaxHp = 50f;
            progress.HeroStats.Damage = 1f;
            progress.HeroStats.DamageRadius = 0.5f;
            progress.HeroState.ResetHp();

            return progress;
        }

        public void InformProgressReaders()
        {
            foreach (ISavedProgressReader progressReader in _gameFactory.ProgressReaders)
                progressReader.LoadProgress(_progressService.Progress);
        }

        private LevelStaticData LevelStaticData() =>
            _staticData.ForLevel(SceneManager.GetActiveScene().name);

        private async Task InitSpawners(LevelStaticData levelData)
        {
            foreach (EnemySpawnerData spawnerData in levelData.EnemySpawners)
                await _gameFactory.CreateSpawner(spawnerData.Position, spawnerData.Id, spawnerData.EnemyTypeId);
        }

        private async Task InitLootPieces()
        {
            foreach (KeyValuePair<string, LootPieceData> item in _progressService.Progress.WorldData.LootData
                         .LootPiecesOnScene.Dictionary)
            {
                var lootPiece = await _gameFactory.CreateLoot();
                lootPiece.GetComponent<UniqueId>().Id = item.Key;
                lootPiece.Initialize(item.Value.Loot);
                lootPiece.transform.position = item.Value.Position.AsUnityVector();
            }
        }

        private async Task<GameObject> InitHero(LevelStaticData levelData) =>
            await _gameFactory.CreateHero(levelData.InitialHeroPosition);

        private async Task InitHud(GameObject hero)
        {
            GameObject hud = await _gameFactory.CreateHud();

            hud.GetComponentInChildren<ActorUI>()
                .Construct(hero.GetComponent<HeroHealth>());
        }

        private void CameraFollow(GameObject hero) =>
            Camera.main.GetComponent<CameraFollow>().Follow(hero);
    }
}