﻿using ConsoleTableExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCardApp
{
    internal class FormatTable
    {
        internal static void ShowStackTable<T>(List<T> tableData) where T : class
        {
            ConsoleTableBuilder
                .From(tableData)
                .WithTitle("Stacks")
                .ExportAndWriteLine();            
        }

        internal static void ShowCardTable<T>(List<T> tableData) where T : class
        {
            ConsoleTableBuilder
                .From(tableData)
                .WithTitle("Cards")
                .ExportAndWriteLine();
        }
    }
}