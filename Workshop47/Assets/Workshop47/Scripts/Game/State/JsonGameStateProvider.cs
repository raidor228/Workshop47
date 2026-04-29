using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using R3;
using Workshop47.Scripts.Game.State.Entities;
using Workshop47.Scripts.Game.State.Entities.Upgradeable.Player;
using Workshop47.Scripts.Game.State.GameResources;
using Workshop47.Scripts.Game.State.Maps;
using Workshop47.Scripts.Game.State.Root;
using Workshop47.Scripts.Utils.Converters;

namespace Workshop47.Scripts.Game.State
{
    public class JsonGameStateProvider : IGameStateProvider
    {
        private const string GAME_STATE_KEY = "gamestate.json";
        private const string GAME_SETTINGS_STATE_KEY = "gamesettings.json";
        
        public GameState GameState { get; private set; }
        public GameSettingsState SettingsState { get; private set; }
        
        private GameStateData _gameStateOrigin;
        private GameSettingsStateData _gameSettingsStateOrigin;

        public JsonGameStateProvider()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
                Converters =
                {
                    new Vector3Converter()
                }
            };
        }
        
        public Observable<GameState> LoadGameState()
        {
            if (!File.Exists(GAME_STATE_KEY))
            {
                GameState = CreateGameStateFromSettings();
                SaveGameState();
            }
            else
            {
                string json = File.ReadAllText(GAME_STATE_KEY);

                try
                {
                    _gameStateOrigin = JsonConvert.DeserializeObject<GameStateData>(json);
                    GameState = new GameState(_gameStateOrigin);
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"Не удалось загрузить файл сохранения: {e}");
                    GameState = CreateGameStateFromSettings();
                    SaveGameState();
                }
            }

            return Observable.Return(GameState);
        }
        
        public Observable<GameSettingsState> LoadSettingsState()
        {
            if (!File.Exists(GAME_SETTINGS_STATE_KEY))
            {
                SettingsState = CreateGameSettingsStateFromSettings();
                SaveSettingsState();
            }
            else
            {
                var json = File.ReadAllText(GAME_SETTINGS_STATE_KEY);
                try
                {
                    _gameSettingsStateOrigin = JsonConvert.DeserializeObject<GameSettingsStateData>(json);
                    SettingsState = new GameSettingsState(_gameSettingsStateOrigin);
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"Не удалось загрузить файл настроек: {e}");
                    SettingsState = CreateGameSettingsStateFromSettings();
                    SaveSettingsState();
                }
            }

            return Observable.Return(SettingsState);
        }

        public Observable<bool> SaveGameState()
        {
            var json = JsonConvert.SerializeObject(_gameStateOrigin, Formatting.Indented);
            File.WriteAllText(GAME_STATE_KEY, json);
            
            return Observable.Return(true);
        }
        
        public Observable<bool> SaveSettingsState()
        {
            var json = JsonConvert.SerializeObject(_gameSettingsStateOrigin, Formatting.Indented);
            File.WriteAllText(GAME_SETTINGS_STATE_KEY, json);

            return Observable.Return(true);
        }

        public Observable<bool> ResetGameState()
        {
            GameState = CreateGameStateFromSettings();
            SaveGameState();
            
            return Observable.Return(true);
        }
        
        public Observable<GameSettingsState> ResetSettingsState()
        {
            SettingsState = CreateGameSettingsStateFromSettings();
            SaveSettingsState();
            
            return Observable.Return(SettingsState);
        }
        
        private GameState CreateGameStateFromSettings()
        {
            _gameStateOrigin = new GameStateData
            {
                Resources = new List<ResourceData>()
                {
                    new ResourceData()
                    {
                        ResourceType = ResourceType.Eidos,
                        Amount = 1000
                    },
                    new ResourceData()
                    {
                        ResourceType = ResourceType.Ether,
                        Amount = 1000
                    },
                    new ResourceData()
                    {
                        ResourceType = ResourceType.Rice,
                        Amount = 1000
                    },
                },
                Maps = new List<MapData>(),
            };
            _gameStateOrigin.Player = new PlayerEntityData()
            {
                UniqueId = _gameStateOrigin.CreateEntityId(),
                Type = EntityType.Player,
                Name = "Player Name",
                Health = 100,
                Level = 1,
                Position = new Vector3(0, 1, 0),
                Rotation = Vector3.zero
            };
                
            return new GameState(_gameStateOrigin);
        }
        
        private GameSettingsState CreateGameSettingsStateFromSettings()
        {
            _gameSettingsStateOrigin = new GameSettingsStateData()
            {
                MusicVolume = 100,
                SFXVolume = 100
            };
                
            return new GameSettingsState(_gameSettingsStateOrigin);
        }
    }
}