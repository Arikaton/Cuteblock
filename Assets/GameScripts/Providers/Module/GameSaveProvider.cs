using System;
using GameScripts.Game;
using Newtonsoft.Json;
using UnityEngine;

namespace GameScripts.Providers
{
    public class GameSaveProvider : IGameSaveProvider
    {
        private const string GAME_SAVE_PREFS_KEY = "GAME_SAVE";
        
        public bool HasSavedGame { get; private set; }

        public GameSaveProvider()
        {
            HasSavedGame = !string.IsNullOrEmpty(PlayerPrefs.GetString(GAME_SAVE_PREFS_KEY, ""));
        }

        public void ClearSaveData()
        {
            PlayerPrefs.DeleteKey(GAME_SAVE_PREFS_KEY);
            HasSavedGame = false;
        }

        public void SaveGame(FieldModel fieldModel)
        {
            var fieldMatrix = new Flat2DArray<CellModelData>(9, 9);
            for (int i = 0; i < 81; i++)
            {
                var cellModel = fieldModel.FieldMatrix.array[i];
                fieldMatrix.array[i] = new CellModelData(cellModel.uid.Value, cellModel.hp.Value, cellModel.positionInShape, cellModel.shapeRotation);
            }
            
            var availableShapes = new ShapeModelData[3];
            for (int i = 0; i < 3; i++)
            {
                if (fieldModel.AvailableShapes[i] == null)
                {
                    availableShapes[i] = null;
                    continue;
                }
                availableShapes[i] = new ShapeModelData(fieldModel.AvailableShapes[i].Uid, fieldModel.AvailableShapes[i].Rotation.Value);
            }

            var fieldData = new FieldModelData(fieldMatrix, availableShapes, fieldModel.Score.Value);
            PlayerPrefs.SetString(GAME_SAVE_PREFS_KEY, JsonConvert.SerializeObject(fieldData));
            HasSavedGame = true;
        }

        public FieldModel LoadSavedGame()
        {
            var data = PlayerPrefs.GetString(GAME_SAVE_PREFS_KEY, "");
            if (string.IsNullOrEmpty(data))
                return new FieldModel();

            var savedData = JsonConvert.DeserializeObject<FieldModelData>(data);
            var fieldMatrix = new Flat2DArray<CellModel>(9, 9);
            for (int i = 0; i < 81; i++)
            {
                var cellData = savedData.fieldMatrix.array[i];
                fieldMatrix.array[i] = new CellModel(cellData.uid, cellData.hp, cellData.positionInShape, cellData.shapeRotation);
            }

            var availableShapes = new ShapeModel[3];
            for (int i = 0; i < 3; i++)
            {
                if(savedData.availableShapes[i] == null)
                {
                    availableShapes[i] = null;
                    continue;
                }
                availableShapes[i] = new ShapeModel(savedData.availableShapes[i].uid, savedData.availableShapes[i].rotation);
            }

            var fieldModel = new FieldModel(fieldMatrix, availableShapes, savedData.score);
            return fieldModel;
        }

        [Serializable]
        private class FieldModelData
        {
            public Flat2DArray<CellModelData> fieldMatrix;
            public ShapeModelData[] availableShapes;
            public int score;

            public FieldModelData(Flat2DArray<CellModelData> fieldMatrix, ShapeModelData[] availableShapes, int score)
            {
                this.fieldMatrix = fieldMatrix;
                this.availableShapes = availableShapes;
                this.score = score;
            }
        }

        [Serializable]
        private class ShapeModelData
        {
            public readonly int uid;
            public Rotation rotation;

            public ShapeModelData(int uid, Rotation rotation)
            {
                this.uid = uid;
                this.rotation = rotation;
            }
        }
        
        [Serializable]
        private class CellModelData
        {
            public int uid;
            public int hp;
            public Vector2Int positionInShape;
            public Rotation shapeRotation;

            public CellModelData(int uid, int hp, Vector2Int positionInShape, Rotation shapeRotation)
            {
                this.uid = uid;
                this.hp = hp;
                this.positionInShape = positionInShape;
                this.shapeRotation = shapeRotation;
            }
        }
    }
}