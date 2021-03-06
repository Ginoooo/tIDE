﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TileMapEditor.Plugin.Interface
{
    public interface IMenuItemCollection: IEnumerable<IMenuItem>
    {
        IMenuItem AddItem(string text);

        IMenuItem this[string text] { get; }
        IMenuItem this[int index] { get; }
    }
}
