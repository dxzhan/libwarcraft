﻿//
//  ModelGroupNames.cs
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Warcraft.Core.Extensions;
using Warcraft.Core.Interfaces;
using Warcraft.WMO.GroupFile;

namespace Warcraft.WMO.RootFile.Chunks
{
    /// <summary>
    /// Holds the group names associated with the model.
    /// </summary>
    public class ModelGroupNames : IIFFChunk, IBinarySerializable
    {
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
        public const string Signature = "MOGN";

        /// <summary>
        /// Gets a dictionary of offsets that map to the group names.
        /// </summary>
        public Dictionary<long, string> GroupNames { get; } = new Dictionary<long, string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelGroupNames"/> class.
        /// </summary>
        public ModelGroupNames()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelGroupNames"/> class.
        /// </summary>
        /// <param name="inData">The binary data.</param>
        public ModelGroupNames(byte[] inData)
        {
            LoadBinaryData(inData);
        }

        /// <inheritdoc/>
        public void LoadBinaryData(byte[] inData)
        {
            using var ms = new MemoryStream(inData);
            using var br = new BinaryReader(ms);
            while (ms.Position < ms.Length)
            {
                GroupNames.Add(ms.Position, br.ReadNullTerminatedString());
            }
        }

        /// <inheritdoc/>
        public string GetSignature()
        {
            return Signature;
        }

        /// <summary>
        /// Gets the internal group name of the given group.
        /// </summary>
        /// <param name="modelGroup">The group.</param>
        /// <returns>The group name.</returns>
        /// <exception cref="ArgumentException">Thrown if the name was not found.</exception>
        public string GetInternalGroupName(ModelGroup modelGroup)
        {
            var internalNameOffset = (int)modelGroup.GetInternalNameOffset();
            if (GroupNames.ContainsKey(internalNameOffset))
            {
                return GroupNames[internalNameOffset];
            }

            throw new ArgumentException("Group name not found.", nameof(modelGroup));
        }

        /// <summary>
        /// Gets the internal descriptive group name of the given group.
        /// </summary>
        /// <param name="modelGroup">The group.</param>
        /// <returns>The descriptive group name.</returns>
        /// <exception cref="ArgumentException">Thrown if the name was not found.</exception>
        public string GetInternalDescriptiveGroupName(ModelGroup modelGroup)
        {
            var internalDescriptiveNameOffset = (int)modelGroup.GetInternalDescriptiveNameOffset();
            if (GroupNames.ContainsKey(internalDescriptiveNameOffset))
            {
                return GroupNames[internalDescriptiveNameOffset];
            }

            throw new ArgumentException("Descriptive group name not found.", nameof(modelGroup));
        }

        /// <inheritdoc/>
        public byte[] Serialize()
        {
            using var ms = new MemoryStream();
            using (var bw = new BinaryWriter(ms))
            {
                // Each block begins with two empty strings
                bw.Write('\0');
                bw.Write('\0');

                // Then the actual data
                for (var i = 0; i < GroupNames.Count; ++i)
                {
                    bw.WriteNullTerminatedString(GroupNames.ElementAt(i).Value);
                }

                // Then zero padding to an even 4-byte boundary at the end
                var count = 4 - (ms.Position % 4);
                if (count >= 4)
                {
                    return ms.ToArray();
                }

                for (long i = 0; i < count; ++i)
                {
                    bw.Write('\0');
                }
            }

            return ms.ToArray();
        }
    }
}
