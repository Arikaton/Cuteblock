using Sirenix.OdinInspector;
using UnityEngine;

namespace GameScripts.Game
{
    public class FieldView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _cellRenderer;

        [Button]
        public void SetupCells()
        {
            var counter = 0;
            var cellSize = _cellRenderer.size;
            var fieldWidth = cellSize.x * 9;
            var fieldHeight = cellSize.y * 9;
            
            for (var i = 0; i < 9; i++)
            {
                for (var j = 0; j < 9; j++)
                {
                    var cell = transform.GetChild(counter++);
                    var cellXPos = cellSize.x * j + cellSize.x / 2f - fieldWidth / 2f;
                    var cellYPos = cellSize.y * i + cellSize.y / 2f - fieldHeight / 2f;
                    cell.transform.localPosition = new Vector2(cellXPos, cellYPos);
                }
            }
        }

    }
}