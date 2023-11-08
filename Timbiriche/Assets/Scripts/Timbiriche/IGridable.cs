using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Timbiriche
{
    public interface IGridable
    {
        void SetPosition(int x, int y);
        Vector2Int GetPosition();
    }
}