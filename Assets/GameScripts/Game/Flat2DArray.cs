using UnityEngine;

namespace GameScripts.Game
{
    [System.Serializable]
    public class Flat2DArray<T>
    {
        public T[] array;
        public int height;
        public int width;

        public Flat2DArray(int height, int width)
        {
            this.height = height;
            this.width = width;
            array = new T[height * width];
        }

        public T this[int x, int y]
        {
            get => array[x + y * width];
            set => array[x + y * width] = value;
        }
        
        public T this[Vector2Int position]
        {
            get => this[position.x, position.y];
            set => this[position.x, position.y] = value;
        }
    }
}