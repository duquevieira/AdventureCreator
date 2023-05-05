using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MeadowGames.UINodeConnect4
{
    public interface IGraphElement : IElement
    {
        string ID { get; set; }
        Color ElementColor { get; set; }
    }
}