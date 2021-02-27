﻿//
//  ModelVisibleBlocks.cs
//
//  Author:
//       Jarl Gullberg <jarl.gullberg@gmail.com>
//
//  Copyright (c) 2017 Jarl Gullberg
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System.Collections.Generic;
using System.IO;
using Warcraft.Core.Interfaces;

namespace Warcraft.WMO.RootFile.Chunks
{
    /// <summary>
    /// Holds visible blocks.
    /// </summary>
    public class ModelVisibleBlocks : IIFFChunk, IBinarySerializable
    {
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
        public const string Signature = "MOVB";

        /// <summary>
        /// Gets the visible blocks.
        /// </summary>
        public List<VisibleBlock> VisibleBlocks { get; } = new List<VisibleBlock>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelVisibleBlocks"/> class.
        /// </summary>
        public ModelVisibleBlocks()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelVisibleBlocks"/> class.
        /// </summary>
        /// <param name="inData">The binary data.</param>
        public ModelVisibleBlocks(byte[] inData)
        {
            LoadBinaryData(inData);
        }

        /// <inheritdoc/>
        public void LoadBinaryData(byte[] inData)
        {
            using var ms = new MemoryStream(inData);
            using var br = new BinaryReader(ms);
            var visibleBlockCount = inData.Length / VisibleBlock.GetSize();
            for (var i = 0; i < visibleBlockCount; ++i)
            {
                VisibleBlocks.Add(new VisibleBlock(br.ReadBytes(VisibleBlock.GetSize())));
            }
        }

        /// <inheritdoc/>
        public string GetSignature()
        {
            return Signature;
        }

        /// <inheritdoc/>
        public byte[] Serialize()
        {
            using var ms = new MemoryStream();
            using (var bw = new BinaryWriter(ms))
            {
                foreach (var visibleBlock in VisibleBlocks)
                {
                    bw.Write(visibleBlock.Serialize());
                }
            }

            return ms.ToArray();
        }
    }
}
